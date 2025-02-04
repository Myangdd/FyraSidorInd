using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Models.VM
{
    public class CreateUserRoleVM
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        [DisplayName("Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DisplayName("First Name")]
        [StringLength(200, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [StringLength(200, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)] // Specifies that this is a date field
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateOnly DOB { get; set; }

        [Required]
        [DisplayName("Role Name")]
        public required string RoleName { get; set; }

    }
}
