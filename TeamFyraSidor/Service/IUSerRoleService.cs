using Microsoft.AspNetCore.Identity;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models.VM;

namespace TeamFyraSidor.Service
{
    public interface IUSerRoleService
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<List<string>> ListRoleNameAsync();
        Task<IdentityRole> FindRoleByIdAsync(string roleId);
        Task<IdentityRole> FindRoleByNameAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
        Task<IdentityResult> RegisterEmployeeAsync(User employee, string password);
        Task<IdentityResult> AddToRoleAsync(User employee, string roleName);
        Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName);
        Task<List<EditUserAndRoleVM>> ListEmployeeRolesAsync();
        Task<List<EditUserAndRoleVM>> ListUserRolesAsync();
        Task<EditUserAndRoleVM> FindUserRolesByEmailAsync(string email);
        Task<User> FindUserByIdAsync(string id);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IList<string>> GetRoleNamesAsync(User user);

    }
}
