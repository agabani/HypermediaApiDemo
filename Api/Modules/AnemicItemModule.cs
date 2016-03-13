using Api.Siren;
using Api.ValueObjects;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class AnemicItemModule : ItemModule
    {
        public AnemicItemModule(HttpRequest request, string id) : base(request, id)
        {
        }

        protected override Action[] BuildActions(Item item)
        {
            return new Action[]{};
        }

        protected override Link[] BuildLinks()
        {
            var linkFactory = LinkFactory;

            return new[]
            {
                linkFactory.Create("item", Id, true)
            };
        }
    }
}