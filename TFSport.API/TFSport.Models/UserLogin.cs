using System.ComponentModel.DataAnnotations;

namespace TFSport.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@!?])[A-Za-z\d@!?]{8,}$", ErrorMessage = "Password must contain at least one letter, one digit, and one non-alphanumeric character.")]
        public string Password { get; set; }
    }
}
