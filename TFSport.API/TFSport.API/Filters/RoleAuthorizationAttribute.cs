using Microsoft.AspNetCore.Mvc;
using TFSport.Models;

namespace TFSport.API.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RoleAuthorizationAttribute : TypeFilterAttribute
    {
        public RoleAuthorizationAttribute(params UserRoles[] acceptedRoles)
            : base(typeof(RoleAuthorizationFilter))
        {
            Arguments = new object[] { acceptedRoles };
        }
    }
}
