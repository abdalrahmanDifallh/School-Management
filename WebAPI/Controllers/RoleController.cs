using Core;
using Core.Domains;
using Core.Enums;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.WebAPI.Filters;
using Services.AuthenticationService.RolesService;
using Syncfusion.EJ2.Base;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {

        private readonly IRolesService _rolesService;

        public RoleController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpPost("CreateRole")]
       // [Permission(nameof(Permissions.Roles_Create))]
        public async Task<ResponseResult<RoleDTO>> CreateRole([FromBody] RoleCreateDTO role)
        {
            var result = await _rolesService.CreateRoleAsync(role);
            return result;
        }

        [HttpGet("GetPermissions")]
      //  [Permission(nameof(Permissions.Roles_Get))]
        public async Task<ResponseResult<List<PermissionDTO>>> GetPermissions()
        {
            var result = await _rolesService.GetPermissionsAsync();
            return result;
        }

        [HttpPost("GetRoles")]
       
        public async Task<PagedListResult<RoleDTO>> GetRoles([FromBody] DataManagerRequest dm)
        {
            var result = await _rolesService.GetRolesAsync(dm);
            return result;
        }

        [HttpPost("UpdateRole")]
     //   [Permission(nameof(Permissions.Roles_Update))]
        public async Task<ResponseResult<RoleDTO>> UpdateRole([FromBody] RoleUpdateDTO role)
        {
            var result = await _rolesService.UpdateRoleAsync(role);
            return result;
        }

    }
}
