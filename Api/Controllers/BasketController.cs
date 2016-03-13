using Api.Modules;
using Api.Siren;
using Api.ViewModels;
using Microsoft.AspNet.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class BasketController : Controller
    {
        [HttpGet]
        public Entity Get()
        {
            return new BasketModule(Request).Handle();
        }

        [HttpPost]
        public Entity Post(BasketAddModel model)
        {
            return new BasketModule(Request).Handle(model);
        }

        [HttpDelete("{id}")]
        public Entity Delete(string id)
        {
            return new BasketModule(Request).Handle(id);
        }
    }
}