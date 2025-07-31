using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Domain.Entities;

public class Organization : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;
    
    [Required]
    public int StatusId { get; set; } = 2;
    
    public int MaxPaymentUsers { get; set; } = 0;
    
    [Required]
    public int PaymentFeeId { get; set; } = 1;
    
    public bool UseIpWhitelist { get; set; } = false;
    
    public bool IsDeleted { get; set; } = false;
    
    public DateTime? DeletedAt { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(StatusId))]
    public virtual OrganizationStatus Status { get; set; } = null!;
    
    [ForeignKey(nameof(PaymentFeeId))]
    public virtual PaymentFee PaymentFee { get; set; } = null!;
    
    public virtual Buyer? Buyer { get; set; }
    
    public virtual ICollection<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();
    
    public virtual ICollection<OrganizationIpWhitelist> IpWhitelists { get; set; } = new List<OrganizationIpWhitelist>();
    
    public virtual ICollection<PaymentCollection> PaymentCollections { get; set; } = new List<PaymentCollection>();
    
    public virtual ICollection<MaxUserRecord> MaxUserRecords { get; set; } = new List<MaxUserRecord>();
    
    public virtual ICollection<ClientDataMapping> ClientDataMappings { get; set; } = new List<ClientDataMapping>();
    
    public virtual ICollection<UserClient> UserClients { get; set; } = new List<UserClient>();
} 