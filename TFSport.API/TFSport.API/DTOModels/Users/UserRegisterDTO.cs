using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TFSport.Models;

namespace TFSport.API.DTOModels.Users
{
	public class UserRegisterDTO
	{

		[RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = ErrorMessages.NamesValidation)]
		public string FirstName { get; set; }

		[RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = ErrorMessages.NamesValidation)]
		public string LastName { get; set; }

		[EmailAddress(ErrorMessage = ErrorMessages.EmailNotValid)]
		public string Email { get; set; }

		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-_+=])[A-Za-z\d-_+=]{8,}$", ErrorMessage = ErrorMessages.PasswordValidation)]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = ErrorMessages.PasswordMatch)]
		public string RepeatPassword { get; set; }

	}
}
