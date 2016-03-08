using System;
using Api.Extensions;
using Api.Siren;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class RootModule : Module
    {
        public RootModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity()
        {
            var baseAddress = Request.GetBaseAddress();

            return new Entity
            {
                Class = new[] {"root"},
                Entities = new Entity[] {},
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = Request.GetAbsoluteAddress()
                    },
                    new Link
                    {
                        Rel = new[] {"items"},
                        Href = new Uri(baseAddress, "/items")
                    },
                    new Link
                    {
                        Rel = new []{"basket"},
                        Href = new Uri(baseAddress, "/basket")
                    },
                    new Link
                    {
                        Rel = new[] {"status"},
                        Href = new Uri(baseAddress, "/status")
                    }
                }
            };
        }
    }
}