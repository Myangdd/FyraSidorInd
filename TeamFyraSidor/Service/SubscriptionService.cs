using TeamFyraSidor.Data;
using Microsoft.EntityFrameworkCore;
using TeamFyraSidor.Models.ViewModels;
using TeamFyraSidor.Helpers;
using Microsoft.AspNetCore.Identity;

namespace TeamFyraSidor.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _db;
        private readonly IStripeService _stripeService;
        private readonly UserManager<User> _userManager;
        public SubscriptionService(ApplicationDbContext db, IStripeService stripeService, UserManager<User> userManager)
        {
            _db = db;
            _stripeService = stripeService;
            _userManager = userManager;
        }
        public void CreateSubscription(Subscription subscription)   
        {
            _db.Subscriptions.Add(subscription);
            _db.SaveChanges();
        }
        public Subscription ConfirmSubscription(int id)
        {
          return  _db.Subscriptions
                 .Include(s => s.SubscriptionType)
                 .Include(s => s.User)
                 .FirstOrDefault(s => s.Id == id)!;
        }
        public void UpdateSubscription(Subscription subscription)
        {
            _db.Subscriptions.Update(subscription);
            _db.SaveChanges();
        }
        public void DeleteSubscription(int id)
        {
            var sub = _db.Subscriptions.Find(id);
            
            if (sub != null)
            {
                _db.Subscriptions.Remove(sub);
                _db.SaveChanges();
            }
            
        }
        public Subscription GetSubscription(int id)
        {
            return _db.Subscriptions
                   .Include(s => s.User)
                   .FirstOrDefault(x => x.Id == id)!;

        }
        public List<Subscription> GetAllSubscriptions()
        {
            return _db.Subscriptions
                   .Include(s => s.User).Include(s=> s.SubscriptionType)
                   .OrderByDescending(s=>s.Created).ToList();
        }

        public Subscription GetSubscriptionUser(string id)
        {
            return _db.Subscriptions.Where(c => c.User.Id == id).FirstOrDefault()!;
        }


        public List<MonthlySubscriberDataVM> GetSubscriberData(int year)
        {
            var yearList = new List<MonthlySubscriberDataVM>();
            
            foreach(Month month in Enum.GetValues(typeof(Month)))
            {
                yearList.Add(new MonthlySubscriberDataVM()
                { 
                    Month = month,
                    NewSubscriberCount = _db.Subscriptions.
                    Where(s => s.Created.Month == (int)month && s.Created.Year == year).Count(),

                    AllSubscriberCount =_db.Subscriptions.
                    Where(s => s.Created <= new DateTime(year, (int)month, DateTime.DaysInMonth(year, (int)month), 23, 59, 59) 
                    && s.Expires > new DateTime(year, (int)month, DateTime.DaysInMonth(year, (int)month), 23, 59, 59)).Count()
                });
            }
            return yearList;
        }

        public async Task CheckIfExpiredAsync(User user)
        {
            var userSub = GetSubscriptionUser(user.Id);
            var stripeSub = _stripeService.GetSubscriptions(user).FirstOrDefault(c => c.StartDate == userSub.Created);
            if (stripeSub != null)
            {
                if (userSub.Expires < stripeSub.CurrentPeriodEnd && stripeSub.Status == "active")
                {
                    userSub.Expires = stripeSub.CurrentPeriodEnd;
                    _db.Subscriptions.Update(userSub);
                    await _db.SaveChangesAsync();
                }
                else if (stripeSub.Status != "active")
                {
                    _db.Subscriptions.Remove(userSub);
                    await _userManager.RemoveFromRoleAsync(user, "Subscriber");
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
