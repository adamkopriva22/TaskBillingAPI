using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;
using TaskBilling.Core.Interfaces;

namespace TaskBilling.Infrastructure.PaymentGateways
{
    public class FreePayGateway : IPaymentGateway
    {
        public PaymentGatewayType Type => PaymentGatewayType.FreePay;

        public Task<ProcessResult> ProcessAsync(Order order)
        {
            // TO DO
            // integrate with FreePay

            return Task.FromResult(new ProcessResult($"Processing by payment gateway {Type} failed"));
        }
    }
}
