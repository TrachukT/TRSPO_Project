using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TFSport.Models;
using TFSport.Services.Interfaces;

namespace TFSport.Filters
{
    public class RoleAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IUserService _userService;
        private readonly UserRoles[] _acceptedRoles;

        public RoleAuthorizationFilter(IUserService userService, params UserRoles[] acceptedRoles)
        {
            _userService = userService;
            _acceptedRoles = acceptedRoles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var emailClaim = user.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(emailClaim))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = await _userService.GetUserRolesByEmailAsync(emailClaim);

            if (!_acceptedRoles.Any(role => userRoles.Contains(role)))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
