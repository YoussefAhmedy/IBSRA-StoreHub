namespace IBSRAStore.DTOs
{
    public class OrderDetailsDto
    {
        public string OrderId { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubtotalAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; } = string.Empty;
        public string CardNumberLast4 { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
