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
            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"items", "collection"},
                Entities = new[]
                {
                    new ItemModule(Request, "items", "A").BuildEntity(),
                    new ItemModule(Request, "items", "B").BuildEntity(),
                    new ItemModule(Request, "items", "C").BuildEntity(),
                    new ItemModule(Request, "items", "D").BuildEntity()
                },
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = href
                    }
                }
            };
        }
    }
}