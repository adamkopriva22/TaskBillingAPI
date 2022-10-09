using System.ComponentModel.DataAnnotations;
using TaskBilling.Core.Enums;

namespace TaskBilling.API.Models
{
    public class OrderModel
    {
        [Required]
        public string? OrderNumber { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public decimal? Amount { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentGatewayType), ErrorMessage = "Payment gateway type doesn't exist within enum")]
        public PaymentGatewayType? PaymentGateway { get; set; }

        public string? Description { get; set; }        
    }
}
