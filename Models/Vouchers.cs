using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class Vouchers
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Voucher Code")]
        public string? voucherCode { get; set; }

        [Required]
        [Display(Name = "Discount Amount")]
        public int? discountAmount { get; set; }

        [Required]
        public int? Quantity { get; set; }
        [Required]
        [Display(Name = "Vouchers Used")]
        public int? Used { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime expiryDate { get; set; }

        public string? ImageURL { get; set; }
    }   
}

// ? is nullable and [required] if to make sure sql is not nullable but c# is nullable
