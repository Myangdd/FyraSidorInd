using TeamFyraSidor.Helpers;

namespace TeamFyraSidor.Models.ViewModels
{
    public class MonthlySubscriberDataVM
    {
        public Month Month {  get; set; }

        // new subscriber signed this month
        public int NewSubscriberCount { get; set; }

        // subscriber with active subscription this month
        public int AllSubscriberCount { get; set; }
    }
}
