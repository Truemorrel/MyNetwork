namespace MyNetwork.ViewModels.Account
{
	public class MainViewModel
	{
		public RegisterViewModel RegisterView { get; set; }
		public LoginViewModel LoginView { get; set; }
//        public UserViewModel UserView { get; set; }

        public MainViewModel()
		{
			RegisterView = new RegisterViewModel();
			LoginView = new LoginViewModel();
//			UserView = new UserViewModel(new Models.Users.User());
		}
	}
}
