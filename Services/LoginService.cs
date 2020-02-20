using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Helpdesk.Models;
using Helpdesk.Helpers;

namespace Helpdesk.Services
{
    public interface ILoginService
    {
        void CreateToken(ref Identity identity);
    }

    public class LoginService : ILoginService
    {
        private readonly AppSettings _appSettings;
        public LoginService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        // https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api#app-settings-json
        public void CreateToken(ref Identity identity)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, identity.UserId.ToString()),
                    new Claim(ClaimTypes.Role, identity.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            identity.Token = tokenHandler.WriteToken(token);
        }
    }
}