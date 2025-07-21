using Core;
using Core.Domains;
using Data.DTOs;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationService.RolesService
{
    public interface IRolesService
    {
        Task<ResponseResult<RoleDTO>> CreateRoleAsync(RoleCreateDTO dto);
      //  Task<ResponseResult<bool>> DeleteRoleAsync(string id);
        Task<ResponseResult<List<PermissionDTO>>> GetPermissionsAsync();
      //  Task<ResponseResult<RoleDTO>> GetRoleByIdAsync(string id);
         Task<PagedListResult<RoleDTO>> GetRolesAsync(DataManagerRequest dm);
        Task<ResponseResult<RoleDTO>> UpdateRoleAsync(RoleUpdateDTO dto);
    }
}
