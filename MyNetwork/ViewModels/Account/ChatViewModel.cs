using MyNetwork.Models.Users;

namespace MyNetwork.ViewModels.Account
{
	public class ChatViewModel
	{
		public User You { get; set; } = default!;

		public User ToWhom { get; set; } = default!;

		public List<Message>? History { get; set; }

		public MessageViewModel NewMessage { get; set; } = default!;

		public ChatViewModel()
		{
			NewMessage = new MessageViewModel();
		}

	}
}
