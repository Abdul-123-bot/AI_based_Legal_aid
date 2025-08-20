using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models;

public class UserProfile{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email {get; set;}

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
