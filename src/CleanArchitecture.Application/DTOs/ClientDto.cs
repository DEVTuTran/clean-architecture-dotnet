namespace CleanArchitecture.Application.DTOs;

public class ClientDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string LastDataName { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public string LastUpdatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateClientDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string LastDataName { get; set; } = string.Empty;
    public bool IsFavorite { get; set; } = false;
    public string LastUpdatedBy { get; set; } = string.Empty;
}

public class UpdateClientDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string LastDataName { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public string LastUpdatedBy { get; set; } = string.Empty;
}