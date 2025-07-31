using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace CleanArchitecture.Domain.Entities;

public class AuditLog : BaseEntity
{
    public int? UserId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string EntityType { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? EntityId { get; set; }
    
    public JsonDocument? OldValues { get; set; }
    
    public JsonDocument? NewValues { get; set; }
    
    [MaxLength(45)]
    public string? IpAddress { get; set; }
    
    public string? UserAgent { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
} 