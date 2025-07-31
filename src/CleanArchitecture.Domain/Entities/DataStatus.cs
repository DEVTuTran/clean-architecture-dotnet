using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class DataStatus : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    // Navigation properties
    public virtual ICollection<ClientData> ClientData { get; set; } = new List<ClientData>();
} 