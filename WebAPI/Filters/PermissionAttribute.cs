using Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using System.Security.Claims;

namespace ProjectManagement.WebAPI.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAttribute : TypeFilterAttribute
{
    // base() : takes the type of the filter that will be created by `TypeFilterAttribute`
    // Arguments : takes the Ctor args of PermissionFilter
    public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
    {
        Arguments = [permission];
    }

    public class PermissionFilter(string permission) : IAsyncAuthorizationFilter
    {
        private readonly string _permission = permission;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var permissionsList = _permission.Split(",").ToList();
            var principal = context.HttpContext.User;

            if (!principal.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetRequiredService<SchoolManagementContext>();

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userRole = await dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
            var userPermissions = await dbContext.RolePermissionMappings
                .Where(rpm => rpm.RoleId == userRole.RoleId)
                .Select(rpm => rpm.Permission.Key)
                .ToListAsync();



            if (!permissionsList.Any(userPermissions.Contains))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}