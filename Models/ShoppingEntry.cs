using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spoonful.Models
{
    public class ShoppingEntry
    {
        [Key]
        [Display(Name = "No.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Display(Name="Purchased?")]
        public bool isBought { get; set; } = false;
    }
}
