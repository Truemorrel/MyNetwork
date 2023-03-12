using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyNetwork.Models.Users;
using MyNetwork.ViewModels.Account;

namespace MyNetwork.Controllers.Account
{
    // ChangePasswordAsync(user, old, new)	изменяет пароль пользователя
    // CreateAsync(user)                    создает нового пользователя
    // DeleteAsync(user)                    удаляет пользователя
    // FindByIdAsync(id)                    ищет пользователя по id
    // FindByEmailAsync(email)              ищет пользователя по email
    // FindByNameAsync(name)                ищет пользователя по нику
    // UpdateAsync(user)                    обновляет пользователя
    // Users                                возвращает всех пользователей
    // AddToRoleAsync(user, role)           добавляет для пользователя user роль role
    // GetRolesAsync(user)                  возвращает список ролей, к которым принадлежит пользователь user
    // IsInRoleAsync(user, name)            возвращает true, если пользователь user принадлежит роли name
    // RemoveFromRoleAsync(user, name)      удаляет роль name у пользователя user
    public class RegisterController : Controller
    {
        private IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RegisterController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View("Home/Register");
        }

        [Route("RegisterPart2")]
        [HttpGet]
        public IActionResult RegisterPart2(RegisterViewModel model)
        {
            return View("RegisterPart2", model);
        }
        
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("RegisterPart2", model);
        }
    }
}
