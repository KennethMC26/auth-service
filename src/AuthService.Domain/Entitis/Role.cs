using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entitis;

public class Role
{
    [Key]
    [MaxLength(15)]
    public string Id { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100, ErrorMessage = "El nombre del rol debe tener menos de 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
