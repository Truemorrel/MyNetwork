using MyNetwork.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace MyNetwork.ViewModels.Account
{
	public class UserViewModel
	{
		public User User { get; set; }

		public UserViewModel(User user)
		{
			User = user;
		}

	}
}
