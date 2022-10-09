using TaskBilling.Core.Entities;

namespace TaskBilling.Core.Interfaces
{
    public interface IBillingService
    {
        Task<BillingResult> ProcessOrderAsync(Order order);
    }
}