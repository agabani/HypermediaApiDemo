using System;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class BasketModule : Module
    {
        public BasketModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity()
        {
            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"basket", "collection"},
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