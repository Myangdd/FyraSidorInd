using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamFyraSidor.Models.VM
{
    public class EditUserRoleVM
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Email {  get; set; }

        [DisplayName("Role Name")]
        public List<string>? ListRoleName { get; set; }
        [DisplayName("Role Name")]
        [Required]
        public string RoleToDo { get; set; } = string.Empty;

    }
}
