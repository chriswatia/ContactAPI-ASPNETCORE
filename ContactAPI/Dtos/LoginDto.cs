using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Dtos
{
    public class LoginDto
    {
        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; } = string.Empty;
    }
}
