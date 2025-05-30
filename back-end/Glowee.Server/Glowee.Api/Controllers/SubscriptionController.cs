using Glowee.Api.Requests.Subscription;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Logging;
using Glowee.Application.Contracts.StripePayment;
using Glowee.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Glowee.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IStripePaymentService _stripePaymentService;
        private readonly IUserService _userService;
        private readonly IAppLogger<SubscriptionController> _appLogger;

        public SubscriptionController(IStripePaymentService stripePaymentService, IUserService userService, IAppLogger<SubscriptionController> appLogger)
        {
            _stripePaymentService = stripePaymentService;
            _userService = userService;
            _appLogger = appLogger;
        }

        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok(_stripePaymentService.Config());
        }

        [HttpPost("checkoutSession")]
        public async Task<IActionResult> GetCheckoutSessionAsync([FromBody] CreateCheckoutSessionRequest request)
        {
            var session = await _stripePaymentService.CreateCheckoutSessionAsync(request.PriceId);
            return Ok(session);
        }

        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradeSubscriptionAsync([FromBody] UpgradeSubscriptionRequest request)
        {
            var session = await _stripePaymentService.UpgradeSubscriptionAsync(request.PriceId);
            return Ok(session);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelSubscriptionAsync([FromBody] CancelSubscriptionRequest request)
        {
            await _stripePaymentService.CancelSubscriptionAsync(request.PriceId);
            return Ok();
        }

        [HttpGet("availableSubscriptions")]
        public async Task<IActionResult> GetAvailaleSubscriptionsAsync()
        {
            return Ok(await _stripePaymentService.GetAvailableSubscriptionsAsync());
        }

        [HttpGet("userSubscriptions")]
        public async Task<IActionResult> GetUserSubscriptionsAsync()
        {
            if (string.IsNullOrEmpty(_userService.UserId))
                throw new UnauthorizedAccessException("User must be authenticated to comment.");

            var userId = long.Parse(_userService.UserId!);
            var activeSubscriptions = await _stripePaymentService.GetUserSubscriptionsAsync(new UserId(userId));

            return Ok(activeSubscriptions);
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            Request.EnableBuffering();

            using var reader = new StreamReader(Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            var json = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var stripeSignature = Request.Headers["Stripe-Signature"];


            await _stripePaymentService.HandleStripeWebhookAsync(json, stripeSignature);
            return Ok();
        }
    }
}
