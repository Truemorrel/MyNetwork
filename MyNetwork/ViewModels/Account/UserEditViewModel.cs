namespace MyNetwork.ViewModels.Account
{
	public class UserEditViewModel
	{
		public string? Image { get; set; }
		public string LastName { get; set; } = null!;
		public string MiddleName { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string? Email { get; set; }
		public DateTime BirthDate { get; set; }
		public string? UserName { get; set; }
		public string? Status { get; set; }
		public string? About { get; set; }
	}
}