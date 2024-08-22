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
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Token
{
    public class AppTokenHandler : IAppTokenHandler
    {
        private readonly IConfiguration _configuration;

        public AppTokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDTO CreateAccessToken(int accessTokenLifeTime, AppUser appUser)
        {
            TokenDTO token = new()
            {
                ExpirationDate = DateTime.UtcNow.AddMilliseconds(accessTokenLifeTime),
            };

            // Get the symmetric security key.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // Create the encrypted credentials.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // Set the token's configurations.
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.ExpirationDate,
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCredentials,
                claims: [new Claim(ClaimTypes.Name, appUser.UserName)]
            );

            // Create an instance of the token handler class.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            // Generate the refresh token.
            // token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
