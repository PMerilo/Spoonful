using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
	public class ProblemThread
	{
        public int Id { get; set; }

        [Required]
        public string username { get; set; } = "test";

        [Required]
        public string TitleFeedback { get; set; } = "test";

        [Required]
        public string MainFeedback { get; set; } = "test";

        [Required]
        public string Category { get; set; } = "test";

        [Required]
        public string datetime { get; set; } = DateTime.Now.ToString();

        [Required]
        public string datetime2 { get; set; } = DateTime.Now.ToString();

        
        [Required]
        public string answer { get; set; } = "test";
        
        public string checker { get; set; } = "";
    }
}
