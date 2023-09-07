using System.ComponentModel.DataAnnotations;

namespace TFSport.Models.DTOModels
{
    public class RefreshRequestModel
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
