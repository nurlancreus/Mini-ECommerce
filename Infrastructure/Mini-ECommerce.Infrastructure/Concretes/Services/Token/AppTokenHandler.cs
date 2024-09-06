using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.Options.Token;
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
        private readonly Mini_ECommerce.Application.Options.Token.TokenOptions _tokenOptions;
        private readonly UserManager<AppUser> _userManager;

        public AppTokenHandler(IOptions<Mini_ECommerce.Application.Options.Token.TokenOptions> tokenSettings, UserManager<AppUser> userManager)
        {
            _tokenOptions = tokenSettings.Value;
            _userManager = userManager;
        }

        public TokenDTO CreateAccessToken(AppUser appUser)
        {
            TokenDTO token = new()
            {
                ExpirationDate = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenLifeTimeInMinutes),
            };

            // Get the symmetric security key.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            // Create the encrypted credentials.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, appUser.UserName!)
            };

            // Add role claims for the user.
            var userRoles = _userManager.GetRolesAsync(appUser).Result;

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Set the token's configurations.
            JwtSecurityToken securityToken = new(
                audience: _tokenOptions.Audience,
                issuer: _tokenOptions.Issuer,
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
                ValidAudience = _tokenOptions.Audience,
                ValidateIssuer = true,
                ValidIssuer = _tokenOptions.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)),

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
