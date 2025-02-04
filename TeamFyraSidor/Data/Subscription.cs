using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamFyraSidor.Data
{
    public class Subscription
    {
        public int Id { get; set; }
        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; } = new SubscriptionType();
        public decimal Price { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; } = DateTime.Now.AddMonths(1);
        public User User { get; set; } = new User();
        [DisplayName("Payment Complete")]
        public bool PaymentComplete { get; set; }
    }
}
