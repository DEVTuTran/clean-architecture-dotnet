using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class PaymentPlan : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public decimal PriceMonthly { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? PriceYearly { get; set; }
    
    public int? MaxUsers { get; set; }
    
    public int? MaxClients { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
} 