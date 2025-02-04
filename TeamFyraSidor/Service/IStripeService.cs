using Stripe;
using Stripe.Checkout;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Service
{
    public interface IStripeService
    {
        public StripeList<Price> GetProductPrice(string productId);
        long? GetPriceAmount(string productId);
        void CreateCustomer(string stripeToken, User user);
        Stripe.Subscription CreateSubscription(string priceId, User user);
        Session CreateSession(string priceId);
        StripeList<Product> GetProducts();
        IEnumerable<Stripe.Subscription> GetSubscriptions(User user);
        Task UpdateDatabaseIfComplete(IEnumerable<Stripe.Subscription> subs, User user);
        void CancelSubscription(string subcriptionId);
    }
}
