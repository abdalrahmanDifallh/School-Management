//using Core.Domains;
//using Data.DTOs;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using SchoolManagement.Data;
//using SchoolManagement.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.Design;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading.Tasks;

//namespace Services.AuthenticationService
//{
//    public class AuthService : IAuthService
//    {
//        private readonly SchoolManagementContext _context;
//        private readonly IConfiguration _configuration;
//        public AuthService(SchoolManagementContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration;
//        }
//        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
//            if (user is null)
//            {
//                return null;
//            }

//            if (new PasswordHasher<ApplicationUser>().VerifyHashedPassword(user, user.PasswordHash, request.PasswordHash)
//                == PasswordVerificationResult.Failed)
//            {
//                return null;
//            }

            

//            return await CreateTokenResponse(user);
//        }

//        private async Task<TokenResponseDto> CreateTokenResponse(ApplicationUser user)
//        {
//            return new TokenResponseDto
//            {
//                AccessToken = CreateToken(user),
//                RefreshToken = await GeneratAndSaveRefreshTokwnAsync(user),
//            };
//        }

//        public async Task<ApplicationUser?> RegisterStudentAsync(AppUserDTO request)
//        {
//            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
//            {
//                return null;
//            }
//            var user = new ApplicationUser();
//            var hashedPassword = new PasswordHasher<ApplicationUser>().HashPassword(user, request.PasswordHash);

//            user.FullName = request.FullName;
//            user.Email = request.Email;
//            user.PhoneNumber = request.PhoneNumber;
//            user.Address = request.Address;
//            user.CreatedOnUtc = DateTime.UtcNow;
//            user.Gender = request.Gender;
//            user.DateOfBirth = request.DateOfBirth;
//            user.Image = request.Image;
//            user.RoleId = "3";
//            user.PasswordHash = hashedPassword;
          

//            var subjects = await _context.Subjects.ToListAsync();


//            foreach (var subject in subjects)
//            {
//                user.Grades.Add(new Grades
//                {
//                    ApplicationUserId = user.Id,
//                    SubjectId = subject.Id,
//                    ClassroomId =   request.ClassRoomId,
//                    StudentGrad = 0,
//                    AssignedDate = DateTime.UtcNow
//                });
//            }


//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return user;
//        }

//        public async Task<ApplicationUser?> RegisterTeacherAsync(AppUserDTO request)
//        {
//            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
//            {
//                return null;
//            }
//            var user = new ApplicationUser();
//            var hashedPassword = new PasswordHasher<ApplicationUser>().HashPassword(user, request.PasswordHash);

//            user.FullName = request.FullName;
//            user.Email = request.Email;
//            user.PhoneNumber = request.PhoneNumber;
//            user.Address = request.Address;
//            user.Gender = request.Gender;
//            user.DateOfBirth = request.DateOfBirth;
//            user.Image = request.Image;
//            user.CreatedOnUtc = DateTime.UtcNow;
//            user.RoleId = "2";
//            user.PasswordHash = hashedPassword;

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return user;

//        }

//        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
//        {
//            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken); ;
//            if (user is null)
//                return null;

//            return await CreateTokenResponse(user);


//        }

//        private async Task<ApplicationUser?> ValidateRefreshTokenAsync(Guid userId, string refrshToken)
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user is null || user.RefreshToken != refrshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
//            {
//                return null; 
//              }
//            return user;
//        }

//        private string GenerateRefreshTokent()
//        {
//            var randomNumber = new byte[32];
//            using var rng = RandomNumberGenerator.Create();
//             rng.GetBytes(randomNumber);
//            return Convert.ToBase64String(randomNumber);
//        }

//        private async Task<string> GeneratAndSaveRefreshTokwnAsync(ApplicationUser user)
//        {
//            var refreshToken = GenerateRefreshTokent();
//            user.RefreshToken = refreshToken;
//            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
//            await _context.SaveChangesAsync();
//            return refreshToken;



//        }

//        private string CreateToken(ApplicationUser user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.Email),
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                //new Claim(ClaimTypes.Role, user.RoleId)

//            };

//            var keyString = _configuration.GetValue<string>("AppSettings:Token")
//                ?? throw new InvalidOperationException("JWT token key is not configured.");
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

//            var tokenDescriptor = new JwtSecurityToken(
//                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
//                audience: _configuration.GetValue<string>("AppSettings:Audience"), // Fixed typo
//                claims: claims,
//                expires: DateTime.UtcNow.AddDays(1),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
//        }

        
//    }
//}
