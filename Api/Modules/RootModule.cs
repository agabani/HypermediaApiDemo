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

        public Entity Handle()
        {
            return new Entity
            {
                Class = BuildClass(),
                Entities = BuildEntities(),
                Links = BuildLinks()
            };
        }

        private static string[] BuildClass()
        {
            return new[] {"root"};
        }

        private static Entity[] BuildEntities()
        {
            return new Entity[] {};
        }

        private Link[] BuildLinks()
        {
            var baseAddress = Request.GetBaseAddress();

            return new[]
            {
                new Link
                {
                    Rel = new[] {"self"},
                    Href = baseAddress
                },
                new Link
                {
                    Rel = new[] {"items"},
                    Href = new Uri(baseAddress, "/items")
                },
                new Link
                {
                    Rel = new[] {"basket"},
                    Href = new Uri(baseAddress, "/basket")
                },
                new Link
                {
                    Rel = new[] {"status"},
                    Href = new Uri(baseAddress, "/status")
                }
            };
        }
    }
}