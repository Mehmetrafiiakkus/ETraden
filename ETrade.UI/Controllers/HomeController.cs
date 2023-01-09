using ETrade.Dal.Abstract;
using ETrade.Data.Models.Entites;
using ETrade.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ETrade.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductDAL _productDAL;
        private readonly ICategoryDAl _categoryDAl;

        public HomeController(ILogger<HomeController> logger,IProductDAL product,ICategoryDAl category)
        {
            _logger = logger;
            _productDAL = product;
            _categoryDAl= category;

        }

        public IActionResult Index()
        {
            var prodcut=_productDAL.GetAll(i=>i.IsHome && i.IsApproved); 
            return View(prodcut);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}