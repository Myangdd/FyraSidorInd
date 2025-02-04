using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TeamFyraSidor.Models.VM
{
    public class CreateRoleVM
    {
        [Required]
        [DisplayName("Role Name")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 256 characters.")]
        public required string Name { get; set; }
    }
}
