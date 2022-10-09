using TaskBilling.API.Extensions.Mappers;
using TaskBilling.API.Models;
using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;

namespace TaskBilling.Tests.API.Extensions.Mappers
{
    public class OrderMapperTests
    {
        public static IEnumerable<object[]> ToOrderTestData =>
            new List<object[]>
            {
                new object[] {
                    new OrderModel
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = PaymentGatewayType.EasyPay,
                        UserId = "xtest",
                        Amount = (decimal) 200.0,
                        Description = "My description"
                    },
                    new Order
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = PaymentGatewayType.EasyPay,
                        UserId = "xtest",
                        Amount = (decimal) 200.0,
                        Description = "My description"
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        OrderNumber = "",
                        PaymentGateway = PaymentGatewayType.FreePay,
                        UserId = "",
                        Amount = (decimal) 0,
                        Description = ""
                    },
                    new Order
                    {
                        OrderNumber = "",
                        PaymentGateway = PaymentGatewayType.FreePay,
                        UserId = "",
                        Amount = (decimal) 0,
                        Description = ""
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = PaymentGatewayType.FreePay,
                        UserId = "xtest",
                        Amount = (decimal) 200.0,
                    },
                    new Order
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = PaymentGatewayType.FreePay,
                        UserId = "xtest",
                        Amount = (decimal) 200.0,
                    }
                }
            };

        [Theory]
        [MemberData(nameof(ToOrderTestData))]
        public void GIVEN_OrderModel_WHEN_ToOrder_Then_CorrectlyMapped(OrderModel input, Order expectedValue)
        {
            // GIVEN
            // WHEN
            var result = input.ToOrder();

            // THEN
            result.Should().BeEquivalentTo(expectedValue);
        }
    }
}
