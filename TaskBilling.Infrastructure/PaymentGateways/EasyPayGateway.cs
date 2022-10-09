using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;
using TaskBilling.Core.Interfaces;

namespace TaskBilling.Infrastructure.PaymentGateways
{
    public class EasyPayGateway : IPaymentGateway
    {
        public PaymentGatewayType Type => PaymentGatewayType.EasyPay;

        public Task<ProcessResult> ProcessAsync(Order order)
        {
            // TO DO
            // integrate with EasyPay

            return Task.FromResult(new ProcessResult());
        }
    }
}
