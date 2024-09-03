using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Mini_ECommerce.API.Configurations
{
    public static class AuthConfiguration
    {
        public static AuthenticationBuilder ConfigureAuth(this WebApplicationBuilder builder)
        {
            return (builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Specifies which origins/sites can use the generated token value. -> www.somesite.com
            ValidateIssuer = true, // Specifies who issued the generated token value. -> www.myapi.com
            ValidateIssuerSigningKey = true, // Validation of the security key, confirming that the generated token value belongs to our application.
            ValidateLifetime = true, // Validation that checks the expiration time of the generated token value.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

            NameClaimType = ClaimTypes.Name, // The value corresponding to the Name claim in the JWT can be obtained from the User.Identity.Name property.
            RoleClaimType = ClaimTypes.Role
        };
    }));
        }
    }
}
