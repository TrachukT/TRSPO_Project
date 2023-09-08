using System.ComponentModel.DataAnnotations;
using TFSport.Models.Entities;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Users
{
    public class ChangeUserRoleDTO
    {
        [Required(ErrorMessage = ErrorMessages.RoleIsRequired)]
        public UserRoles NewUserRole { get; set; }
    }
}
