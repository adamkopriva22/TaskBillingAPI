using Microsoft.Extensions.DependencyInjection;
using TaskBilling.Core.Enums;
using TaskBilling.Core.Interfaces;

namespace TaskBilling.Core.Services
{
    public class PaymentGatewayFactory : IPaymentGatewayFactory
    {
        private readonly IServiceProvider serviceProvider;

        public PaymentGatewayFactory(
            IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IPaymentGateway GetPaymentGateway(PaymentGatewayType type)
        {
            var paymentGatewayService = serviceProvider.GetServices<IPaymentGateway>()
                .FirstOrDefault(s => s.Type == type);

            if (paymentGatewayService == null)
            {
                throw new NotImplementedException($"No implementation for payment gateway with type {type} found!");
            }

            return paymentGatewayService;
        }
    }
}
