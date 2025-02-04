using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace TeamFyraSidor.Models.VM
{
    public class EditUserAndRoleVM
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Phone Number")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        [DisplayName("First Name")]
        [StringLength(200, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Last Name")]
        [StringLength(200, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)] // Specifies that this is a date field
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly? DOB { get; set; }

        [DisplayName("Role Name")]
        public List<string>? ListRoleName { get; set; }

        [TempData]
        public string? ErrorMsg { get; set; }
    }
}
