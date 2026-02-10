using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entitis;

public class UserRole{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(36)]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(36)]
    [ForeignKey(nameof(Role))]
    public string RoleId { get; set; } = string.Empty;

    [Required]
    public DateTime AssignedDate { get; set; }

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
}