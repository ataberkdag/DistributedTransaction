using User.DAL.Models;

namespace User.BL.Services.Interfaces
{
    public interface IAccessTokenService
    {
        public Task<AppUserToken> GetToken(AppUser appUser, List<string> userRoles);
        public Task DeleteToken(AppUser appUser);
    }
}
