using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeamFyraSidor.Data
{
    public class SubscriptionType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Subscription Name")]
        public string TypeName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }
    }
}
