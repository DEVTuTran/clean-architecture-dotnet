using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class Buyer : BaseEntity
{
    [Required]
    public int OrganizationId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FamilyName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string GivenName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string FamilyNameKana { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string GivenNameKana { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(7)]
    public string ZipCode { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Prefecture { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? Department { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string PresidentFamilyName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string PresidentGivenName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string PresidentFamilyNameKana { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string PresidentGivenNameKana { get; set; } = string.Empty;
    
    public DateTime? Birthday { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; } = null!;
} 