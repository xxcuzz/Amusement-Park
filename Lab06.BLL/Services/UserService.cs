using Lab06.BLL.Services.Interfaces;
using Lab06.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab06.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<IQueryable<ApplicationUser>> GetAllUsersWithUserRoleAsync()
        {
            var allUsers = _userManager.Users;

            if (allUsers == null)
            {
                return null;
            }

            var usersInUserRole = new List<ApplicationUser>();
            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "User"))
                {
                    usersInUserRole.Add(user);
                }
            }

            return usersInUserRole.AsQueryable();
        }
    }
}
