using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.DTOs;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}

public class FirebaseLoginRequestDto
{
    [Required]
    public string IdToken { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}

public class TokenRefreshRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

public class PasswordResetRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class PasswordChangeRequestDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    
    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;
}

// Additional DTOs for new Firebase methods
public class BanUserRequestDto
{
    [Required]
    public string Uid { get; set; } = string.Empty;
    
    public bool Disabled { get; set; } = true;
}

public class BanUserBulkRequestDto
{
    [Required]
    public string[] Uids { get; set; } = Array.Empty<string>();
    
    public bool Disabled { get; set; } = true;
}

public class EmailVerificationRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class EmailVerificationResponseDto
{
    public string VerificationLink { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class RevokeTokensRequestDto
{
    [Required]
    public string Uid { get; set; } = string.Empty;
}

public class UpdateUserProfileRequestDto
{
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? PhotoUrl { get; set; }
    public bool? EmailVerified { get; set; }
    public bool? Disabled { get; set; }
}

public class SetRoleRequestDto
{
    [Required]
    public string Uid { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = string.Empty;
    
    public Dictionary<string, object>? CustomClaims { get; set; }
}

public class SyncRoleRequestDto
{
    public string? Uid { get; set; }
    public bool SyncAll { get; set; } = false;
} 