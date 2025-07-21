using Core;
using Core.Domains;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using Services.SyncGridOperations;
using Syncfusion.EJ2.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationService.RolesService
{
    public class RolesService : BaseService<RolesService>, IRolesService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RolesService(SchoolManagementContext dbContext,
                           ILoggerService<RolesService> logger, 
                           IHttpContextAccessor httpAccessor ,
                           RoleManager<ApplicationRole> roleManager) : base(dbContext, logger, httpAccessor)
        {
            _roleManager = roleManager;
        }


      //  get all roles
        public async Task<PagedListResult<RoleDTO>> GetRolesAsync(DataManagerRequest dm)
        {
            try
            {
                var query = _dbContext.Roles.Select(x => new RoleDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                });

                var result = await SyncGridOperations<RoleDTO>.PagingAndFilterAsync(query, dm);
                return result;
            }
            catch (Exception)
            {
                return new PagedListResult<RoleDTO>();

            }

        }


        // get role by id
        //public async Task<ResponseResult<RoleDTO>> GetRoleByIdAsync(string id)
        //{
        //    try
        //    {
        //        var role = await _dbContext.Roles.AsNoTracking().AsSplitQuery()
        //            .Include(x => x.RolePermissionMappings)
        //            .ThenInclude(x => x.Permission)
        //            .FirstOrDefaultAsync(x => x.Id == id);

        //        if (role == null)
        //            return Error<RoleDTO>("Role not found");

        //        var roleDto = new RoleDTO
        //        {
        //            Id = role.Id,
        //            Name = role.Name,
        //            Permissions = role.RolePermissionMappings.Select(x => new PermissionDTO
        //            {
        //                Id = x.PermissionId,
        //                Key = x.Permission.Key,
        //                DisplayName = x.Permission.DisplayName
        //            }).ToList()
        //        };

        //        return Success(roleDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error<RoleDTO>(ex);
        //    }
        //}


        // create role
        public async Task<ResponseResult<RoleDTO>> CreateRoleAsync(RoleCreateDTO dto)
        {
            try
            {
                var role = new ApplicationRole
                {
                    Name = dto.Name,
                };

                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description).ToList();
                    return Error<RoleDTO>(errors);
                }




                if (dto.Permissions != null && dto.Permissions.Any())
                {
                    var rolePermissionMappings = dto.Permissions.Select(x => new RolePermissionMapping
                    {
                        RoleId = role.Id,
                        PermissionId = x
                    }).ToList();

                    await _dbContext.RolePermissionMappings.AddRangeAsync(rolePermissionMappings);
                    await _dbContext.SaveChangesAsync();
                }

                return Success(new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = dto.Permissions.Select(x => new PermissionDTO
                    {
                        Id = x,
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return Error<RoleDTO>(ex);
            }
        }


        // update role
        public async Task<ResponseResult<RoleDTO>> UpdateRoleAsync(RoleUpdateDTO dto)
        {
            try
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (role == null)
                    return Error<RoleDTO>("Role not found");

                role.Name = dto.Name;

                _dbContext.Roles.Update(role);
                await _dbContext.SaveChangesAsync();

                var rolePermissionMappings = await _dbContext.RolePermissionMappings
                    .Where(x => x.RoleId == role.Id)
                    .ToListAsync();

                _dbContext.RolePermissionMappings.RemoveRange(rolePermissionMappings);
                await _dbContext.SaveChangesAsync();

                if (dto.Permissions != null && dto.Permissions.Any())
                {
                    rolePermissionMappings = dto.Permissions.Select(x => new RolePermissionMapping
                    {
                        RoleId = role.Id,
                        PermissionId = x,
                    }).ToList();

                    await _dbContext.RolePermissionMappings.AddRangeAsync(rolePermissionMappings);
                    await _dbContext.SaveChangesAsync();
                }

                return Success(new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    Permissions = _dbContext.RolePermissionMappings
                        .Where(x => x.RoleId == role.Id)
                        .Select(x => new PermissionDTO
                        {
                            Id = x.PermissionId,
                        }).ToList()

                });
            }
            catch (Exception ex)
            {
                return Error<RoleDTO>(ex);
            }
        }


        // delete role
        //public async Task<ResponseResult<bool>> DeleteRoleAsync(string id)
        //{
        //    try
        //    {
        //        var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
        //        if (role == null)
        //            return Error<bool>("Role not found");

        //        role.IsDeleted = true;
        //        await _dbContext.SaveChangesAsync();

        //        return Success(true);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Error<bool>(ex);
        //    }
        //}


        // get all permissions
        public async Task<ResponseResult<List<PermissionDTO>>> GetPermissionsAsync()
        {
            try
            {
                var permissions = await _dbContext.Permissions.Select(x => new PermissionDTO
                {
                    Id = x.Id,
                    Key = x.Key,
                    DisplayName = x.DisplayName
                }).ToListAsync();

                return Success(permissions);
            }
            catch (Exception ex)
            {
                return Error<List<PermissionDTO>>(ex);
            }
        }



    }
}
