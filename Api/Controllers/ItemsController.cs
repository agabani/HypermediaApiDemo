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

        [HttpGet("{id}")]
        public Entity Get(string id)
        {
            return new ItemModule(Request, id).BuildEntity();
        }
    }
}