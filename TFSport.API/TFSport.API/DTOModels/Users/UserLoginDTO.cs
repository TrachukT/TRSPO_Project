using System.ComponentModel.DataAnnotations;

namespace TFSport.Models
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = ErrorMessages.EmailIsRequired)]
        [EmailAddress(ErrorMessage = ErrorMessages.EmailNotValid)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessages.PasswordIsRequired)]
        public string Password { get; set; }
    }
}
