using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyNetwork.Models;
using MyNetwork.Models.Users;
using MyNetwork.ViewModels.Account;
using System.Diagnostics;

namespace MyNetwork.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountManagerController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
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
                        return RedirectToAction("Index", "Home");
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

		[Authorize]
		[Route("MyPage")]
		[HttpGet]
		public IActionResult MyPage()
		{
			var user = User;

			var result = _userManager.GetUserAsync(user);

			return View("User", new UserViewModel(result.Result));
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
