using Chat.Api.Exceptions;
using Chat.Api.Helper;
using Microsoft.AspNetCore.Identity;

namespace Chat.Api.Role
{
    public class RoleInitializer
    {

        private readonly UserManager<IdentityRole> _manager;

        public RoleInitializer(UserManager<IdentityRole> manager)
        {
            _manager = manager;
        }

        public async Task Initializer(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Constants.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.Admin));
            }

            if (!await roleManager.RoleExistsAsync(Constants.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.User));
            }
        }
        
        public async Task<string> AssignRole(Guid userId, string role)
        {
            var user = await _manager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new UserNotFound();

            var result = _manager.AddToRoleAsync(user, role);
            if (result.IsCompletedSuccessfully)
            {
                return "Role Assigned";
            }
            return "";
        }
    }
}
