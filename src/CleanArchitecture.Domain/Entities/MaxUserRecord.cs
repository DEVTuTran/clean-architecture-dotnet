using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class MaxUserRecord : BaseEntity
{
    [Required]
    public int OrganizationId { get; set; }
    
    [Required]
    public int MaxPaymentUsers { get; set; }
    
    public int? ChangedBy { get; set; }
    
    [MaxLength(255)]
    public string? Reason { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; } = null!;
    
    [ForeignKey(nameof(ChangedBy))]
    public virtual User? ChangedByUser { get; set; }
} 