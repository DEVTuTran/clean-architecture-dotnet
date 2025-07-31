using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class Client : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastDataName { get; set; } = string.Empty;
    
    public bool IsFavorite { get; set; } = false;
    
    [Required]
    [MaxLength(100)]
    public string LastUpdatedBy { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual ICollection<ClientDataMapping> ClientDataMappings { get; set; } = new List<ClientDataMapping>();
    
    public virtual ICollection<UserClient> UserClients { get; set; } = new List<UserClient>();
} 