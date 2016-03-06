using Api.Modules;
using Api.Siren;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("/")]
    public class RootController : Controller
    {
        [HttpGet]
        public Entity Get()
        {
            return new RootModule(Request).BuildEntity();
        }
    }
}