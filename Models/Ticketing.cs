using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spoonful.Models
{
    public class Ticketing
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }


        public string Feedbackstatus { get; set; } = "Pending";

        [Required]
        public string email { get; set; }

        [Required]
        public string TitleFeedback { get; set; }

        [Required]
        public string MainFeedback { get; set; }

        [Required]
        public string Category { get; set; } = "test";

        [Required]
        public string datetime { get; set; } = DateTime.Now.ToString();
    }
}
