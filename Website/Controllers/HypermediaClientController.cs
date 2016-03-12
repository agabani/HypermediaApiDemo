using Microsoft.AspNet.Mvc;

namespace Website.Controllers
{
    public class HypermediaClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}