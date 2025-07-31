using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class TaxType : BaseEntity
{
    [Required]
    [Range(0, 100)]
    public decimal Rate { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = true;
    
    [Required]
    public DateTime EffectiveFrom { get; set; }
    
    public DateTime? EffectiveTo { get; set; }
    
    // Navigation properties
    public virtual ICollection<PaymentCollection> PaymentCollections { get; set; } = new List<PaymentCollection>();
} 