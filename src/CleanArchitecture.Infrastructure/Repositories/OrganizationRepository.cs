using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly ApplicationDbContext _context;

    public OrganizationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Organization>> GetAllAsync()
    {
        return await _context.Organizations
            .Include(o => o.Status)
            .Include(o => o.PaymentFee)
            .Where(o => !o.IsDeleted)
            .ToListAsync();
    }

    public async Task<Organization?> GetByIdAsync(int id)
    {
        return await _context.Organizations
            .Include(o => o.Status)
            .Include(o => o.PaymentFee)
            .Include(o => o.Buyer)
            .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<Organization?> GetByCodeAsync(string code)
    {
        return await _context.Organizations
            .Include(o => o.Status)
            .Include(o => o.PaymentFee)
            .Include(o => o.Buyer)
            .FirstOrDefaultAsync(o => o.Code == code && !o.IsDeleted);
    }

    public async Task<Organization> AddAsync(Organization organization)
    {
        _context.Organizations.Add(organization);
        await _context.SaveChangesAsync();
        return organization;
    }

    public async Task<Organization> UpdateAsync(Organization organization)
    {
        organization.UpdatedAt = DateTime.UtcNow;
        _context.Organizations.Update(organization);
        await _context.SaveChangesAsync();
        return organization;
    }

    public async Task DeleteAsync(int id)
    {
        var organization = await _context.Organizations.FindAsync(id);
        if (organization != null)
        {
            organization.IsDeleted = true;
            organization.DeletedAt = DateTime.UtcNow;
            organization.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Organizations.AnyAsync(o => o.Id == id && !o.IsDeleted);
    }

    public async Task<bool> ExistsByCodeAsync(string code)
    {
        return await _context.Organizations.AnyAsync(o => o.Code == code && !o.IsDeleted);
    }
} 