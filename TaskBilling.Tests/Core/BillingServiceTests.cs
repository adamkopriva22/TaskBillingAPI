using Microsoft.Extensions.Logging;
using Moq;
using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;
using TaskBilling.Core.Interfaces;
using TaskBilling.Core.Services;
using static TaskBilling.Tests.Core.BillingServiceTests.MockGatewaySuccess;

namespace TaskBilling.Tests.Core
{
    public class BillingServiceTests
    {
        public static IEnumerable<object[]> ProcessOrderAsyncTestData =>
            new List<object[]>
            {
                new object[] {
                    new Order
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = (PaymentGatewayType) int.MaxValue,
                        UserId = "xtest",
                        Amount = (decimal) 200.0,
                        Description = "My description"
                    },
                    new BillingResult(new Receipt()
                    {
                        Amount = (decimal) 200.0,
                    })
                },
                new object[] {
                    new Order
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = (PaymentGatewayType) int.MaxValue,
                        UserId = "xtest",
                        Amount = (decimal) 0.0,
                    },
                    new BillingResult(new Receipt()
                    {
                        Amount = (decimal) 0.0,
                    })
                }
            };

        [Theory]
        [MemberData(nameof(ProcessOrderAsyncTestData))]
        public async Task GIVEN_BillingService_WHEN_ProcessOrderAsync_Then_CorrectValueReturned(Order input, BillingResult expectedBillingResult)
        {
            // GIVEN
            var mockPaymentGatewayFactory = new Mock<IPaymentGatewayFactory>();

            mockPaymentGatewayFactory
                .Setup(m => m.GetPaymentGateway(It.IsAny<PaymentGatewayType>()))
                .Returns(new MockGatewaySuccess());

            var billingService = new BillingService(
                new Mock<ILogger<BillingService>>().Object,
                mockPaymentGatewayFactory.Object);

            // WHEN
            var result = await billingService.ProcessOrderAsync(input);

            // THEN
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedBillingResult);
        }

        public static IEnumerable<object[]> ProcessOrderAsyncWithFailure =>
            new List<object[]>
            {
                new object[] {
                    new Order
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = (PaymentGatewayType) int.MaxValue,
                        UserId = "xtest",
                        Amount = (decimal) 200.0,
                        Description = "My description"
                    },
                    new BillingResult("Failure")
                }
            };

        [Theory]
        [MemberData(nameof(ProcessOrderAsyncWithFailure))]
        public async Task GIVEN_BillingService_WHEN_ProcessOrderAsyncWithFailure_Then_CorrectValueReturned(Order input, BillingResult expectedBillingResult)
        {
            // GIVEN
            var mockPaymentGatewayFactory = new Mock<IPaymentGatewayFactory>();

            mockPaymentGatewayFactory
                .Setup(m => m.GetPaymentGateway(It.IsAny<PaymentGatewayType>()))
                .Returns(new MockGatewayFailure());

            var billingService = new BillingService(
                new Mock<ILogger<BillingService>>().Object,
                mockPaymentGatewayFactory.Object);

            // WHEN
            var result = await billingService.ProcessOrderAsync(input);

            // THEN
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedBillingResult);
        }

        public class MockGatewaySuccess : IPaymentGateway
        {
            public PaymentGatewayType Type => (PaymentGatewayType) int.MaxValue;

            public Task<ProcessResult> ProcessAsync(Order order)
            {
                return Task.FromResult(new ProcessResult());
            }

            public class MockGatewayFailure : IPaymentGateway
            {
                public PaymentGatewayType Type => (PaymentGatewayType)int.MaxValue;

                public Task<ProcessResult> ProcessAsync(Order order)
                {
                    return Task.FromResult(new ProcessResult("Failure"));
                }
            }
        }
    }
}
