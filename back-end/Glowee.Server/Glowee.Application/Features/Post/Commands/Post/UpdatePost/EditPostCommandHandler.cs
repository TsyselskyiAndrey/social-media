using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Tags;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Commands.Post.UpdatePost;

public class EditPostCommandHandler : IRequestHandler<EditPostCommand>
{
     private readonly IPostRepository _postRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ITagRepository _tagRepository;
    private readonly IUserService _userService;

    public EditPostCommandHandler(IPostRepository postRepository
        , IBlobStorageService blobStorageService
        , ITagRepository tagRepository
        , IUserService userService)
    {
        _postRepository = postRepository;
        _blobStorageService = blobStorageService;
        _tagRepository = tagRepository;
        _userService = userService;
    }

    public async Task Handle(EditPostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to update a post.");
        
        var post = await _postRepository.GetByIdAsync(new PostId(request.Id));
        if (post == null)
            throw new NotFoundException($"Post with ID {request.Id} not found.");

        if (!string.IsNullOrEmpty(request.Caption))
            post.Caption = request.Caption;

        var tagEntities = new List<Tag>();
        foreach (var tagName in request.Tags.Distinct())
        {
            var tag = await _tagRepository.GetTagByNameAsync(tagName);
            if (tag == null) throw new BadRequestException($"Tag '{tagName}' does not exist.");
            tagEntities.Add(tag);
        }

        
        var uploadedFiles = new List<(BlobContainerType containerType, string blobName)>();

        int position = 1;
        foreach (var media in request.PostMedias)
        {
            var ext = Path.GetExtension(media.FileName).ToLower();

            string? originalBlobName = post.PostMedias
                .FirstOrDefault(pm => pm.MediaPath.Contains(media.FileName))?.MediaPath;

            var newBlobName = await _blobStorageService.UploadBlob(
                BlobContainerType.PostMedia,
                media.OpenReadStream(),
                media.FileName,
                post.Id.ToString(),
                originalBlobName
            );

            uploadedFiles.Add((BlobContainerType.PostMedia, newBlobName));

            post.PostMedias.Add(new PostMedia
            {
                PostId = post.Id,
                MediaPath = newBlobName,
                Format = ext.Trim('.'),
                Size = media.Length,
                Position = position++,
                IsUploaded = true
            });
        }
        

        if (request.Thumbnail != null)
        {
            var ext = Path.GetExtension(request.Thumbnail.FileName).ToLower();
            var originalThumbBlobName = post.PostMedias
                .FirstOrDefault(pm => pm.MediaPath.Contains("thumb"))?.MediaPath;

            var thumbBlobName = await _blobStorageService.UploadBlob(
                BlobContainerType.PostMedia,
                request.Thumbnail.OpenReadStream(),
                request.Thumbnail.FileName,
                post.Id.ToString(),
                originalThumbBlobName
            );

            var firstVideo = post.PostMedias.FirstOrDefault(m => m.PostMediaTypeId.Value == 2);
            if (firstVideo != null)
                firstVideo.ThumbnailPath = thumbBlobName;
        }

        await _postRepository.UpdateAsync(post);
    }
}