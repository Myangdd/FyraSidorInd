using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamFyraSidor.Models.VM
{
    public class EditRoleVM
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        [DisplayName("Role Name")]
        [StringLength(256, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 256 characters.")]
        public required string Name { get; set; }
    }
}
