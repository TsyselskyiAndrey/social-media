using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Storage
{
    public interface IProfileImageStorageService
    {
        Task<string> UploadProfileImageAsync(Stream stream, string fileName, UserId userId, string? originalBlobName = null);
        string GetProfileImageUrl(string blobName);
        Task RemoveProfileImageAsync(string blobName);
    }
}
