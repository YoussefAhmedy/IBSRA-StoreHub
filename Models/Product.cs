using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace IBSRAStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal OriginalPrice { get; set; }
        
        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
        
        [Required]
        [Range(0, 5)]
        public decimal Rating { get; set; }
        
        [Required]
        public int ReviewCount { get; set; }
        
        [Required]
        public string Features { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Helper property to get features as list
        public List<string> FeaturesList => 
            string.IsNullOrEmpty(Features) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(Features) ?? new List<string>();
    }
}
