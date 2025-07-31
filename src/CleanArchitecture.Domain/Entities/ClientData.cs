using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class ClientData : BaseEntity
{
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    public int StatusId { get; set; } = 2;
    
    public bool IsFirstData { get; set; } = false;
    
    [Required]
    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string S3Key { get; set; } = string.Empty;
    
    public bool IsFavorite { get; set; } = false;
    
    public int? FileSize { get; set; }
    
    [MaxLength(50)]
    public string? FileType { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastUpdatedBy { get; set; } = string.Empty;
    
    // Navigation properties
    [ForeignKey(nameof(StatusId))]
    public virtual DataStatus Status { get; set; } = null!;
    
    public virtual ICollection<ClientDataMapping> ClientDataMappings { get; set; } = new List<ClientDataMapping>();
} 