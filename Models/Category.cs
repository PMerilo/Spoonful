using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Category
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Category Type")]
        public string? name { get; set; }
        
        
        
    }
}
