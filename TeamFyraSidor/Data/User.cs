using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamFyraSidor.Data
{
    public class User : IdentityUser
    {

        [Required]
        [StringLength(200)]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        //public string FullName { get { return $"{FirstName} {LastName}"; }}

        [Required]
        [DisplayName("Date of Birth")]
        public DateOnly DOB { get; set; } = new DateOnly(2000, 01, 01);

        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

        public bool Newsletter { get; set; } = false;

        public string? CustomerIdentifier { get; set; }

    }
}
