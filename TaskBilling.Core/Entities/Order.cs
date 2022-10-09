using TaskBilling.Core.Enums;

namespace TaskBilling.Core.Entities
{
    public class Order
    {
        public string OrderNumber { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public decimal Amount { get; set; }
        public PaymentGatewayType PaymentGateway { get; set; }
        public string? Description { get; set; }
    }
}
