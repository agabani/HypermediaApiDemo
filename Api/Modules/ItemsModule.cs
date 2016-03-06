using System;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class ItemsModule : Module
    {
        public ItemsModule(HttpRequest request) : base(request)
        {
        }

        public override Entity BuildEntity()
        {
            var href = Request.GetDisplayUrl();

            return new Entity
            {
                Class = new[] {"items", "collection"},
                Links = new []
                {
                    new Link
                    {
                        Rel = new []{"self"},
                        Href = new Uri(href)
                    }
                }
            };
        }
    }
}