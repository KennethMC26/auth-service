public class UserProfile
{
    [Key]
    [MaxLength(16)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(16)]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;

    // CAMBIO 1: Renombrar de ProfilePictureUrl a ProfilePicture
    public string? ProfilePicture { get; set; } 

    // CAMBIO 2: Agregar la propiedad Phone que falta
    public string? Phone { get; set; }

    public string? Bio { get; set; }

    public DateTime? DateOfBirth { get; set; }

    // Relación
    public User User { get; set; } = null!;
}