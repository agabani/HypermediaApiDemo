using Api.Modules;
using Api.Siren;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("items")]
    public class ItemsController : Controller
    {
        [HttpGet]
        public Entity Get()
        {
            return new ItemsModule(Request).BuildEntity();
        }
    }
}