using Glowee.Application.Contracts.FileProcessing;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Helpers;
using Glowee.Application.Models.Storage;
using Glowee.Domain.Entities.Users;
using Microsoft.Extensions.Options;

namespace Glowee.Infrastructure.Storage
{
    public class ProfileImageStorageService : IProfileImageStorageService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly DefaultFiles _defaultFiles;
        private readonly IImageProcessor _imageProcessor;

        public ProfileImageStorageService(IBlobStorageService blobStorageService, IOptions<DefaultFiles> defaultFiles, IImageProcessor imageProcessor)
        {
            _blobStorageService = blobStorageService;
            _defaultFiles = defaultFiles.Value;
            _imageProcessor = imageProcessor;
        }

        public string GetProfileImageUrl(string blobName)
        {
            return _blobStorageService.GetBlobUrl(BlobContainerType.ProfileImages, blobName);
        }

        public async Task RemoveProfileImageAsync(string blobName)
        {
            await _blobStorageService.RemoveBlobAsync(BlobContainerType.ProfileImages, blobName);
        }

        public async Task<string> UploadProfileImageAsync(Stream stream, string fileName, UserId userId, string? originalBlobName = null)
        {
            try
            {
                var processedStream = await _imageProcessor.ProcessSmallImageAsync(stream);

                string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.jpg";

                if (originalBlobName == _defaultFiles.DefaultProfilePicture)
                {
                    originalBlobName = null;
                }

                return await _blobStorageService.UploadBlob(
                    BlobContainerType.ProfileImages,
                    processedStream,
                    newFileName,
                    userId.Value.ToString(),
                    originalBlobName
                );
            }
            catch
            {
                throw new InternalServerException();
            }
        }
    }
}
