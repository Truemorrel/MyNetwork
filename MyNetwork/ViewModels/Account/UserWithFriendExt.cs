using MyNetwork.Models.Users;

namespace MyNetwork.ViewModels.Account
{
    public class UserWithFriendExt : User
    {
        public bool IsFriendWithCurrent { get; set; }
    }
}
