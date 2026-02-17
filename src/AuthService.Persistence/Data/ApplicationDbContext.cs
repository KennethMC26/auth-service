using AuthService.Domain.Entitis;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserEmail> UserEmails { get; set; }
    public DbSet<UserPasswordReset> UserPasswordResets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Snake Case
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
                entity.SetTableName(ToSnakeCase(tableName));

            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (!string.IsNullOrEmpty(columnName))
                    property.SetColumnName(ToSnakeCase(columnName));
            }
        }

        // 2. CONFIGURACIÓN DE USER (Ajustada a tu User.cs real)
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            
            // CORRECCIÓN 1: UserName con N mayúscula
            entity.HasIndex(e => e.UserName).IsUnique();

            // CORRECCIÓN 2: UserProfile (en lugar de Profile)
            entity.HasOne(e => e.UserProfile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // CORRECCIÓN 3: UserEmails con 's' al final
            entity.HasOne(e => e.UserEmails)
                .WithOne(ue => ue.User)
                .HasForeignKey<UserEmail>(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // CORRECCIÓN 4: UserPasswordResets con 's' al final
            entity.HasOne(e => e.UserPasswordResets)
                .WithOne(upr => upr.User)
                .HasForeignKey<UserPasswordReset>(upr => upr.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
        });
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) 
            ? "_" + x.ToString().ToLower() 
            : x.ToString().ToLower()));
    }
}