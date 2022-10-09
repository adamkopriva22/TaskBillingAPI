using TaskBilling.Core.Enums;

namespace TaskBilling.Core.Interfaces
{
    public interface IPaymentGatewayFactory
    {
        IPaymentGateway GetPaymentGateway(PaymentGatewayType type);
    }
}