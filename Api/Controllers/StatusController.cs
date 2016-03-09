using Api.Modules;
using Api.Siren;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class StatusController : Controller
    {
        [HttpGet]
        public Entity Get()
        {
            return new StatusModule(Request).Handle();
        }
    }
}