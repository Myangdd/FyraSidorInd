using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using TeamFyraSidor.Data;

namespace TeamFyraSidor.Service
{
    public class SubscriptionTypeService : ISubscriptionTypeService
    {
        private readonly ApplicationDbContext _db;

        public SubscriptionTypeService(ApplicationDbContext db)
        {
            _db = db;
        }
        public void CreateSubscriptionType(SubscriptionType subscription)
        {
            _db.SubscriptionsTypes.Add(subscription);
            _db.SaveChanges();
        }
        public void UpdateSubscriptionType(SubscriptionType subscription)
        {
            _db.SubscriptionsTypes.Update(subscription);
            _db.SaveChanges();
        }
        public void DeleteSubscriptionType(int id)
        {
            var type = _db.SubscriptionsTypes.Find(id);
            if (type != null)
            {
                _db.SubscriptionsTypes.Remove(type);
                _db.SaveChanges();
            }
        }
        public SubscriptionType GetSubscriptionType(int id)
        {
           return _db.SubscriptionsTypes.FirstOrDefault(t => t.Id == id)!;
        }
        public List<SubscriptionType> GetAllSubscriptionsTypes()
        {
            return _db.SubscriptionsTypes
                .OrderBy(t => t.Id)
                .ToList();
        }
    }
}
