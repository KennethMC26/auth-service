using AuthService.Domain.Entitis;
using AuthService.Application.Services;
using AuthService.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Data;

public static class DataSeeder
{
   public static async Task SeedAsync(ApplicationDbContext context)
{
    // 1. Seed de Roles: Se ejecuta solo si la tabla de Roles está vacía
    if (!await context.Roles.AnyAsync())
    {
        var roles = new List<Role>
        {
            new() 
            {
                Id = UuidGenerator.GenerateRoleId(),
                Name = RoleConstants.ADMIN_ROLE
            },
            new() 
            {
                Id = UuidGenerator.GenerateRoleId(),
                Name = RoleConstants.USER_ROLE
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    // 2. Seed de Usuario Administrador: Se ejecuta solo si no existen usuarios
    if (!await context.Users.AnyAsync())
    {
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleConstants.ADMIN_ROLE);

        if (adminRole != null)
        {
            var userId = UuidGenerator.GenerateUserId();
            var profileId = UuidGenerator.GenerateUserId();
            var emailId = UuidGenerator.GenerateUserId();
            var userRoleId = UuidGenerator.GenerateUserId();

            var adminUser = new User
            {
                Id = userId,
                Name = "Admin",
                Surname = "User",
                UserName = "admin",
                Email = "admin@ksports.local",
                Password = "12345678",
                Status = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,

                // Relación uno a uno con Perfil
                UserProfile = new UserProfile
                {
                    Id = profileId,
                    UserId = userId
                },

                // Relación uno a uno con Email
                UserEmails = new UserEmail
                {
                    Id = emailId,
                    UserId = userId,
                    EmailVerified = true,
                    EmailVerificationToken = null,
                    EmailVerificationTokenExpiry = null
                },

                // Relación muchos a muchos (UserRole)
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Id = userRoleId,
                        UserId = userId,
                        RoleId = adminRole.Id
                    }
                }
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}
}
