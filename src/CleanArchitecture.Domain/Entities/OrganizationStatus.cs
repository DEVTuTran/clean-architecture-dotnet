using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class OrganizationStatus : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
} 