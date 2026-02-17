using System.ComponentModel.DataAnnotations;

namespace AuthService.Domain.Entitis;

public class Role
{
    [Key]
    [MaxLength(15)]
    public int Id { get; set; }
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}
