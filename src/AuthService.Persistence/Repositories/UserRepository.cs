using AuthService.Application.Services;
using AuthService.Domain.Entitis;
using AuthService.Domain.Interfaces;
using AuthService.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    //1. Busca un usuario por su ID
    public async Task<User> GetByIdAsync(string id)
    {
        var user = await context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.UserEmails)
            .Include(u => u.UserPasswordResets)
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user ?? throw new InvalidOperationException($"User with id {id} not found");
    }

    //2. Busca un usuario por su email
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.UserEmails)
            .Include(u => u.UserPasswordResets)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            // Corregido: El Email está en la tabla User, no en UserEmail
            .FirstOrDefaultAsync(u => EF.Functions.ILike(u.Email, email));
    }

    //3. Busca un usuario por su nombre de usuario
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.UserEmails)
            .Include(u => u.UserPasswordResets)
            .Include(u => u.UserRoles)
            // Corregido: Nombre de propiedad UserName
            .FirstOrDefaultAsync(u => EF.Functions.ILike(u.UserName, username));
    }

    //4. Busca un usuario mediante su token de verificacion de correo
    public async Task<User?> GetByEmailVerificationTokenAsync(string token)
    {
        return await context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.UserEmails)
            .Include(u => u.UserPasswordResets)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            // Corregido: Propiedad EmailVerificationToken
            .FirstOrDefaultAsync(u => u.UserEmails != null && u.UserEmails.EmailVerificationToken == token);
    }

    //5. Busca a un usuario mediante su token de restablecimiento de contraseña
    public async Task<User?> GetByPasswordResetTokenAsync(string token)
    {
        return await context.Users
            .Include(u => u.UserProfile)
            .Include(u => u.UserEmails)
            .Include(u => u.UserPasswordResets)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            // Corregido: Propiedad PasswordResetToken
            .FirstOrDefaultAsync(u => u.UserPasswordResets != null && u.UserPasswordResets.PasswordResetToken == token);
    }

    //6. crea un nuevo registro de usuario en la DB
    public async Task<User> CreateAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return await GetByIdAsync(user.Id);
    }

    //7. actualiza la informacion de un usuario existente
    public async Task<User> UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return await GetByIdAsync(user.Id);
    }

    //8. elimina un usuario de la DB
    public async Task<bool> DeleteAsync(string id)
    {
        var user = await GetByIdAsync(id);
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    //9. verifica si un email ya esta registrado
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await context.Users.AnyAsync(u => EF.Functions.ILike(u.Email, email));
    }

    //10. verifica si un nombre de usuario ya esta en uso
    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        return await context.Users.AnyAsync(u => EF.Functions.ILike(u.UserName, username));
    }

    //11. cambia el rol de un usuario
    public async Task UpdateUserRoleAsync(string userId, string roleId)
    {
        var existingRoles = await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .ToListAsync();

        context.UserRoles.RemoveRange(existingRoles);

        var newUserRole = new UserRole
        {
            // Nota: Si UuidGenerator sigue fallando, usa Guid.NewGuid().ToString()
            Id = Guid.NewGuid().ToString(), 
            UserId = userId,
            RoleId = roleId
        };

        context.UserRoles.Add(newUserRole);
        await context.SaveChangesAsync();
    }
}