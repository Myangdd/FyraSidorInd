using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Text;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models.VM;

namespace TeamFyraSidor.Service
{
    public class UserRoleService:IUSerRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;


        public UserRoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;   
            _db = db;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            bool result = await _roleManager.RoleExistsAsync(roleName);
            return result;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return result;
        }

        public async Task<List<string>> ListRoleNameAsync()
        {
            var roles = _roleManager.Roles;
            var listRoleName = await roles.Select(r => r.Name).ToListAsync();
            return listRoleName!;
        }

        public async Task<IdentityRole> FindRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return role!;
        }

        public async Task<IdentityRole> FindRoleByNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role!;
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return result;
        }

        public async Task<IdentityResult> DeleteRoleAsync(IdentityRole role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return result;
        }

        public async Task<IdentityResult> RegisterEmployeeAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(User user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result;
        }

        public async Task<List<EditUserAndRoleVM>> ListEmployeeRolesAsync()
        {
            var users = await _db.Users.ToListAsync();
            var listEmployeeRole = new List<EditUserAndRoleVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Admin") || roles.Contains("Editor"))
                {
                    listEmployeeRole.Add(new EditUserAndRoleVM()
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email!,
                        PhoneNumber = user.PhoneNumber,
                        DOB = user.DOB,
                        ListRoleName = roles.ToList()
                    });
                }
            }
            return listEmployeeRole;
        }

        public async Task<List<EditUserAndRoleVM>> ListUserRolesAsync()
        {
            var users = await _db.Users.ToListAsync();
            var listEmployeeRole = new List<EditUserAndRoleVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                if (!roles.Contains("Admin") && !roles.Contains("Editor"))
                {
                    listEmployeeRole.Add(new EditUserAndRoleVM()
                    {
                        UserId = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email!,
                        PhoneNumber = user.PhoneNumber,
                        DOB = user.DOB,
                        ListRoleName = roles.ToList()
                    });
                }
            }
            return listEmployeeRole;
        }

        public async Task<EditUserAndRoleVM> FindUserRolesByEmailAsync(string email)
        {
            var model = new EditUserAndRoleVM();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    model.ErrorMsg = $"No user found with email: {email}";
                    return model;
                }
                
                var roles = await _userManager.GetRolesAsync(user);
                                
                model.UserId = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
                model.Email = user.Email!;
                model.DOB = user.DOB;
                if (roles == null)
                {
                    model.ListRoleName = null;
                }
                else 
                {
                    model.ListRoleName = roles.ToList();
                }                
                return model;
            }
            catch (Exception ex)
            {
                model.ErrorMsg = $"An error occurred: {ex.Message}";
            }
            return model;
        }

        public async Task<User> FindUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);            
            return user!;
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public Task<IList<string>> GetRoleNamesAsync(User user)
        {
            var listRoleName = _userManager.GetRolesAsync(user);
            return listRoleName;
        }

    }

        
}
