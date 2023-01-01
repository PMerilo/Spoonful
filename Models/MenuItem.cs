using Microsoft.Build.Framework;

namespace Spoonful.Models
{
    public class MenuItem
    {
        
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string Tags { get; set; }

        public string Image { get; set; }

        public int FoodTypeId { get; set; }

        public int CategoryId { get; set; }
    }
}
