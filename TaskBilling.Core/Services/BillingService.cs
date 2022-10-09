using Microsoft.Extensions.Logging;
using TaskBilling.Core.Entities;
using TaskBilling.Core.Interfaces;

namespace TaskBilling.Core.Services
{
    public class BillingService : IBillingService
    {
        private readonly ILogger<BillingService> logger;
        private readonly IPaymentGatewayFactory paymentGatewayFactory;

        public BillingService(
            ILogger<BillingService> logger,
            IPaymentGatewayFactory paymentGatewayFactory)
        {
            this.logger = logger;
            this.paymentGatewayFactory = paymentGatewayFactory;
        }

        public async Task<BillingResult> ProcessOrderAsync(Order order)
        {
            var paymentGateway = paymentGatewayFactory.GetPaymentGateway(order.PaymentGateway);

            var result = await paymentGateway.ProcessAsync(order);

            if (!result.Success)
            {
                logger.LogWarning($"Processing by payment gateway {order.PaymentGateway} failed - Message {result.Message}");
                return new BillingResult(result.Message);
            }

            return new BillingResult(
                new Receipt()
                {
                    Amount = order.Amount
                });
        }
    }
}
