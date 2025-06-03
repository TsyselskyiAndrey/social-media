using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Helpers;
using Glowee.Application.Models.Storage;
using Microsoft.Extensions.Options;

namespace Glowee.Infrastructure.Storage
{
    public class BlobContainerResolver : IBlobContainerResolver
    {
        private readonly BlobStorageContainerOptions _options;

        public BlobContainerResolver(IOptions<BlobStorageContainerOptions> options)
        {
            _options = options.Value;
        }

        public string GetContainerName(BlobContainerType type)
        {
            return type switch
            {
                BlobContainerType.MessageAttachments => _options.MessageAttachments,
                BlobContainerType.PostMedia => _options.PostMedia,
                BlobContainerType.ChatLogos => _options.ChatLogos,
                BlobContainerType.Thumbnails => _options.Thumbnails,
                BlobContainerType.ProfileImages => _options.ProfileImages,
                BlobContainerType.Downloads => _options.Downloads,
                _ => throw new InternalServerException()
            };
        }
    }
}
