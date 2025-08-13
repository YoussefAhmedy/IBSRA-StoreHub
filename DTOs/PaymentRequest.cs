using System.ComponentModel.DataAnnotations;

namespace IBSRAStore.DTOs
{
    public class PaymentRequest
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        
        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = "USD";
        
        [Required]
        public PaymentMethodDto PaymentMethod { get; set; } = new();
        
        [Required]
        public BillingAddressDto BillingAddress { get; set; } = new();
    }

    public class PaymentMethodDto
    {
        [Required]
        [StringLength(19)]
        public string CardNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(5)]
        public string ExpiryDate { get; set; } = string.Empty;
        
        [Required]
        [StringLength(3)]
        public string Cvv { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string HolderName { get; set; } = string.Empty;
    }

    public class BillingAddressDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; } = string.Empty;
    }
}
