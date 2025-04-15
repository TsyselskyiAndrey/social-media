using Glowee.Application.Contracts.FileProcessing;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Helpers;
using Glowee.Application.Models.Storage;
using Glowee.Domain.Entities.Chats;
using Microsoft.Extensions.Options;

namespace Glowee.Infrastructure.Storage
{
    public class ChatLogoStorageService : IChatLogoStorageService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly DefaultFiles _defaultFiles;
        private readonly IImageProcessor _imageProcessor;

        public ChatLogoStorageService(IBlobStorageService blobStorageService, IOptions<DefaultFiles> defaultFiles, IImageProcessor imageProcessor)
        {
            _blobStorageService = blobStorageService;
            _defaultFiles = defaultFiles.Value;
            _imageProcessor = imageProcessor;
        }

        public string GetChatLogoUrl(string blobName)
        {
            if (blobName == _defaultFiles.DefaultChatLogo)
            {
                throw new InternalServerException();
            }
            return _blobStorageService.GetBlobUrl(BlobContainerType.ChatLogos, blobName);
        }

        public async Task RemoveChatLogoAsync(string blobName)
        {
            await _blobStorageService.RemoveBlobAsync(BlobContainerType.ChatLogos, blobName);
        }

        public async Task<string> UploadChatLogoAsync(Stream stream, string fileName, ChatId chatId, string? originalBlobName = null)
        {
            try
            {
                var processedStream = await _imageProcessor.ProcessSmallImageAsync(stream);

                string newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.jpg";

                if (originalBlobName == _defaultFiles.DefaultChatLogo)
                {
                    originalBlobName = null;
                }

                return await _blobStorageService.UploadBlob(
                BlobContainerType.ChatLogos,
                    processedStream,
                    newFileName,
                    chatId.Value.ToString(),
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
