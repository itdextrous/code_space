using System.ComponentModel.DataAnnotations;

namespace InPlayWiseCommon.DTOs
{
    public class SignInDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
