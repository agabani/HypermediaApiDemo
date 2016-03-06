using System;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class RootModule : Module
    {
        public RootModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity()
        {
            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"root"},
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = href
                    },
                    new Link
                    {
                        Rel = new[] {"items"},
                        Href = new Uri(href, "items")
                    },
                    new Link
                    {
                        Rel = new[] {"status"},
                        Href = new Uri(href, "status")
                    }
                }
            };
        }
    }
}