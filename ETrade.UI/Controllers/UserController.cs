using ETrade.Data.Models.Identity;
using ETrade.Data.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using System.Data;

namespace ETrade.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var users = new List<AppUser>();
            foreach (var item in admins)
            {
                users = _userManager.Users.Where(x => x.Id != item.Id).ToList();//admin olmayan kişileri görüntülemek için yani ikimizde aynı admin yetkisindeysek birbirlerine dokunmasın 
            }

            return View(users);
        }
        
        public async Task<IActionResult> RoleAssing(int id)
        {
            var users = await _userManager.FindByIdAsync(id.ToString());
            var roles = _roleManager.Roles.Where(x => x.Name != "Admin").ToList();
            var userRole = await _userManager.GetRolesAsync(users);
            var RoleAssings = new List<RoleAssginViewModel>();
            roles.ForEach(role => RoleAssings.Add(new RoleAssginViewModel()
            {
                HasAssign = userRole.Contains(role.Name),
                Id = role.Id,
                Name = role.Name

            }));
            ViewBag.UserName = users.Name;
            return View(RoleAssings);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssing(List<RoleAssginViewModel> models, int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            foreach (var role in models)
            {
                if (role.HasAssign)
                {
                  await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("Index");
        }
        public  async Task<IActionResult> Delete(int id)
        {
            var user=await _userManager.FindByIdAsync(id.ToString());
            var sonuc=await _userManager.DeleteAsync(user);
            if (sonuc.Succeeded) 
            {
                return RedirectToAction("Index");
            
            }
            return NotFound("Silme İşlemi Başarısız...");
        }
    }
}
