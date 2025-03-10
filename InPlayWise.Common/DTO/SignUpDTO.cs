using System.ComponentModel.DataAnnotations;

namespace InPlayWiseCommon.DTOs
{
    public class SignUpDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }

        [Required]
        public string Role { get; set; } = "User";

    }
}
