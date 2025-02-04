using Microsoft.AspNetCore.Identity;
using Stripe;
using Stripe.Checkout;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Service
{
    public class StripeService : IStripeService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public StripeService(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration configuration,
            ApplicationDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public StripeList<Product> GetProducts()
        {
            var product = new ProductService();
            var productsOptions = new ProductListOptions
            {
            };
            StripeList<Product> products = product.List(productsOptions);
            return products;
        }

        public StripeList<Price> GetProductPrice(string productId)
        {
            var service = new PriceService();
            var serviceOptions = new PriceListOptions
            {
                Product = productId
            };
            StripeList<Price> plans = service.List(serviceOptions);
            return plans;
        }

        public long? GetPriceAmount(string productId)
        {
            var service = new PriceService();
            Price plan = service.Get(productId);
            long? planPrice = plan.UnitAmount;

            return planPrice;
        }

        public IEnumerable<Stripe.Subscription> GetSubscriptions(User user)
        {
            var subscriptionService = new Stripe.SubscriptionService();
            IEnumerable<Stripe.Subscription> response = subscriptionService.List(new SubscriptionListOptions
            {
                Customer = user.CustomerIdentifier
            });
            return response;
        }

        public void CreateCustomer(string stripeToken, User user)
        {
            var customers = new CustomerService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = user.Email,
                Source = stripeToken,
                Description = user.Email + " " + "[" + user.Id + "]",
                Name = user.FirstName + " " + user.LastName,
                Phone = user.PhoneNumber,
            });

            user.CustomerIdentifier = customer.Id;
            _db.SaveChanges();
        }

        public Stripe.Subscription CreateSubscription(string priceId, User user)
        {
            var subscriptionService = new Stripe.SubscriptionService();
            var subscriptionItems = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = priceId
                    }
                };
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = user.CustomerIdentifier,
                Items = subscriptionItems
            };

            var subscription = subscriptionService.Create(subscriptionOptions);
            return subscription;
        }

        public Session CreateSession(string priceId)
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = priceId,
                        Quantity = 1,
                    },
                },
                Mode = "subscription",
                SuccessUrl = "https://localhost:7006/User/Success.cshtml?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7006/User/Canceled.cshtml",
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        public async Task UpdateDatabaseIfComplete(IEnumerable<Stripe.Subscription> subs, User user)
        {
            int subTypeId = 0;
            if (_db.Subscriptions.Where(c => c.User == user).FirstOrDefault() == null || 
                (_db.Subscriptions.Where(c => c.User == user).Select(c => c.PaymentComplete).FirstOrDefault() == false &&
                 subs.Select(c => c.CancelAtPeriodEnd == false).FirstOrDefault()))
            {
                foreach (var sub in subs)
                {
                    if (sub.Items.Select(c => c.Price.Id).FirstOrDefault() == "price_1QgncnQjvxAv2prnVIkVV3aF" || 
                        sub.Items.Select(c => c.Price.Id).FirstOrDefault() == "price_1Qh659QjvxAv2prnPinaxKdG")
                    {
                        subTypeId = 2;
                    }
                    else if (sub.Items.Select(c => c.Price.Id).FirstOrDefault() == "price_1QgnbpQjvxAv2prnQndfLtsR" || 
                             sub.Items.Select(c => c.Price.Id).FirstOrDefault() == "price_1Qh67NQjvxAv2prn2ErsVuKP")
                    {
                        subTypeId = 1;
                    }
                    else if (sub.Items.Select(c => c.Price.Id).FirstOrDefault() == "price_1QgnbAQjvxAv2prnfnNzGNZ7" || 
                             sub.Items.Select(c => c.Price.Id).FirstOrDefault() == "price_1Qh68VQjvxAv2prn4o58gk6t")
                    {
                        subTypeId = 3;
                    }
                    var subscription = new Data.Subscription
                    {
                        SubscriptionType = _db.SubscriptionsTypes.Where(c => c.Id == subTypeId).FirstOrDefault(),
                        Price = _db.SubscriptionsTypes.Where(c => c.Id == subTypeId).Select(c => c.Price).FirstOrDefault(),
                        Created = sub.StartDate,
                        Expires = sub.CurrentPeriodEnd,
                        User = user,
                        PaymentComplete = true
                    };
                    if (!_db.Subscriptions.Contains(subscription))
                    {
                        _db.Subscriptions.Add(subscription);
                        await _db.SaveChangesAsync();
                        await _userManager.AddToRoleAsync(user, "Subscriber");
                    }

                }
            }
        }

        public void CancelSubscription(string subcriptionId)
        {
            var options = new SubscriptionUpdateOptions { CancelAtPeriodEnd = true };
            var service = new Stripe.SubscriptionService();
            service.Update(subcriptionId, options);
        }
    }
}
