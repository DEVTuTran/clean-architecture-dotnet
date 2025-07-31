using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByCodeAsync(string code);
    Task<IEnumerable<Client>> GetByUserIdAsync(int userId);
    Task<Client> AddAsync(Client client);
    Task<Client> UpdateAsync(Client client);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByCodeAsync(string code);
} 