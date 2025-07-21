using Core.Domains;
using Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using Services._Base;
using Services.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationService
{
    public class AuthenticationService(UserManager<ApplicationUser> userManager,
       IJwtTokenGenerator jwtTokenGenerator,
       SchoolManagementContext dbContext,
       ILoggerService<AuthenticationService> logger,
       IHttpContextAccessor httpAccessor)
       : BaseService<AuthenticationService>(dbContext, logger, httpAccessor), IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

        public async Task<ResponseResult<AppUserDTO1>> Login(LoginDto requestModel)
        {
          //  var user = await _userManager.FindByNameAsync(requestModel.Email);
           var user = await _userManager.FindByEmailAsync(requestModel.Email);
            if (user is null)
                return Error<AppUserDTO1>("EmailNotFound");

            bool isValid = await _userManager.CheckPasswordAsync(user, requestModel.PasswordHash);

            if (!isValid)
                return Error<AppUserDTO1>("WrongUsernameOrPassword");

            var claimsIdentity = await GetUserClaimsIdentity(user);
            var token = _jwtTokenGenerator.GenerateToken(claimsIdentity);
            var userDto = new AppUserDTO1
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                RoleName = claimsIdentity.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                FullName = user.FullName,
                Token = token,
           
            };


            return Success(userDto);
        }


        public async Task<ClaimsIdentity> GetUserClaimsIdentity(ApplicationUser user)
        {
            // Default claims setup
            var claimList = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()), // HttpContext.User.Identity.IsAuthenticated
                new(ClaimTypes.Email,user.Email!),
                new(ClaimTypes.Name, user.UserName),

                new("FullName", user.FullName ?? ""),
            };

            var roleId = await _dbContext.UserRoles.FirstOrDefaultAsync(a => a.UserId == user.Id);
            var role = await _dbContext.Roles.FirstOrDefaultAsync(a => a.Id == roleId.RoleId);

            var permissions = await _dbContext.RolePermissionMappings
                .Where(r => role.Id == r.RoleId)
                .Select(p => p.Permission)
                .ToListAsync();

            // Roles and permissions claims
            claimList.Add(new Claim(ClaimTypes.Role, role.Name));
            claimList.AddRange(permissions.Select(role => new Claim("scope", role.Key)));

            return new ClaimsIdentity(claimList);
        }

        public async Task<ClaimsIdentity> GetUserClaimsIdentity(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new ClaimsIdentity();

            return await GetUserClaimsIdentity(user);
        }
    }
}
