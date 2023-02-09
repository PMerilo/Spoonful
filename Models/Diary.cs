using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spoonful.Models
{
    public class Diary
    {
        [Key]
        [Display(Name = "No.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Purchase Date")]
        public DateTime Purchase { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Expiry Date")]
        public DateTime Expiry { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);


    }
}
