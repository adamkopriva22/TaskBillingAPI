using Microsoft.AspNetCore.Mvc;
using TaskBilling.API.Extensions;
using TaskBilling.API.Extensions.Mappers;
using TaskBilling.API.Models;
using TaskBilling.Core.Interfaces;

namespace TaskBilling.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IBillingService billingService;

        public OrderController(IBillingService billingService)
        {
            this.billingService = billingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderModel orderModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await billingService.ProcessOrderAsync(orderModel.ToOrder());

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Receipt);
        }
    }
}
