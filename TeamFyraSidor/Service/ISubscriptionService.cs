using TeamFyraSidor.Data;
using TeamFyraSidor.Models.ViewModels;

namespace TeamFyraSidor.Service
{
    public interface ISubscriptionService
    {
        public void CreateSubscription(Subscription subscription);
        public Subscription ConfirmSubscription(int id);
        public void UpdateSubscription(Subscription subscription);
        public void DeleteSubscription(int id);
        public Subscription GetSubscription(int id);
        public List<Subscription> GetAllSubscriptions();
        Subscription GetSubscriptionUser(string id);
        public List<MonthlySubscriberDataVM> GetSubscriberData(int year);
        Task CheckIfExpiredAsync(User user);


    }
}
