using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class UserSession : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string SessionToken { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? RefreshToken { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string DeviceId { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? DeviceName { get; set; }
    
    [Required]
    [MaxLength(45)]
    public string IpAddress { get; set; } = string.Empty;
    
    public string? UserAgent { get; set; }
    
    public DateTime TokenIssuedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public DateTime TokenExpiresAt { get; set; }
    
    public DateTime? RefreshTokenExpiresAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
} 