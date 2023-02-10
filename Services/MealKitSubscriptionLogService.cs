using Spoonful.Models;

namespace Spoonful.Services
{
    public class MealKitSubscriptionLogService
    {
        private readonly AuthDbContext _context;
        public MealKitSubscriptionLogService(AuthDbContext context)
        {
            _context = context;
        }
        public List<MealKitSubscriptionLog> GetAll()
        {
            return _context.MealKitSubscriptionLog.OrderBy(x => x.Id).ToList();
        }

        public MealKitSubscriptionLog? GetMealKitSubscriptionLogById(int id)
        {
            MealKitSubscriptionLog mealKitSubscriptionLog = _context.MealKitSubscriptionLog.FirstOrDefault(x => x.Id.Equals(id));
            return mealKitSubscriptionLog;
        }

        public void AddMealKitSubscriptionLog(MealKitSubscriptionLog mealKitSubscriptionLog)
        {
            _context.Add(mealKitSubscriptionLog);
            _context.SaveChanges();
        }

        public void DeleteMealKitSubscriptionLog(MealKitSubscriptionLog mealKitSubscriptionLog)
        {
            _context.Remove(mealKitSubscriptionLog);
            _context.SaveChanges();
        }
    }
}
