using System;
using System.ComponentModel.DataAnnotations;

namespace OurChat.Models
{
    public class AuthViewModel
    {
        [Required]
        public string login { get; set; }
        [Required]
        public string password { get; set; }

        public string ReturnUrl { get; set; }
    }
}