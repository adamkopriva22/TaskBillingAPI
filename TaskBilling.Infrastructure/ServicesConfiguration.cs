using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TaskBilling.Core.Interfaces;
using TaskBilling.Infrastructure.PaymentGateways;

namespace TaskBilling.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ServicesConfiguration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IPaymentGateway, EasyPayGateway>();
            services.AddTransient<IPaymentGateway, FreePayGateway>();
        }
    }
}
