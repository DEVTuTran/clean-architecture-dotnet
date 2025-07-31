using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class PaymentFee : BaseEntity
{
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Fee { get; set; } = 10000.00m;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
} 