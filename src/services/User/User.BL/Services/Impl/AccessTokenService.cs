using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.BL.Models;
using User.BL.Services.Interfaces;
using User.DAL.Models;

namespace User.BL.Services.Impl
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IDbContextHandler _contextHandler;
        private readonly TokenOptions _tokenOptions;

        public AccessTokenService(IDbContextHandler contextHandler,
            IOptions<TokenOptions> tokenOptions)
        {
            _contextHandler = contextHandler;
            _tokenOptions = tokenOptions.Value;
        }

        public async Task<DAL.Models.AppUserToken> GetToken(AppUser appUser, List<string> userRoles)
        {
            var appUserToken = _contextHandler.GetDbSet<AppUserToken>().FirstOrDefault(x => x.UserId == appUser.Id);
            if (appUserToken is not null && appUserToken.ExpireDate > DateTime.Now)
                return appUserToken;

            var tokenInfo = GenerateToken(appUser, userRoles);

            if (appUserToken is not null)
            {
                appUserToken.ExpireDate = tokenInfo.ExpireDate;
                appUserToken.Value = tokenInfo.Token;

                _contextHandler.GetDbSet<AppUserToken>().Update(appUserToken);
            }
            else
            {
                appUserToken = new DAL.Models.AppUserToken()
                {
                    UserId = appUser.Id,
                    LoginProvider = "UserAPI",
                    Name = appUser.UserName,
                    Value = tokenInfo.Token,
                    ExpireDate = tokenInfo.ExpireDate
                };

                _contextHandler.GetDbSet<AppUserToken>().Add(appUserToken);
            }

            await _contextHandler.SaveChangesAsync();

            return appUserToken;
        }

        private TokenInfo GenerateToken(AppUser user, List<string> userRoles)
        {
            DateTime expireDate = DateTime.Now.AddMinutes(5);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenOptions.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _tokenOptions.Audience,
                Issuer = _tokenOptions.Issuer,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),

                //ExpireDate
                Expires = expireDate,

                //Şifreleme türünü belirtiyoruz: HmacSha256Signature
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            userRoles?.ForEach(role => {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            });

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenInfo { 
                Token = tokenString,
                ExpireDate = expireDate
            };
        }

        public async Task DeleteToken(AppUser appUser)
        {
            var appUserToken = _contextHandler.GetDbSet<AppUserToken>().FirstOrDefault(x => x.UserId == appUser.Id);
            if (appUserToken == null)
                return;

            _contextHandler.GetDbSet<AppUserToken>().Remove(appUserToken);

            await _contextHandler.SaveChangesAsync();
        }
    }
}
