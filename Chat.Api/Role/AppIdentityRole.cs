using Microsoft.AspNetCore.Identity;

namespace Chat.Api.Role
{
    public class AppIdentityRole : RoleManager<AppIdentityRole>
    {
        public AppIdentityRole(IRoleStore<AppIdentityRole> store, IEnumerable<IRoleValidator<AppIdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<AppIdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        
    }
}
