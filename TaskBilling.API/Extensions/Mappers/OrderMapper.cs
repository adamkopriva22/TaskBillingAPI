using TaskBilling.API.Models;
using TaskBilling.Core.Entities;

namespace TaskBilling.API.Extensions.Mappers
{
    public static class OrderMapper
    {
        public static Order ToOrder(this OrderModel orderModel) => new Order
        {
            OrderNumber = orderModel.OrderNumber!,
            UserId = orderModel.UserId!,
            Amount = orderModel.Amount!.Value,
            PaymentGateway = orderModel.PaymentGateway!.Value,
            Description = orderModel.Description
        };
    }
}
