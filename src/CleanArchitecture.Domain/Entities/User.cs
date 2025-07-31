using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(30)]
    public string Uid { get; set; } = string.Empty;
    
    [Required]
    public int PaymentPlanId { get; set; } = 1;
    
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    public DateTime? EmailVerifiedAt { get; set; }
    
    [MaxLength(255)]
    public string? PasswordHash { get; set; }
    
    [Required]
    public int RoleId { get; set; } = 1;
    
    public bool IsDisabled { get; set; } = false;
    
    public int ClientsCount { get; set; } = 0;
    
    public bool IsOwner { get; set; } = false;
    
    public DateTime? LastLoginAt { get; set; }
    
    [MaxLength(45)]
    public string? LastLoginIp { get; set; }
    
    public bool MustChangePassword { get; set; } = false;
    
    // Navigation properties
    [ForeignKey(nameof(PaymentPlanId))]
    public virtual PaymentPlan PaymentPlan { get; set; } = null!;
    
    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; } = null!;
    
    public virtual ICollection<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();
    
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
    
    public virtual ICollection<ClientDataMapping> ClientDataMappings { get; set; } = new List<ClientDataMapping>();
    
    public virtual ICollection<UserClient> UserClients { get; set; } = new List<UserClient>();
    
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    
    public virtual ICollection<MaxUserRecord> MaxUserRecords { get; set; } = new List<MaxUserRecord>();
    
    public virtual ICollection<OrganizationIpWhitelist> CreatedIpWhitelists { get; set; } = new List<OrganizationIpWhitelist>();
} 