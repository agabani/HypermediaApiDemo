using System;
using System.Collections.Generic;
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
                    CreateItem(href, "A", 50d),
                    CreateItem(href, "B", 30d),
                    CreateItem(href, "C", 20d),
                    CreateItem(href, "D", 15d)
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

        private static Entity CreateItem(Uri href, string id, double value)
        {
            return new Entity
            {
                Class = new[] {"item"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"id", id},
                    {"value", value}
                },
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = new Uri(href, $"items/{id}")
                    }
                }
            };
        }
    }
}