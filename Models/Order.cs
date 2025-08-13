using System.ComponentModel.DataAnnotations;

namespace IBSRAStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OrderId { get; set; } = string.Empty;
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string CustomerEmail { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TaxAmount { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal SubtotalAmount { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Pending";
        
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public Product? Product { get; set; }
        public PaymentDetail? PaymentDetail { get; set; }
        public BillingAddress? BillingAddress { get; set; }
    }
}
