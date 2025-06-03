using Glowee.Application.Contracts.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class DownloadsController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;

        public DownloadsController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        [HttpGet("getAndroidInstaller")]
        public IActionResult GetAndroidInstaller()
        {
            return Ok(_blobStorageService.GetBlobUrl(Application.Helpers.BlobContainerType.Downloads, "android_apk.apk"));
        }

    }
}
