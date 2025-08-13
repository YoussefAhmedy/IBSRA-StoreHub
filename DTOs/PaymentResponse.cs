namespace IBSRAStore.DTOs
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? OrderId { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
    }
}
