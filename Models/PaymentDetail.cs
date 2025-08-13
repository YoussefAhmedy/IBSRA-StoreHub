using System.ComponentModel.DataAnnotations;

namespace IBSRAStore.Models
{
    public class PaymentDetail
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        [StringLength(4)]
        public string CardNumberLast4 { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string CardHolderName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(5)]
        public string ExpiryDate { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Credit Card";
        
        [StringLength(100)]
        public string? TransactionId { get; set; }
        
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public Order? Order { get; set; }
    }
}
