namespace CleanArchitecture.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Uid { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? EmailVerifiedAt { get; set; }
    public int PaymentPlanId { get; set; }
    public string PaymentPlanName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }
    public int ClientsCount { get; set; }
    public bool IsOwner { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
    public bool MustChangePassword { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateUserDto
{
    public string Uid { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
    public int PaymentPlanId { get; set; } = 1;
    public int RoleId { get; set; } = 1;
    public bool IsOwner { get; set; } = false;
}

public class UpdateUserDto
{
    public string? Name { get; set; }
    public string? PasswordHash { get; set; }
    public int PaymentPlanId { get; set; }
    public int RoleId { get; set; }
    public bool IsDisabled { get; set; }
    public bool MustChangePassword { get; set; }
}