﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Spoonful.Models
{
    public class Feedbackform
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; }


        public string Feedbackstatus { get; set; } = "New Message*";

        [Required]
        public string Customername { get; set; } = "Anonymous";

        [Required]
        public string TitleFeedback { get; set; }

        [Required]
        public string MainFeedback { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string datetime { get; set; } = DateTime.Now.ToString();

    }

}
