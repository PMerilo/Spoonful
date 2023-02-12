using Microsoft.AspNetCore.Mvc;
using Spoonful.Services;

namespace Spoonful.Controllers
{
    [Route("/Admin/mealkitsubscriptions")]
    public class MealKitSubscriptionController : Controller
    {
        private readonly MealKitSubscriptionLogService _mealKitSubscriptionLogService;

        public MealKitSubscriptionController(MealKitSubscriptionLogService mealKitSubscriptionLogService)
        {
            _mealKitSubscriptionLogService = mealKitSubscriptionLogService;
        }

        [HttpGet]
        public async Task<ActionResult<String>> GenerateJsonData()
        {
            try
            {
                var mealKitsSubscriptions = _mealKitSubscriptionLogService.GetAll().GroupBy(x => new { group = x.dateIssued })
                    .Select(group => new
                    {
                        dateIssued = group.Key.group,
                        count = group.Sum(x => x.noOfUsersSubscribed)
                    }).ToList();



                return Ok(mealKitsSubscriptions);

            }
            catch (Exception)
            {
                Console.WriteLine("Error in generating chart data");
                return "error";
            }
        }
    }
}
