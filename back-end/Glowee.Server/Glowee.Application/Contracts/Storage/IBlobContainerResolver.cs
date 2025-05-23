using Glowee.Application.Helpers;

namespace Glowee.Application.Contracts.Storage
{
    public interface IBlobContainerResolver
    {
        string GetContainerName(BlobContainerType type);
    }
}
