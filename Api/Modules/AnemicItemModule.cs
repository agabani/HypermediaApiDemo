using System;
using Api.Siren;
using Api.ValueObjects;
using Microsoft.AspNet.Http;
using Action = Api.Siren.Action;

namespace Api.Modules
{
    public class AnemicItemModule : ItemModule
    {
        public AnemicItemModule(HttpRequest request, string id) : base(request, id)
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
                    Href = new Uri(BaseAddress, $"/items/{Id}")
                }
            };
        }
    }
}