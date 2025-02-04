using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TeamFyraSidor.Data;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.Areas.Identity.Pages.Account.Manage
{
    public class ManageNewsletterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ISubscriptionService _subscriptionService;

        public ManageNewsletterModel(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            ISubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _subscriptionService = subscriptionService;
        }

        public required string Username { get; set; }

        [TempData]
        public required string StatusMessage { get; set; }

        public int SubscriptionTypeId { get; set; }

        [BindProperty]
        public required InputModel Input { get; set; }

        public class InputModel
        {
            public bool Newsletter { get; set; }

        }

        private void Load(User user)
        {
            var newsletter = user.Newsletter;

            Input = new InputModel
            {
                Newsletter = newsletter,

            };
        }

        public async Task <IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            // Check if user is null
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Check if user has a subscription
            if (_subscriptionService.GetSubscriptionUser(user!.Id) != null)
            {
                SubscriptionTypeId = _subscriptionService.GetSubscriptionUser(user.Id).SubscriptionTypeId;
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            // Check if user is null
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Check for vaild ModelState
            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            // Check if Input has changed.
            if (Input.Newsletter != user.Newsletter)
            {
                // If changed update user.Newsletter and save changes.
                user.Newsletter = Input.Newsletter;
                var saveChanges = await _userManager.UpdateAsync(user);

                // Check if for some reason the newsletter could not save.
                if (!saveChanges.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to update Newsletter setting.";
                    return RedirectToPage();
                }
            }

            // Refresh the UserData with a RefreshSignIn
            await _signInManager.RefreshSignInAsync(user);
            if (user.Newsletter == true) // Check if user subscribed.
            { StatusMessage = "You successfully subscribed to our Newsletter!"; }
            else // Else user unsubscribed.
            { StatusMessage = "You successfully unsubscribed from our Newsletter."; }

            return RedirectToPage();
        }
    }
}
