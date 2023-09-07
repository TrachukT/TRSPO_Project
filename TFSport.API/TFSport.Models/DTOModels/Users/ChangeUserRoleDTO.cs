using System.ComponentModel.DataAnnotations;
using TFSport.Models.Exceptions;

namespace TFSport.Models.DTOModels.Users
{
    public class ChangeUserRoleDTO
    {
        [Required(ErrorMessage = ErrorMessages.RoleIsRequired)]
        public string NewUserRole { get; set; }
    }
}
