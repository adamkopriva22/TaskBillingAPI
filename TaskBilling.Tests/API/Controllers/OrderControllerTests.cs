using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using TaskBilling.API.Controllers;
using TaskBilling.API.Models;
using TaskBilling.Core.Entities;
using TaskBilling.Core.Enums;
using TaskBilling.Core.Interfaces;

namespace TaskBilling.Tests.API.Controllers
{
    public class OrderControllerTests
    {
        public static IEnumerable<object[]> CreateOrderAsyncTestData =>
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
                    new BillingResult(new Receipt()
                    {
                        Amount = (decimal) 200.0,
                    })
                },
                new object[] {
                    new OrderModel
                    {
                        OrderNumber = "AB_123456",
                        PaymentGateway = PaymentGatewayType.FreePay,
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
        [MemberData(nameof(CreateOrderAsyncTestData))]
        public async Task GIVEN_OrderController_WHEN_CreateOrderAsync_Then_CorrectValueReturned(OrderModel input, BillingResult expectedBillingResult)
        {
            // GIVEN
            var mockBillingService = new Mock<IBillingService>();

            mockBillingService
                .Setup(m => m.ProcessOrderAsync(It.IsAny<Order>()))
                .Returns(Task.FromResult(expectedBillingResult));

            var controller = new OrderController(mockBillingService.Object);
            MockModelState(input, controller);

            // WHEN
            var result = await controller.CreateOrderAsync(input);

            // THEN
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new OkObjectResult(expectedBillingResult.Receipt));
        }

        public static IEnumerable<object[]> CreateOrderAsyncModelStateValidationTestData =>
            new List<object[]>
            {
                new object[] {
                    new OrderModel
                    {
                    },
                    new List<string>
                    {
                        { "The Amount field is required." },
                        { "The UserId field is required." },
                        { "The OrderNumber field is required." },
                        { "The PaymentGateway field is required." }
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        Amount = (decimal) 200.0
                    },
                    new List<string>
                    {
                        { "The UserId field is required." },
                        { "The OrderNumber field is required." },
                        { "The PaymentGateway field is required." }
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        Amount = (decimal) 200.0,
                        UserId = "xuserid"
                    },
                    new List<string>
                    {                        
                        { "The OrderNumber field is required." },
                        { "The PaymentGateway field is required." }
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        Amount = (decimal) 200.0,
                        UserId = "xuserid",
                        OrderNumber = "OP_123456"
                    },
                    new List<string>
                    {
                        { "The PaymentGateway field is required." }
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        OrderNumber = "OP_123456",
                        PaymentGateway = PaymentGatewayType.EasyPay
                    },
                    new List<string>
                    {
                        { "The Amount field is required." },
                        { "The UserId field is required." },
                    }
                },
                new object[] {
                    new OrderModel
                    {
                        Amount = (decimal) 200.0,
                        OrderNumber = "OP_123456",
                        PaymentGateway = (PaymentGatewayType) int.MaxValue
                    },
                    new List<string>
                    {
                        { "The UserId field is required." },
                        { "Payment gateway type doesn't exist within enum" }
                    }
                },
            };

        [Theory]
        [MemberData(nameof(CreateOrderAsyncModelStateValidationTestData))]
        public async Task GIVEN_OrderController_WHEN_CreateOrderAsyncWithInvalidInputDate_Then_CorrectValueReturned(OrderModel input, List<string> errors)
        {
            // GIVEN
            var controller = new OrderController(new Mock<IBillingService>().Object);
            MockModelState(input, controller);

            // WHEN
            var result = await controller.CreateOrderAsync(input);

            // THEN
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new BadRequestObjectResult(errors));
        }      

        public static IEnumerable<object[]> CreateOrderAsyncWithUnsucesfullBillingServiceResultTestData =>
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
                    "Failure"
                }
            };

        [Theory]
        [MemberData(nameof(CreateOrderAsyncWithUnsucesfullBillingServiceResultTestData))]
        public async Task GIVEN_OrderController_WHEN_CreateOrderAsyncWithUnsucesfullBillingServiceResult_Then_CorrectValueReturned(OrderModel input, string expectedMessage)
        {
            // GIVEN
            var mockBillingService = new Mock<IBillingService>();

            mockBillingService
                .Setup(m => m.ProcessOrderAsync(It.IsAny<Order>()))
                .Returns(Task.FromResult(new BillingResult(expectedMessage)));

            var controller = new OrderController(mockBillingService.Object);
            MockModelState(input, controller);

            // WHEN
            var result = await controller.CreateOrderAsync(input);

            // THEN
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new BadRequestObjectResult(expectedMessage));
        }

        private void MockModelState<TModel, TController>(TModel model, TController controller) where TController : ControllerBase
        {
            var validationContext = new ValidationContext(model!, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model!, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage!);
            }
        }
    }
}
