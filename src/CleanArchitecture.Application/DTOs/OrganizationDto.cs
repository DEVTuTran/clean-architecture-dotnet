namespace CleanArchitecture.Application.DTOs;

public class OrganizationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int MaxPaymentUsers { get; set; }
    public int PaymentFeeId { get; set; }
    public decimal PaymentFee { get; set; }
    public bool UseIpWhitelist { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateOrganizationDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int StatusId { get; set; } = 2;
    public int MaxPaymentUsers { get; set; } = 0;
    public int PaymentFeeId { get; set; } = 1;
    public bool UseIpWhitelist { get; set; } = false;
}

public class UpdateOrganizationDto
{
    public string Name { get; set; } = string.Empty;
    public int StatusId { get; set; }
    public int MaxPaymentUsers { get; set; }
    public int PaymentFeeId { get; set; }
    public bool UseIpWhitelist { get; set; }
}