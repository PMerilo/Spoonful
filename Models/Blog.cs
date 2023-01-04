using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
	public class Blog
	{
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string? title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? description { get; set; }

        [Required]
        [Display(Name = "Date Issued")]
        public string? Date { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string? imageURL { get; set; }

    }
}
