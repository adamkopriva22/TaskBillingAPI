using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TaskBilling.Core.Interfaces;
using TaskBilling.Core.Services;

namespace TaskBilling.Core
{
    [ExcludeFromCodeCoverage]
    public static class ServicesConfiguration
    {
        public static void AddCoreServices(this IServiceCollection services) 
        {
            services.AddTransient<IBillingService, BillingService>();
            services.AddTransient<IPaymentGatewayFactory, PaymentGatewayFactory>();
        }
    }
}
