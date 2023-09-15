using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Dtos
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength =5)]
        public string NewPassword { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
