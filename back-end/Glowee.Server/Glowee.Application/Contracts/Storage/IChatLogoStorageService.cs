using Glowee.Domain.Entities.Chats;

namespace Glowee.Application.Contracts.Storage
{
    public interface IChatLogoStorageService
    {
        Task<string> UploadChatLogoAsync(Stream stream, string fileName, ChatId chatId, string? originalBlobName = null);
        string GetChatLogoUrl(string blobName);
        Task RemoveChatLogoAsync(string blobName);
    }
}
