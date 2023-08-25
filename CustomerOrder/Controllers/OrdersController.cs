using Microsoft.AspNetCore.Mvc;

namespace CustomerOrder.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
