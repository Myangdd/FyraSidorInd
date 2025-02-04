using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using TeamFyraSidor.Data;
using System.Linq;
using Stripe.Checkout;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.Areas.Identity.Pages.Account.Manage
{
    public class ManageSubscriptionModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IStripeService _stripeService;

        public ManageSubscriptionModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration configuration,
            IStripeService stripeService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _stripeService = stripeService;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        [TempData]
        public required string StatusMessage { get; set; }

        [BindProperty]
        public required InputModel Input { get; set; }

        public required SelectList SubscriptionProducts { get; set; }
        public required SelectList SubscriptionPrice { get; set; }

        public required List<Stripe.Subscription> Subscriptions { get; set; }

        public required string StripeKey { get; set; }
        public required string UserEmail { get; set; }
        public required string StripeCustomerId { get; set; }

        public long? PriceAmount { get; set; }

        public class InputModel
        {
            public required string ProductId { get; set; }

            public required string PriceId { get; set; }
        }

        public JsonResult OnGetProducts(string productId)
        {
            // Check for null or empty string.
            if (string.IsNullOrEmpty(productId))
            {
                return new JsonResult(null);
            }
            // Get Stripe product prices
            StripeList<Price> price = _stripeService.GetProductPrice(productId);
            SubscriptionPrice = new SelectList(price, "Id", "Nickname");
            return new JsonResult(SubscriptionPrice);
        }

        public JsonResult OnGetPrice(string priceId)
        {
            // Check for null or empty string.
            if (string.IsNullOrEmpty(priceId))
            {
                return new JsonResult(null);
            }
            // Get the price amount for the Stripe product price
            PriceAmount = _stripeService.GetPriceAmount(priceId);

            return new JsonResult(PriceAmount);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            // Check for null value
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Check if CustomerIdentidier exists.
            // CustomerIdentifier is the Stripe CustomerID
            if (user!.CustomerIdentifier != null)
            {
                StripeCustomerId = user.CustomerIdentifier;
            }

            // Check if Null or Empty
            var nullCustomerId = string.IsNullOrEmpty(StripeCustomerId);

            UserEmail = user.Email!;

            // Get List of products
            var products = _stripeService.GetProducts();
            SubscriptionProducts = new SelectList(products, "Id", "Name");

            // If false (nullCustomerId is not Null/Empty)
            if (!nullCustomerId)
            {
                // Get Stripe Subscriptions from User
                var response = _stripeService.GetSubscriptions(user);
                Subscriptions = response.ToList();

                // Check if Subscription is active, if true update website database.
                if (Subscriptions.Where(c => c.Status == "active").Select(c => c.Status).FirstOrDefault() == "active")
                {
                    await _stripeService.UpdateDatabaseIfComplete(response, user);
                }
            }

            StripeKey = _configuration["Stripe:PublishableKey"]!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] string stripeToken)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserEmail = user!.Email!;

            if (user.CustomerIdentifier != null)
            {
                StripeCustomerId = user.CustomerIdentifier;
            }

            var nullCustomerId = string.IsNullOrEmpty(StripeCustomerId);

            // Get PriceId and Price Amount
            var priceId = Input.PriceId;
            var priceAmount = _stripeService.GetPriceAmount(priceId);


            var customerService = new CustomerService();
            Customer customerLookup = new Customer();


            if (!nullCustomerId)
            {
                // Get Customer From Stripe
                customerLookup = customerService.Get(StripeCustomerId);
            }


            // Create new customer in stripe if doesn't exist
            if (nullCustomerId || customerLookup.Deleted == true)
            {
                _stripeService.CreateCustomer(stripeToken, user);
            }

            // Set Stripe Customer Id
            StripeCustomerId = user.CustomerIdentifier!;


            var subscription = _stripeService.CreateSubscription(priceId, user);

            var session = _stripeService.CreateSession(priceId);
            await _signInManager.RefreshSignInAsync(user);

            // If successfully created subscription
            // Update website database.
            if (subscription.Status == "active")
            {
                var subscriptions = _stripeService.GetSubscriptions(user);
                await _stripeService.UpdateDatabaseIfComplete(subscriptions, user);
            }

            StatusMessage = "Your payment: " + session.Status;

            return RedirectToPage();
        }

        public IActionResult OnGetCancelSubscription(string subscriptionId)
        {
            _stripeService.CancelSubscription(subscriptionId);

            return RedirectToPage();
        }

    }
}
