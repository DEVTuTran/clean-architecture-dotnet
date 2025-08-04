using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed OrganizationStatus
        if (!await context.OrganizationStatuses.AnyAsync())
        {
            var statuses = new List<OrganizationStatus>
            {
                new() { Name = "Active", Description = "Organization is active" },
                new() { Name = "Inactive", Description = "Organization is inactive" },
                new() { Name = "Suspended", Description = "Organization is suspended" }
            };
            await context.OrganizationStatuses.AddRangeAsync(statuses);
        }

        // Seed PaymentFee
        if (!await context.PaymentFees.AnyAsync())
        {
            var fees = new List<PaymentFee>
            {
                new() { Fee = 0, Description = "Free tier", IsActive = true },
                new() { Fee = 10.00m, Description = "Basic tier", IsActive = true },
                new() { Fee = 25.00m, Description = "Premium tier", IsActive = true }
            };
            await context.PaymentFees.AddRangeAsync(fees);
        }

        // Seed PaymentPlan
        if (!await context.PaymentPlans.AnyAsync())
        {
            var plans = new List<PaymentPlan>
            {
                new() { Name = "Free", Description = "Free plan", PriceMonthly = 0, MaxUsers = 1, MaxClients = 5, IsActive = true },
                new() { Name = "Basic", Description = "Basic plan", PriceMonthly = 10.00m, MaxUsers = 5, MaxClients = 20, IsActive = true },
                new() { Name = "Premium", Description = "Premium plan", PriceMonthly = 25.00m, MaxUsers = 20, MaxClients = 100, IsActive = true }
            };
            await context.PaymentPlans.AddRangeAsync(plans);
        }

        // Seed Role
        if (!await context.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = "Admin", Description = "Administrator role", Permissions = "[\"read\",\"write\",\"delete\",\"admin\"]", IsSystemRole = true },
                new() { Name = "User", Description = "Regular user role", Permissions = "[\"read\",\"write\"]", IsSystemRole = true },
                new() { Name = "Viewer", Description = "View only role", Permissions = "[\"read\"]", IsSystemRole = true }
            };
            await context.Roles.AddRangeAsync(roles);
        }

        // Seed TaxType
        if (!await context.TaxTypes.AnyAsync())
        {
            var taxTypes = new List<TaxType>
            {
                new() { Name = "No Tax", Code = "NO_TAX", Rate = 0, IsActive = true, EffectiveFrom = DateTime.UtcNow },
                new() { Name = "Standard Tax", Code = "STANDARD", Rate = 10.00m, IsActive = true, EffectiveFrom = DateTime.UtcNow },
                new() { Name = "Reduced Tax", Code = "REDUCED", Rate = 5.00m, IsActive = true, EffectiveFrom = DateTime.UtcNow }
            };
            await context.TaxTypes.AddRangeAsync(taxTypes);
        }

        // Seed DataStatus
        if (!await context.DataStatuses.AnyAsync())
        {
            var dataStatuses = new List<DataStatus>
            {
                new() { Name = "Active", Description = "Data is active" },
                new() { Name = "Inactive", Description = "Data is inactive" },
                new() { Name = "Processing", Description = "Data is being processed" }
            };
            await context.DataStatuses.AddRangeAsync(dataStatuses);
        }

        await context.SaveChangesAsync();
    }
}