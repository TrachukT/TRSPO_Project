using System.ComponentModel.DataAnnotations;
using TFSport.Models;

namespace TFSport.API.DTOModels.Users
{
    public class ChangeUserRoleDTO
    {
        [Required(ErrorMessage = ErrorMessages.EmailIsRequired)]
        [EmailAddress(ErrorMessage = ErrorMessages.EmailNotValid)]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = ErrorMessages.RoleIsRequired)]
        public string NewUserRole { get; set; }
    }
}
