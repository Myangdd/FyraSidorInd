using System.ComponentModel;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Models
{
    public class CreateSubscriptionViewModel
    {
        public int Id { get; set; }
        public Subscription Subscription { get; set; } = new Subscription();
        public List<SubscriptionType> SubscriptionTypeList { get; set; } = new List<SubscriptionType>();
        public int SubscriptionTypeId { get; set; } = 2;
        public SubscriptionType SubscriptionType { get; set; } = new SubscriptionType();
        public decimal Price { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; } = DateTime.Now.AddMonths(1);
        [DisplayName("Payment Complete")]
        public bool PaymentComplete { get; set; }
    }
}
