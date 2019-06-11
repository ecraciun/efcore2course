namespace SnackMachine.Domain
{
    public class PurchaseMadeEvent : IDomainEvent
    {
        public decimal Amount { get; set; }
        public long SnackId { get; set; }

        public PurchaseMadeEvent(decimal amount, long snackId)
        {
            Amount = amount;
            SnackId = snackId;
        }
    }
}