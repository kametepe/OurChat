using System;
using System.ComponentModel.DataAnnotations;

namespace OurChat.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "login")]
        public string login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de Passe")]
        [StringLength(12, ErrorMessage = "Mot de Passe Incorrect")]
        public string CPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau Mot de Passe")]
        [StringLength(12, ErrorMessage = "Nouveau Mot de Passe Incorrect")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation Mot de Passe")]
        [StringLength(12, ErrorMessage = "Confirmation Mot de Passe Incorrect")]
        public string ConfirmPassword { get; set; }

        public string returnUrl { get; set; }
    }
}