using System.ComponentModel.DataAnnotations;
using TFSport.Models;

namespace TFSport.API.DTOModels.Users
{
    public class ChangeUserRoleDTO
    {
        [Required(ErrorMessage = ErrorMessages.RoleIsRequired)]
        public string NewUserRole { get; set; }
    }
}
