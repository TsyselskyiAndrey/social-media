using Glowee.Api.Requests.Subscription;
using Glowee.Application.Contracts.StripePayment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IStripePaymentService _stripePaymentService;

        public SubscriptionController(IStripePaymentService stripePaymentService)
        {
            _stripePaymentService = stripePaymentService;
        }

        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok(_stripePaymentService.Config());
        }

        [HttpPost("checkoutSession")]
        public async Task<IActionResult> GetCheckoutSessionAsync([FromBody] CreateCheckoutSessionRequest request)
        {
            return Ok(await _stripePaymentService.CreateCheckoutSessionAsync(request.PriceId));
        }

        [HttpGet("availableSubscriptions")]
        public async Task<IActionResult> GetAvailaleSubscriptionsAsync()
        {
            return Ok(await _stripePaymentService.GetAvailableSubscriptionsAsync());
        }
    }
}
