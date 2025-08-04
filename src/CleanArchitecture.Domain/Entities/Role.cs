using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Entities;

public class Role : BaseEntity
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    public string? Permissions { get; set; }

    public bool IsSystemRole { get; set; } = false;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();
}