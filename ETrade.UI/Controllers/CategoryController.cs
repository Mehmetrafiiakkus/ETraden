using Microsoft.AspNetCore.Mvc;
using ETrade.Data.Models.Entites;
using ETrade.Dal.Abstract;
using ETrade.Dal.Concrete;

namespace ETrade.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryDAl categoryDAL;
        //CategoryDAL _categoryDAL = new CategoryDAL();
        public CategoryController(ICategoryDAl categoryDAL)
        {
          this.categoryDAL = categoryDAL;   
        }
        public IActionResult Index()
        {
            return View(categoryDAL.GetAll());
        }
        public IActionResult Create() =>View();
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryDAL.Add(category);
                return RedirectToAction("Index"); 
            }
            return View();
        }


    }
}
