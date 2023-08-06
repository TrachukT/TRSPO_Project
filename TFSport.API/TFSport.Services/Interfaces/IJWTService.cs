using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSport.Models;

namespace TFSport.Services.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateAccessTokenAsync(string email, IList<UserRoles> roles);
        Task<string> GenerateRefreshTokenAsync();
    }
}
