using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TFSport.Models;

namespace TFSport.API.Filters
{
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        private readonly UserRoles[] _acceptedRoles;

        public RoleAuthorizationFilter(params UserRoles[] acceptedRoles)
        {
            _acceptedRoles = acceptedRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => Enum.Parse<UserRoles>(c.Value))
                .ToList();

            if (!_acceptedRoles.Any(role => userRoles.Contains(role)))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
