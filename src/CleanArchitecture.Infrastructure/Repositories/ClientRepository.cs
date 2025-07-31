using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client?> GetByCodeAsync(string code)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task<IEnumerable<Client>> GetByUserIdAsync(int userId)
    {
        return await _context.UserClients
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Client)
            .Select(uc => uc.Client)
            .ToListAsync();
    }

    public async Task<Client> AddAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<Client> UpdateAsync(Client client)
    {
        client.UpdatedAt = DateTime.UtcNow;
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Clients.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Clients.AnyAsync(c => c.Code == code);
    }
} 