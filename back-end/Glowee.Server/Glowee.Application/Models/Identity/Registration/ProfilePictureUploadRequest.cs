using Microsoft.AspNetCore.Http;

namespace Glowee.Application.Models.Identity.Registration
{
    public class ProfilePictureUploadRequest
    {
        public IFormFile File { get; set; } = null!;
    }
}
