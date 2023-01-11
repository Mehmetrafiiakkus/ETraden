using ETrade.Dal.Abstract;
using ETrade.Data.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ETrade.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderDAL _IOrderDAL;

        public OrderController(IOrderDAL ıOrderDAL)
        {
            _IOrderDAL = ıOrderDAL;
        }

        public IActionResult Index()
        {
            return View(_IOrderDAL.GetAll());
        }

        public IActionResult Details(int id)
        {
            return View(_IOrderDAL.Get(id));
        }

        public IActionResult Edit(int id)
        {
            var order = _IOrderDAL.Get(id);
            var model = new OrderStateViewModel()
            {
                OrderID = order.Id,
                OrderNumber = order.OrderNumber,
                IsCompleted = false

            };
            if (order.OrderState == EnumOrderState.Completed)
            {
                model.IsCompleted = true;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(OrderStateViewModel model)
        {
            var order = _IOrderDAL.Get(model.OrderID);
            if (model.IsCompleted)
            {
                order.OrderState = EnumOrderState.Completed;
                _IOrderDAL.Update(order);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var order = _IOrderDAL.Get(id);
            if (order != null)
            {
                _IOrderDAL.Delete(order);
            }
            return RedirectToAction("Index");
        }
    }
}
