using System.ComponentModel.DataAnnotations;
namespace AuthService.Domain.Entitis;

public class UserPasswordReset{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(36)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? PasswordResetToken { get; set; } = string.Empty;

    public DateTime? PasswordResetTokenExpiry { get; set; }

    [Required]
    public User User { get; set; } = null!;
}