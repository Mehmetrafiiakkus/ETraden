using ETrade.Data.Models.Identity;
using ETrade.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace ETrade.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser > _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
      
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;

        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (User.Identity.Name != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }

            return View(login);
        }
        public IActionResult Register()
        {
            if (User.Identity.Name != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            var user = new AppUser()
            {
                Name = register.Name,
                SurName = register.Surname,
                UserName = register.Username,
                Email = register.Email
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            await _userManager.AddToRoleAsync(user, "Users");
            if (result.Succeeded)
                return RedirectToAction("Login");

            return View(register);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
