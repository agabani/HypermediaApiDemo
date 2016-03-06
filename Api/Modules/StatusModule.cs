using System;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class StatusModule : Module
    {
        public StatusModule(HttpRequest request) : base(request)
        {
        }

        public override Entity BuildEntity()
        {
            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"status"},
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