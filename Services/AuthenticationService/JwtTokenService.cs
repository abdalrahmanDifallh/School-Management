using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationService
{

    public interface IJwtTokenGenerator
    {
        string GenerateToken(ClaimsIdentity claimsIdentity);
    }

    public class JwtTokenService(IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public string GenerateToken(ClaimsIdentity claimsIdentity)
        {
            // Generating the JWT token
            //  var keyBytes = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddMinutes(500),
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


    }

    public sealed record JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }

}
