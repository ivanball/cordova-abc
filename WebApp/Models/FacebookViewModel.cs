using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Facebook;

namespace WebApp.Models
{
    public class FacebookViewModel
    {
        [Required]
        [Display(Name="Friend's Name")]
        public string Name { get; set; }

        [FacebookFieldModifier("type(large)")]
        public string ImageURL { get; set; }
    }
}