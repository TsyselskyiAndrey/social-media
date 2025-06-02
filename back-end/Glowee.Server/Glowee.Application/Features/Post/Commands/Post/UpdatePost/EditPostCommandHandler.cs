using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.PostMediaTypes;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Tags;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Commands.Post.UpdatePost;

public class EditPostCommandHandler : IRequestHandler<EditPostCommand>
{
    private readonly IPostRepository _postRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUserService _userService;
    private readonly IPostMediaStorageService _postMediaStorageService;

    public EditPostCommandHandler(IPostRepository postRepository
        , IPostMediaStorageService postMediaStorageService
        , ITagRepository tagRepository
        , IUserService userService)
    {
        _postRepository = postRepository;
        _postMediaStorageService = postMediaStorageService;
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

         if (request.Tags != null && request.Tags.Any())
         {
             var tagEntities = new List<Tag>();
             foreach (var tagName in request.Tags.Distinct())
             {
                 var tag = await _tagRepository.GetTagByNameAsync(tagName);
                 if (tag == null) throw new BadRequestException($"Tag '{tagName}' does not exist.");
                 tagEntities.Add(tag);
             }
             post.Tags = tagEntities;
         }

         var newFileNames = request.PostMedias.Select(f => f.FileName).ToHashSet(StringComparer.OrdinalIgnoreCase);

         var mediasToRemove = post.PostMedias
             .Where(pm => !newFileNames.Any(newName => pm.MediaPath.Contains(newName)))
             .ToList();

         foreach (var mediaToRemove in mediasToRemove)
         {
             await _postMediaStorageService.RemovePostMediaAsync(mediaToRemove.MediaPath);
             post.PostMedias.Remove(mediaToRemove);
         }

         var existingFileNames = post.PostMedias.Select(pm => Path.GetFileName(pm.MediaPath)).ToHashSet(StringComparer.OrdinalIgnoreCase);

         int position = post.PostMedias.Any() ? post.PostMedias.Max(pm => pm.Position ?? 0) + 1 : 1;

         foreach (var media in request.PostMedias)
         {
             if (existingFileNames.Contains(media.FileName))
             {
                 continue;
             }

             var ext = Path.GetExtension(media.FileName).ToLower();

             var newBlobName = await _postMediaStorageService.UploadPostMediaAsync(
                 media.OpenReadStream(),
                 media.FileName,
                 post.Id,
                 null 
             );

             PostMediaTypeId mediaType = GetMediaTypeByExtension(ext);

             post.PostMedias.Add(new PostMedia
             {
                 PostId = post.Id,
                 MediaPath = newBlobName,
                 Format = ext.Trim('.'),
                 Size = media.Length,
                 Position = position++,
                 IsUploaded = true,
                 PostMediaTypeId = mediaType
             });
         }

         if (request.Thumbnail != null)
         {
             var ext = Path.GetExtension(request.Thumbnail.FileName).ToLower();

             var firstVideo = post.PostMedias.FirstOrDefault(m => m.PostMediaTypeId == new PostMediaTypeId(2));
             if (firstVideo != null)
             {
                 var thumbBlobName = await _postMediaStorageService.UploadPostMediaAsync(
                     request.Thumbnail.OpenReadStream(),
                     request.Thumbnail.FileName,
                     post.Id,
                     firstVideo.ThumbnailPath 
                 );
                 firstVideo.ThumbnailPath = thumbBlobName;
             }
         }
        
         var mediaTypes = post.PostMedias.Select(pm => pm.PostMediaTypeId).Distinct().ToList();

         if (mediaTypes.Count == 1)
         {
             post.PostTypeId = mediaTypes[0] == new PostMediaTypeId(1)
                 ? new PostTypeId(1)
                 : new PostTypeId(2);
         }
         else if (mediaTypes.Count > 1)
         {
             post.PostTypeId = new PostTypeId(3);
         }
         else
         {
             post.PostTypeId = new PostTypeId(1);
         }

         await _postRepository.UpdateAsync(post);
    }

    private PostMediaTypeId GetMediaTypeByExtension(string ext)
    {
        var photoExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        var videoExtensions = new HashSet<string> { ".mp4", ".mov", ".avi", ".mkv", ".webm" };

        if (photoExtensions.Contains(ext)) return new PostMediaTypeId(1);
        if (videoExtensions.Contains(ext)) return new PostMediaTypeId(2);

        return new PostMediaTypeId(1);
    }
}