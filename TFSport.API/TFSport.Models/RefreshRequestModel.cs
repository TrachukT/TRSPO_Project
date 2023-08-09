using System.ComponentModel.DataAnnotations;

namespace TFSport.Models
{
    public class RefreshRequestModel
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
