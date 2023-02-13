using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Invoice
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [Display(Name = "Menu Preferences")]
        public string? MenuPreference { get; set; }

        [Required]
        [Display(Name = "Number Of Recipes Per Week")]
        public int? noOfRecipesPerWeek { get; set; }

        [Required]
        [Display(Name = "Number Of People Per Week")]
        public int? noOfPeoplePerWeek { get; set; }

        [Required]
        [Display(Name = "Number Of Servings Per Person")]
        public int? noOfServingsPerPerson { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public string? OrderDate { get; set; }

        [Required]
        [Display(Name = "Order Time")]
        public string? OrderTime { get; set; }

        [Required]
        [Display(Name = "Total Cost")]
        public double Cost { get; set; }

        //[Required]
        //[Display(Name = "PDF Path")]
        //public string invoicePDFPath { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        public string? userId { get; set; }
        [Required]
        public int? mealkitId { get; set; }

        [Required]
        public string? orderDetailsId { get; set; }

        [Display(Name = "Discount Code Used")]
        public string? DiscountCodeUsed { get; set; } = null;

        [Display(Name = "Date Of Payment")]
        public string? DateOfPayment { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");


    }

}
