using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class ClientDataMapping : BaseEntity
{
    [Required]
    public int ClientId { get; set; }
    
    [Required]
    public int ClientDataId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int OrganizationId { get; set; }
    
    [Required]
    [Range(1, 3)]
    public int AccessLevel { get; set; } = 1; // 1: Read, 2: Write, 3: Owner
    
    // Navigation properties
    [ForeignKey(nameof(ClientId))]
    public virtual Client Client { get; set; } = null!;
    
    [ForeignKey(nameof(ClientDataId))]
    public virtual ClientData ClientData { get; set; } = null!;
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;
    
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; } = null!;
} 