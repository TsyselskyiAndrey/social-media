using Glowee.Domain.Entities.Messages;

namespace Glowee.Application.Contracts.Storage
{
    public interface IMessageAttachmentStorageService
    {
        Task<string> UploadMessageAttachmentAsync(Stream stream, string fileName, MessageId messageId, string? originalBlobName = null);
        string GetMessageAttachmentUrl(string blobName);
        Task RemoveMessageAttachmentAsync(string blobName);
    }
}
