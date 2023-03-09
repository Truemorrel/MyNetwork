using System.ComponentModel.DataAnnotations;

namespace MyNetwork.ViewModels.Account
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "введите Имя")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "введите Фамилию")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "Отчество", Prompt = "введите Отчество")]
		public string MiddleName { get; set; } = null!;

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "email@example.com")]
        public string EmailReg { get; set; } = null!;

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [Display(Name = "Год", Prompt = "год рождения")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [Display(Name = "День", Prompt = "день")]
        public int? Date { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [Display(Name = "Месяц", Prompt = "месяц рождения")]
        public int? Month { get; set; }

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "введите пароль")]
        [StringLength(100, ErrorMessage = "Поле {0} должно иметь минимум {2} и максимум {1} символов.", MinimumLength = 5)]
        public string PasswordReg { get; set; } = null!;

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [Compare("PasswordReg", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "подтвердите пароль")]
        public string PasswordConfirm { get; set; } = null!;

        [Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Никнейм", Prompt = "Введите никнейм")]
        public string Login { get; set; } = null!; //  Login => EmailReg
    }
}
