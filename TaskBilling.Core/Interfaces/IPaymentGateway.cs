using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;

namespace TaskBilling.Core.Interfaces
{
    public interface IPaymentGateway
    {
        public PaymentGatewayType Type { get; }

        public Task<ProcessResult> ProcessAsync(Order order);
    }
}
