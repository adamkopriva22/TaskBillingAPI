namespace TaskBilling.Core.Entities
{
    public class BillingResult : ProcessResult
    {
        public BillingResult(Receipt receipt) : base()
        {
            Receipt = receipt;
        }

        public BillingResult(string? message) : base(message) { }

        public Receipt? Receipt { get; set; }
    }
}
