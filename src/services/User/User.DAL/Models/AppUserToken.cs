using Microsoft.AspNetCore.Identity;

namespace User.DAL.Models
{
    public class AppUserToken : IdentityUserToken<string>
    {
        public DateTime ExpireDate { get; set; }
    }
}
