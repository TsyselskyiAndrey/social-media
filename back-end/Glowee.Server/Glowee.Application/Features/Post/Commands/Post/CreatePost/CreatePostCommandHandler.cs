using System.Text.RegularExpressions;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Commands.Post.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand>
{
    private readonly IUserService _userService;
    private readonly ITagRepository _tagRepository;
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IUserService userService, IPostRepository postRepository
        , ITagRepository tagRepository)
    {
        _userService = userService;
        _postRepository = postRepository;
        _tagRepository = tagRepository;
    }
    
    public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to create a post.");

        var validator = new CreatePostValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException(string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

        // var mediaStreams = request.PostMedias
        //     .Select(file => (file.OpenReadStream(), file.FileName, file.Length))
        //     .ToList();
        var mediaStreams = new List<(Stream stream, string fileName, long size)>();

        foreach (var file in request.PostMedias)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            mediaStreams.Add((memoryStream, file.FileName, memoryStream.Length));
        }

        var postTypeId = DeterminePostTypeId(request.PostMedias);

        var tagEntities = new List<Tag>();
        foreach (var tagName in request.Tags.Distinct())
        {
            var tag = await _tagRepository.GetTagByNameAsync(tagName);
            if (tag == null)
                throw new BadRequestException($"Tag '{tagName}' does not exist.");
            tagEntities.Add(tag);
        }

        var post = new Domain.Entities.Posts.Post
        {
            UserId = new UserId(long.Parse(_userService.UserId)),
            Caption = request.Caption
        };

        Stream? thumbnailStream = request.Thumbnail?.OpenReadStream();
        string? thumbnailFileName = request.Thumbnail?.FileName;

        await _postRepository.CreatePostWithPostMediaAsync(
            post,
            mediaStreams,
            thumbnailStream,
            thumbnailFileName,
            tagEntities,
            postTypeId
        );
    }
    
    private PostTypeId DeterminePostTypeId(ICollection<IFormFile> mediaFiles)
    {
        if (mediaFiles.Count > 1)
            return new PostTypeId(3); 

        var ext = Path.GetExtension(mediaFiles.First().FileName).ToLower();
        return Regex.IsMatch(ext, @"\.(jpg|jpeg|png|gif)$")
            ? new PostTypeId(1)
            : new PostTypeId(2);
    }

}