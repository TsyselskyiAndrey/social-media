using Glowee.Application.Contracts.Storage;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.Posts;

namespace Glowee.Infrastructure.Storage
{
    public class ThumbnailStorageService : IThumbnailStorageService
    {
        private readonly IBlobStorageService _blobStorageService;

        public ThumbnailStorageService(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public string GetThumbnailUrl(string blobName)
        {
            return _blobStorageService.GetBlobUrl(BlobContainerType.Thumbnails, blobName);
        }

        public async Task RemoveThumbnailAsync(string blobName)
        {
            await _blobStorageService.RemoveBlobAsync(BlobContainerType.Thumbnails, blobName);
        }

        public async Task<string> UploadThumbnailAsync(Stream stream, string fileName, PostId postId, string? originalBlobName = null)
        {
            return await _blobStorageService.UploadBlob(BlobContainerType.Thumbnails, stream, fileName, postId.Value.ToString(), originalBlobName);
        }
    }
}
