using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using TeamFyraSidor.Data;
using TeamFyraSidor.Models;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.Controllers
{
    public class UserController : Controller
    {
        private readonly ISubscriptionService _subSer;
        private readonly ISubscriptionTypeService _subTypeSer;
        private readonly UserManager<User> _userManager;
      
        private readonly ApplicationDbContext _db;

        public UserController(ISubscriptionService subSer, ISubscriptionTypeService subTypeSer,
                              UserManager<User> userManager, ApplicationDbContext db)
        {
            _subTypeSer = subTypeSer;
            _subSer = subSer;
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateSubscription() // View for the form.
        {
            var list = new CreateSubscriptionViewModel
            {
                SubscriptionTypeList = _subTypeSer.GetAllSubscriptionsTypes()
            };
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateSubscription(Subscription subscription)
        {
            if (_db.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User)) != null)
            {
                subscription.User = _db.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User))!;
            }

            if (!User.Identity!.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "You must be logged in to create a subscription.";
                return RedirectToAction("Login", "User");
            }


            var subscriptionType = _subTypeSer.GetSubscriptionType(subscription.SubscriptionTypeId);
            if (subscriptionType != null)
            {
                subscription.Price = subscriptionType.Price;
            }
            _subSer.CreateSubscription(subscription);
            return RedirectToAction("CreateSubscriptionConfirmation", new { id = subscription.Id });
        }
        public IActionResult CreateSubscriptionConfirmation(Subscription subscription)
        {
            var subConfirmation = _subSer.ConfirmSubscription(subscription.Id);
            return View(subConfirmation);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateSubscription(int id)
        {
            var subscription = _subSer.GetSubscription(id);
            if (subscription != null)
            {
                var viewModel = new CreateSubscriptionViewModel
                {
                    Id = subscription.Id,
                    SubscriptionTypeId = subscription.SubscriptionTypeId,
                    Price = subscription.Price,
                    Created = subscription.Created,
                    Expires = subscription.Expires,
                    PaymentComplete = subscription.PaymentComplete,
                    SubscriptionType = _subTypeSer.GetSubscriptionType(subscription.Id),
                    SubscriptionTypeList = _subTypeSer.GetAllSubscriptionsTypes()
                };
                return View(viewModel);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult UpdateSubscription(CreateSubscriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.SubscriptionTypeList = _subTypeSer.GetAllSubscriptionsTypes();
                return View(model);
            }
            var subscriptionToUpdate = _subSer.GetSubscription(model.Id);
            if (subscriptionToUpdate == null)
            {
                return NotFound();
            }

            subscriptionToUpdate.SubscriptionTypeId = model.SubscriptionTypeId;
            subscriptionToUpdate.Price = model.Price;
            subscriptionToUpdate.Created = model.Created;
            subscriptionToUpdate.Expires = model.Expires;
            subscriptionToUpdate.PaymentComplete = model.PaymentComplete;

            _subSer.UpdateSubscription(subscriptionToUpdate);

            return RedirectToAction("GetSubscription","User", model.Id);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteSubscription(int id)
        {
            var sub = _subSer.GetSubscription(id);
            if (sub == null) 
                return NotFound();

            return View(sub);
           
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteSubscriptionConfirmed(int id)
        {
            _subSer.DeleteSubscription(id);
             return RedirectToAction("GetAllSubscriptions");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetSubscription(int id)
        {
            var sub = _subSer.GetSubscription(id);
            if (sub != null)
            {
                return View(sub);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllSubscriptions()
        {
            var subs = _subSer.GetAllSubscriptions();
            if (subs == null)
            {
                return NotFound();
            }
            return View(subs);
        }

    }
}
