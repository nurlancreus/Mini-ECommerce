using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Domain.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.GoogleLoginUser
{
    public class GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommandRequest, GoogleLoginUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly IAppTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;

        public GoogleLoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> usermanager, IConfiguration configuration, IAppTokenHandler tokenHandler)
        {
            _userManager = usermanager;
            _configuration = configuration;
            _tokenHandler = tokenHandler;
        }

        public async Task<GoogleLoginUserCommandResponse> Handle(GoogleLoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = [_configuration["ExternalLoginSettings:Google:ClientId"]]
            };

           var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var info = new UserLoginInfo(ExternalLoginProvider.Google.ToString(), payload.Subject, ExternalLoginProvider.Google.ToString());

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new()
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName
                    };

                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (!result)
            {
                throw new Exception("Invalid external authentication.");
            }

            await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

            TokenDTO token = _tokenHandler.CreateAccessToken(900, user);

            return new GoogleLoginUserCommandResponse()
            {
                Token = new()
                {
                    AccessToken = token.AccessToken,
                }
            };

        }
    }
}
