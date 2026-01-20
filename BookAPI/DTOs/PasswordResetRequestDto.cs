using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs;

public class PasswordResetRequestDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
}
