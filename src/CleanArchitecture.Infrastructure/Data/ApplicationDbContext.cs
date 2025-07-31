using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<OrganizationStatus> OrganizationStatuses { get; set; }
    public DbSet<PaymentFee> PaymentFees { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<DataStatus> DataStatuses { get; set; }
    public DbSet<ClientData> ClientData { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<PaymentPlan> PaymentPlans { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ClientDataMapping> ClientDataMappings { get; set; }
    public DbSet<MaxUserRecord> MaxUserRecords { get; set; }
    public DbSet<OrganizationIpWhitelist> OrganizationIpWhitelists { get; set; }
    public DbSet<OrganizationUser> OrganizationUsers { get; set; }
    public DbSet<TaxType> TaxTypes { get; set; }
    public DbSet<PaymentCollection> PaymentCollections { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<UserClient> UserClients { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names
        modelBuilder.Entity<OrganizationStatus>().ToTable("organization_status");
        modelBuilder.Entity<PaymentFee>().ToTable("payment_fee");
        modelBuilder.Entity<Organization>().ToTable("organization");
        modelBuilder.Entity<Buyer>().ToTable("buyer");
        modelBuilder.Entity<DataStatus>().ToTable("data_status");
        modelBuilder.Entity<ClientData>().ToTable("client_data");
        modelBuilder.Entity<Client>().ToTable("client");
        modelBuilder.Entity<PaymentPlan>().ToTable("payment_plan");
        modelBuilder.Entity<Role>().ToTable("role");
        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<ClientDataMapping>().ToTable("client_data_mapping");
        modelBuilder.Entity<MaxUserRecord>().ToTable("max_user_record");
        modelBuilder.Entity<OrganizationIpWhitelist>().ToTable("organization_ip_whitelist");
        modelBuilder.Entity<OrganizationUser>().ToTable("organization_user");
        modelBuilder.Entity<TaxType>().ToTable("tax_type");
        modelBuilder.Entity<PaymentCollection>().ToTable("payment_collection");
        modelBuilder.Entity<UserSession>().ToTable("user_session");
        modelBuilder.Entity<UserClient>().ToTable("user_client");
        modelBuilder.Entity<AuditLog>().ToTable("audit_log");

        // Configure primary keys
        modelBuilder.Entity<Buyer>().HasKey(b => b.OrganizationId);

        // Configure relationships
        modelBuilder.Entity<Organization>()
            .HasOne(o => o.Status)
            .WithMany(s => s.Organizations)
            .HasForeignKey(o => o.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Organization>()
            .HasOne(o => o.PaymentFee)
            .WithMany(f => f.Organizations)
            .HasForeignKey(o => o.PaymentFeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Buyer>()
            .HasOne(b => b.Organization)
            .WithOne(o => o.Buyer)
            .HasForeignKey<Buyer>(b => b.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasOne(u => u.PaymentPlan)
            .WithMany(p => p.Users)
            .HasForeignKey(u => u.PaymentPlanId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ClientData>()
            .HasOne(cd => cd.Status)
            .WithMany(s => s.ClientData)
            .HasForeignKey(cd => cd.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentCollection>()
            .HasOne(pc => pc.Organization)
            .WithMany(o => o.PaymentCollections)
            .HasForeignKey(pc => pc.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PaymentCollection>()
            .HasOne(pc => pc.Tax)
            .WithMany(t => t.PaymentCollections)
            .HasForeignKey(pc => pc.TaxId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes
        modelBuilder.Entity<Organization>()
            .HasIndex(o => o.Code)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Uid)
            .IsUnique();

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.Code)
            .IsUnique();

        modelBuilder.Entity<ClientData>()
            .HasIndex(cd => cd.Code)
            .IsUnique();

        modelBuilder.Entity<PaymentCollection>()
            .HasIndex(pc => pc.TransactionId)
            .IsUnique();

        modelBuilder.Entity<UserSession>()
            .HasIndex(us => us.SessionToken)
            .IsUnique();

        // Configure JSON columns
        modelBuilder.Entity<Role>()
            .Property(r => r.Permissions)
            .HasColumnType("json");

        modelBuilder.Entity<PaymentCollection>()
            .Property(pc => pc.Details)
            .HasColumnType("json");

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.OldValues)
            .HasColumnType("json");

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.NewValues)
            .HasColumnType("json");
    }
}