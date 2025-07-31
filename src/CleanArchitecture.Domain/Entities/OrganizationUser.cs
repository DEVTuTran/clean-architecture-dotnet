using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class OrganizationUser : BaseEntity
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int OrganizationId { get; set; }
    
    public int? RoleId { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    public int? InvitedBy { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
    
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; } = null!;
    
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
    
    [ForeignKey(nameof(InvitedBy))]
    public virtual User? InvitedByUser { get; set; }
} 