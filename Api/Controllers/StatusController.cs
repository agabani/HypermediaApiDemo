using Api.Modules;
using Api.Siren;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("status")]
    public class StatusController : Controller
    {
        [HttpGet]
        public Entity Get()
        {
            return new StatusModule(Request).BuildEntity();
        }
    }
}