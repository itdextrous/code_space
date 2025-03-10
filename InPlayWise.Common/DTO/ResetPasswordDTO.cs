using System.ComponentModel.DataAnnotations;

namespace InPlayWiseCommon.DTOs
{
    public class ResetPasswordDTO
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }


    }
}
