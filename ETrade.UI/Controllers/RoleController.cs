using ETrade.Data.Models.Identity;
using ETrade.Data.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.UI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(RoleManager<AppRole> roleManager)
        {
            this._roleManager = roleManager;
        }

        public IActionResult Index()
        {

            if (_roleManager.Roles.ToList()==null)
            {
                return NotFound("Roller Bulunamadı..");
            }
            return View(_roleManager.Roles.Where(X=>X.Name!="Admin").ToList());//Admin rolleri dışındakileri getir 
        }
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            var _role = await _roleManager.FindByNameAsync(role.Name);
            if (_role == null)
            {
                var result = await _roleManager.CreateAsync(new AppRole(role.Name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }

            return View(role);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());
            role.Name = model.Name;
            role.NormalizedName = model.Name.ToUpper(); ;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
