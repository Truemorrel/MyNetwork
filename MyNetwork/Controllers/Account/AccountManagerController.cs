using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
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

            var list = _userManager.Users.AsEnumerable().Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
            var withfriend = await GetAllFriend();

            var data = new List<UserWithFriendExt>();
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

        private async Task<List<User>> GetAllFriend(User user)
        {
            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(user);
        }

        private async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            return repository.GetFriendsByUser(result);
        }

        //private async Task<List<User>> GetAllFriend()
        //{
        //    var user = User;

        //    var result = await _userManager.GetUserAsync(user);

        //    var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

        //    return repository.GetFriendsByUser(result);
        //}

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View("Home/Login");
        }

		[Route("Login")]
		[HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl }) ;
        }

		[Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                var userByEmail = await _userManager.FindByEmailAsync(user.Email!);
				var result = await _signInManager.PasswordSignInAsync(userByEmail!, model.Password!, isPersistent: model.RememberMe, false);

				if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
						return RedirectToAction("MyPage", "AccountManager");
					}
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    return View("Views/Home/Index.cshtml",
                        new MainViewModel {
                            LoginView = model,
                            RegisterView = new RegisterViewModel()
                        }
                        );
                }
            }
            return View("Views/Home/Index.cshtml");// return View("Views/Home/Index.cshtml", model)

		}

        [Route("UserList")]
        [HttpGet]
        public async Task<IActionResult> UserList(string search)
        {
            var model = await CreateSearch(search);
            return View("UserList", model);
        }

        //      [Route("UserList")]
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

        [Authorize]
		[Route("Update")]
		[HttpPost]
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


        [Authorize]
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
