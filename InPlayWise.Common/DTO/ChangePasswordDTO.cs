using System.ComponentModel.DataAnnotations;

namespace InPlayWiseCommon.DTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; }
    }
}
