using System.ComponentModel.DataAnnotations;

namespace IBSRAStore.Models
{
    public class BillingAddress
    {
        public int Id { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Address { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public Order? Order { get; set; }
    }
}
