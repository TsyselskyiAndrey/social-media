using Glowee.Application.Contracts.Storage;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.Messages;

namespace Glowee.Infrastructure.Storage
{
    public class MessageAttachmentStorageService : IMessageAttachmentStorageService
    {
        private readonly IBlobStorageService _blobStorageService;

        public MessageAttachmentStorageService(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public string GetMessageAttachmentUrl(string blobName)
        {
            return _blobStorageService.GetBlobUrl(BlobContainerType.MessageAttachments, blobName);
        }

        public async Task RemoveMessageAttachmentAsync(string blobName)
        {
            await _blobStorageService.RemoveBlobAsync(BlobContainerType.MessageAttachments, blobName);
        }

        public async Task<string> UploadMessageAttachmentAsync(Stream stream, string fileName, MessageId messageId, string? originalBlobName = null)
        {
            return await _blobStorageService.UploadBlob(BlobContainerType.MessageAttachments, stream, fileName, messageId.Value.ToString(), originalBlobName);
        }
    }
}
