using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entitis; // Cambiado de 'Entitis' a 'Entities'

public class User {
    [Key]
    [MaxLength(36)] // Aumentado a 36 para soportar GUIDs si fuera necesario
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [MaxLength(25)]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty; // Cambiado 'Username' -> 'UserName' (N mayúscula)

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)] // El password suele ser más largo por el Hash
    public string Password { get; set; } = string.Empty;
    
    [Required]
    public bool Status { get; set; } = true;

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    // --- PROPIEDADES DE NAVEGACIÓN (Sincronizadas con ApplicationDbContext) ---
    
    public UserProfile UserProfile { get; set; } = null!; // Cambiado 'Profile' -> 'UserProfile'

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public UserEmail UserEmails { get; set; } = null!; // Cambiado 'UserEmail' -> 'UserEmails' (Plural)

    public UserPasswordReset UserPasswordResets { get; set; } = null!; // Cambiado 'PasswordReset' -> 'UserPasswordResets'
}