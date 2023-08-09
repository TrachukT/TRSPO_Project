using System.ComponentModel.DataAnnotations;
using TFSport.Models;

namespace TFSport.API.DTOModels.Users
{
	public class RestorePasswordDTO
	{
		public string VerificationToken { get; set; }
		
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-_+=])[A-Za-z\d-_+=]{8,}$", ErrorMessage = ErrorMessages.PasswordValidation)]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = ErrorMessages.PasswordMatch)]
		public string RepeatPassword { get; set; }

	}
}
