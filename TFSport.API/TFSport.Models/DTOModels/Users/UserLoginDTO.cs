using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Users
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
