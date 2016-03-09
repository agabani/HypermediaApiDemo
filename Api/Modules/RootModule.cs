using System;
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
            return new[]
            {
                new Link
                {
                    Rel = new[] {"self"},
                    Href = BaseAddress
                },
                new Link
                {
                    Rel = new[] {"items"},
                    Href = new Uri(BaseAddress, "/items")
                },
                new Link
                {
                    Rel = new[] {"basket"},
                    Href = new Uri(BaseAddress, "/basket")
                },
                new Link
                {
                    Rel = new[] {"status"},
                    Href = new Uri(BaseAddress, "/status")
                }
            };
        }
    }
}