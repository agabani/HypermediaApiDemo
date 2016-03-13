using Api.Builders;
using Api.Siren;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class RootModule : Module
    {
        public RootModule(HttpRequest request) : base(request)
        {
        }

        public Entity Handle()
        {
            return new EntityBuilder()
                .WithClass("root")
                .WithLink(() => LinkFactory.Create("root", true))
                .WithLink(() => LinkFactory.Create("items", false))
                .WithLink(() => LinkFactory.Create("basket", false))
                .WithLink(() => LinkFactory.Create("status", false))
                .Build();
        }
    }
}