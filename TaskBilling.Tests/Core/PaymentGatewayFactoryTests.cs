using Microsoft.Extensions.DependencyInjection;
using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;
using TaskBilling.Core.Interfaces;
using TaskBilling.Core.Services;

namespace TaskBilling.Tests.Core
{
    public class PaymentGatewayFactoryTests
    {
        public static IEnumerable<object[]> GetPaymentGatewayTestData =>
            new List<object[]>
            {
                new object[] {
                    PaymentGatewayType.EasyPay
                },
                new object[] {
                    PaymentGatewayType.FreePay
                }
            };

        [Theory]
        [MemberData(nameof(GetPaymentGatewayTestData))]
        public void GIVEN_PaymentGatewayFactory_WHEN_GetPaymentGateway_Then_CorrectServiceReturned(PaymentGatewayType input)
        {
            // GIVEN
            ServiceProvider serviceProvider = GetMockServiceProvider();

            var paymentGatewayFactory = new PaymentGatewayFactory(serviceProvider);

            // WHEN
            var result = paymentGatewayFactory.GetPaymentGateway(input);

            // THEN
            result.Should().NotBeNull();
            result.Type.Should().Be(input);
        }

        public static IEnumerable<object[]> GetPaymentGatewayWithNonExistingServiceTestData =>
            new List<object[]>
            {
                new object[] {
                    (PaymentGatewayType) int.MaxValue
                }
            };

        [Theory]
        [MemberData(nameof(GetPaymentGatewayWithNonExistingServiceTestData))]
        public void GIVEN_PaymentGatewayFactory_WHEN_GetPaymentGatewayWithNonExistingService_Then_ExceptionThrown(PaymentGatewayType input)
        {
            // GIVEN
            ServiceProvider serviceProvider = GetMockServiceProvider();

            var paymentGatewayFactory = new PaymentGatewayFactory(serviceProvider);

            // WHEN
            Action act = () => paymentGatewayFactory.GetPaymentGateway(input);

            // THEN
            act.Should().Throw<NotImplementedException>();
        }

        private static ServiceProvider GetMockServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IPaymentGateway, MockGateway1>();
            serviceCollection.AddTransient<IPaymentGateway, MockGateway2>();

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }

    public class MockGateway1 : IPaymentGateway
    {
        public PaymentGatewayType Type => PaymentGatewayType.EasyPay;

        public Task<ProcessResult> ProcessAsync(Order order)
        {
            return Task.FromResult(new ProcessResult());
        }
    }

    public class MockGateway2 : IPaymentGateway
    {
        public PaymentGatewayType Type => PaymentGatewayType.FreePay;

        public Task<ProcessResult> ProcessAsync(Order order)
        {
            return Task.FromResult(new ProcessResult());
        }
    }
}
