using Api.Siren;
using Api.ValueObjects;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class AnemicItemModule : ItemModule
    {
        public AnemicItemModule(HttpRequest request, string path, string id) : base(request, path, id)
        {
        }

        protected override Action[] BuildActions(Item item)
        {
            return null;
        }

        protected override Link[] BuildLinks()
        {
            return new[]
            {
                new Link
                {
                    Rel = new[] {"self"},
                    Href = Href
                }
            };
        }
    }
}