using Core.Domains;
using Data.DTOs;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<ClaimsIdentity> GetUserClaimsIdentity(ApplicationUser user);
        Task<ClaimsIdentity> GetUserClaimsIdentity(string userId);
        Task<ResponseResult<AppUserDTO1>> Login(LoginDto requestModel);

    }
}
