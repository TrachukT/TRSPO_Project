using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Users
{
    public class RestorePasswordDTO
	{
		[MinLength(8, ErrorMessage = ErrorMessages.PasswordMinLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", ErrorMessage = ErrorMessages.PasswordValidation)]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = ErrorMessages.PasswordMatch)]
		public string RepeatPassword { get; set; }

	}
}
