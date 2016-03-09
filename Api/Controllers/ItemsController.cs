using Api.Modules;
using Api.Siren;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class ItemsController : Controller
    {
        [HttpGet]
        public Entity Get()
        {
            return new ItemsModule(Request).Handle();
        }

        [HttpGet("{id}")]
        public Entity Get(string id)
        {
            return new ItemModule(Request, id).Handle();
        }
    }
}