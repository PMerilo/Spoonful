using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
    public class VoucherEmails
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string? Subject { get; set; }

        [Required]
        public string? HtmlContent { get; set; }

        [Required]
        [Display(Name = "Voucher Id")]
        public int vouchersId { get; set; }
        public Vouchers Vouchers { get; set; }

        [Required]
        public string? sendTo { get; set; }

    }
}

// ? is nullable and [required] if to make sure sql is not nullable but c# is nullable
