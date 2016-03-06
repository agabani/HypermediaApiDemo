using Api.Modules;
using Api.Siren;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class BasketController : Controller
    {
        [HttpPost]
        public Entity Post(BasketAddModel model)
        {
            return new BasketModule(Request).BuildEntity();
        }
    }

    public class BasketAddModel
    {
        public string Id { get; set; }
    }
}