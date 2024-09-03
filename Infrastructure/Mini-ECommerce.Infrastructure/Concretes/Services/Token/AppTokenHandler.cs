using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Token
{
    public class AppTokenHandler : IAppTokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public AppTokenHandler(IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public TokenDTO CreateAccessToken(AppUser appUser)
        {
            double tokenLifeTime = Convert.ToDouble(_configuration["Token:AccessTokenLifeTimeInMinutes"]);

            TokenDTO token = new()
            {
                ExpirationDate = DateTime.UtcNow.AddMinutes(tokenLifeTime),
            };

            // Get the symmetric security key.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // Create the encrypted credentials.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, appUser.UserName!)
            ];

            // Add role claims for the user.
            var userRoles = _userManager.GetRolesAsync(appUser).Result;

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Set the token's configurations.
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.ExpirationDate,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: claims
            );

            // Create an instance of the token handler class.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            // Generate the refresh token.
            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[64];

            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);

            return Convert.ToBase64String(number);
        }

        public ClaimsPrincipal? GetPrincipalFromAccessToken(string? accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Token:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["Token:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"])),

                ValidateLifetime = false //should be false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
