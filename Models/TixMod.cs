using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class TixMod
    {
        public int Id { get; set; }
        public string Feedbackstatus { get; set; } = "Pending";

        [Required]
        public string username { get; set; } = "test";

        [Required]
        public string email { get; set; } = "test";

        [Required]
        public string TitleFeedback { get; set; } = "test";

        [Required]
        public string MainFeedback { get; set; } = "test";

        [Required]
        public string Category { get; set; } = "test";

        [Required]
        public string datetime { get; set; } = DateTime.Now.ToString();

        [Required]
        public string notifypreference { get; set; } = "test";
        [Required]
        public string answer { get; set; } = "test";
    }
}
