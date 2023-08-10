using System.ComponentModel.DataAnnotations;
using TFSport.Models;

namespace TFSport.API.DTOModels.Users
{
	public class RestorePasswordDTO
	{
		public string VerificationToken { get; set; }

        [MinLength(8, ErrorMessage = ErrorMessages.PasswordMinLength)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@!?])[A-Za-z\d@!?]{8,}$", ErrorMessage = ErrorMessages.PasswordValidation)]
        public string Password { get; set; }

		[Compare("Password", ErrorMessage = ErrorMessages.PasswordMatch)]
		public string RepeatPassword { get; set; }

	}
}
