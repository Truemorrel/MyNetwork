using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyNetwork.Data;
using MyNetwork.Data.Repository;
using MyNetwork.Data.UoW;
using MyNetwork.Extentions;
using MyNetwork.Models;
using MyNetwork.Models.Users;
using MyNetwork.ViewModels.Account;
using System.Diagnostics;

namespace MyNetwork.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        private async Task<SearchViewModel> CreateSearch(string search)
        {
            var currentuser = User;
            var result = await _userManager.GetUserAsync(currentuser);
            var withfriend = await GetAllFriend();
            var data = new List<UserWithFriendExt>();
            var list = _userManager.Users.AsEnumerable().ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
            }
            list.ForEach(x =>
            {
                var t = _mapper.Map<UserWithFriendExt>(x);
                t.IsFriendWithCurrent = withfriend.Where(y => y.Id == x.Id || x.Id == result.Id).Count() != 0;
                data.Add(t);
            });

            var model = new SearchViewModel()
            {
                UserList = data
            };
            return model;
        }

        private async Task<List<User?>?> GetAllFriend(User user)
        {
            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return await repository.GetFriendsByUser(user);
        }

        //private async Task<List<User>> GetAllFriend()
        //{
        //    var user = User;

        //    var result = await _userManager.GetUserAsync(user);

        //    var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

        //    return repository.GetFriendsByUser(result);
        //}

        private async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return await repository!.GetFriendsByUser(result!);
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpGet]
		[ValidateAntiForgeryToken]
		public IActionResult Login()
        {
            return View("Home/Login");
        }

        [AllowAnonymous]
        [Route("Login")]
		[HttpGet]
        public IActionResult Login(string returnUrl = null!)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl }) ;
        }

        [AllowAnonymous]
        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var userByEmail = await _userManager.FindByEmailAsync(user.Email);
				var result = await _signInManager.PasswordSignInAsync(userByEmail, model.Password, isPersistent: model.RememberMe, false);

				if (result.Succeeded)
                {
						return RedirectToAction("MyPage", "AccountManager");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    //return View("Views/Home/Index.cshtml", new MainViewModel {LoginView = model, RegisterView = new RegisterViewModel()});
                }
            }
            return RedirectToAction("Index", "Home"); // return View("Views/Home/Index.cshtml");// return View("Views/Home/Index.cshtml", model)

        }

        [AllowAnonymous]
        [Route("UserList")]
        [HttpGet]
        public async Task<IActionResult> UserList(string search)
        {
            var model = await CreateSearch(search);
            return View("UserList", model);
        }

		//[Route("UserList")]
		//[HttpPost]
		//public IActionResult UserList()
		//{
		//	var model = new SearchViewModel
		//	{
		//		UserList = _userManager.Users.ToList()
		//	};
		//	return View("UserList", model);
		//}

		//[Route("UserList")]
		//[HttpPost]
		//public IActionResult UserList(string search)
		//{
		//    var model = new SearchViewModel
		//    {
		//        UserList = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().Contains(search)).ToList()
		//    };
		//    return View("UserList", model);
		//}

		[AllowAnonymous]
		[Route("AddFriend")]
        [HttpPost]
        public async Task<IActionResult> AddFriend(string id)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);

            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            repository.AddFriend(result, friend);

            return RedirectToAction("MyPage", "AccountManager");
        }

        [Route("DeleteFriend")]
        [HttpPost]
        public async Task<IActionResult> DeleteFriend(string id)
        {
            var currentuser = User;

            var result = await _userManager.GetUserAsync(currentuser);

            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            repository.DeleteFriend(result, friend);

            return RedirectToAction("MyPage", "AccountManager");

        }

        [AllowAnonymous]
        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            var editmodel = _mapper.Map<UserEditViewModel>(result.Result);

            return View("Edit", editmodel);
        }

        [AllowAnonymous]
		[Route("Update")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UserEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.UserId);

				user.Convert(model);

				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					return RedirectToAction("MyPage", "AccountManager");
				}
				else
				{
					return RedirectToAction("Edit", "AccountManager");
				}
			}
			else
			{
				ModelState.AddModelError("", "Некорректные данные");
				return View("Edit", model);
			}
		}

        //[Authorize]
        //[Route("MyPage")]
        //[HttpGet]
        //public IActionResult MyPage()
        //{
        //	var user = User;

        //	var result = _userManager.GetUserAsync(user);

        //	return View("User", new UserViewModel(result.Result));
        //}

        //[Authorize(Policy = "EmployeeOnly")]

        [AllowAnonymous]
        [Route("MyPage")]
        [HttpGet]
        public async Task<IActionResult> MyPage()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var model = new UserViewModel(result);

            model.Friends = await GetAllFriend(model.User);

            return View("User", model);
        }

		private async Task<ChatViewModel> GenerateChat(string id)
		{
			var currentuser = User;

			var result = await _userManager.GetUserAsync(currentuser);
			var friend = await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

			var mess = await repository.GetMessages(result, friend);

			var model = new ChatViewModel()
			{
				You = result,
				ToWhom = friend,
				History = mess.OrderBy(x => x.Id).ToList(),
			};

			return model;
		}

		[AllowAnonymous]
		[Route("Chat")]
		[HttpGet]
		public async Task<IActionResult> Chat()
		{

			var id = Request.Query["id"];

			var model = await GenerateChat(id!);
			return View("Chat", model);
		}

		[AllowAnonymous]
		[Route("Chat")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Chat(string  id)
		{
			var currentuser = User;

			var result = await _userManager.GetUserAsync(currentuser);
			var friend = await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

			var mess = await repository.GetMessages(result, friend);

			var model = new ChatViewModel()
			{
				You = result,
				ToWhom = friend,
				History = mess.OrderBy(x => x.Id).ToList(),
			};
			return View("Chat", model) ;
			
		}

        [AllowAnonymous]
        [Route("Generate")]
        [HttpGet]
        public async Task<IActionResult> Generate()
        {

            var usergen = new GenetateUsers();
            var userlist = usergen.Populate(35);

            foreach (var user in userlist)
            {
                var result = await _userManager.CreateAsync(user, "123456");

                if (!result.Succeeded)
                    continue;
            }

            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
		[Route("NewMessage")]
		[HttpPost]
		public async Task<IActionResult> NewMessage(string id, ChatViewModel chat)
		{
			var currentuser = User;

			var result = await _userManager.GetUserAsync(currentuser);
			var friend = await _userManager.FindByIdAsync(id);

			var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

			if (!string.IsNullOrEmpty(chat.NewMessage.Text))
			{
				var item = new Message()
				{
					Sender = result,
					Recipient = friend,
					Text = chat.NewMessage.Text,
				};
				await repository.Create(item);
			}

			var mess = await repository.GetMessages(result, friend);

			var model = new ChatViewModel()
			{
				You = result,
				ToWhom = friend,
				History = mess.OrderBy(x => x.Id).ToList(),
			};
			return View("Chat", model); // View("Chat", model)
		}


		[AllowAnonymous]
        [Route("Logout")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
