using Lab06.DAL.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab06.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);

        Task UpdateAsync(ApplicationUser user);

        Task<IQueryable<ApplicationUser>> GetAllUsersWithUserRoleAsync();

        Task<ApplicationUser> GetUserById(string userId);
    }
}
