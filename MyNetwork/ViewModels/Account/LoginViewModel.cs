using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace MyNetwork.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "введите email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "введите пароль")]
        public string Password { get; set; } = null!;

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; } //= null!;
    }
}
