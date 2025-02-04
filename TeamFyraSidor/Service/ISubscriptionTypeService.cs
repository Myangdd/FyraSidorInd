using TeamFyraSidor.Data;

namespace TeamFyraSidor.Service
{
    public interface ISubscriptionTypeService
    {
        public void CreateSubscriptionType(SubscriptionType subscription);
        public void UpdateSubscriptionType(SubscriptionType subscription);
        public void DeleteSubscriptionType(int id);
        public SubscriptionType GetSubscriptionType(int id);
        public List<SubscriptionType> GetAllSubscriptionsTypes();
    }
}
