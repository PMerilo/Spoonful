using Microsoft.AspNetCore.Mvc;
using Spoonful.Models;
using Spoonful.Services;

namespace Spoonful.Controllers
{
    [Route("/Admin/charts")]
    public class MenuPreferencesController : Controller
    {
        private readonly MealKitService _mealKitService;

        public MenuPreferencesController(MealKitService mealKitService)
        {
            _mealKitService = mealKitService;
        }


        [HttpGet]
        public async Task<ActionResult<String>> GenerateJsonData()
        {
            try 
            {
                var mealKits = _mealKitService.GetAll().GroupBy(x => new { group = x.MenuPreference })
                    .Select(group => new
                    {
                        menuPreference = group.Key.group,
                        count = group.Count()
                    }).OrderByDescending(o => o.count).ToList();

                         

                return Ok(mealKits);
            
            }catch(Exception)
            {
                Console.WriteLine("Error in generating chart data");
                return "error";
            }

        }
        
    }
}
