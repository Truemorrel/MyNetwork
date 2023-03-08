using System.ComponentModel.DataAnnotations;

namespace MyNetwork.ViewModels.Account
{
    public class UserViewModel
	{
		[Required(ErrorMessage = "Поле Имя обязательно для заполнения")]
		[DataType(DataType.Text)]
		[Display(Name = "ФИО пользователя")]
		public string Username { get; set; } = null!;

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "Статус")]
		public string? Status { get; set; }

		[Required]
		[DataType(DataType.Text)]
		[Display(Name = "О себе")]
		public string? About { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;

        [Required]
		[DataType(DataType.ImageUrl)]
		[Display(Name = "Picture")]
		public string? ImageUrl { get; set; }

	}
}
