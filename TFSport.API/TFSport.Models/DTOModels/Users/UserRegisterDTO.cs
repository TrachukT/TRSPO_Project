using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Users
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = ErrorMessages.FirstNameIsRequired)]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = ErrorMessages.NamesValidation)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = ErrorMessages.LastNameIsRequired)]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = ErrorMessages.NamesValidation)]
        public string LastName { get; set; }

        [Required(ErrorMessage = ErrorMessages.EmailIsRequired)]
        [EmailAddress(ErrorMessage = ErrorMessages.EmailNotValid)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessages.PasswordIsRequired)]
        [MinLength(8, ErrorMessage = ErrorMessages.PasswordMinLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = ErrorMessages.PasswordValidation)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = ErrorMessages.PasswordMatch)]
        public string RepeatPassword { get; set; }

    }
}
