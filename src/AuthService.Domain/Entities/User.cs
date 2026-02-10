using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entitis;

public class User{
    [Key]
    [MaxLength(16)]
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [MaxLength(25)]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public bool Status { get; set; } = true;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public UserProfile Profile { get; set; } = null!;

    public ICollection<UserRole> UserRoles { get; set; } = [];
    public UserEmail UserEmail { get; set; } = null!;
    public UserPasswordReset PasswordReset { get; set; } = null!;
}