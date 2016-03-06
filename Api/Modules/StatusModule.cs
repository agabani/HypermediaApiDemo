using System;
using System.Collections.Generic;
using Api.Extensions;
using Api.Siren;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class StatusModule : Module
    {
        public StatusModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity()
        {
            return new Entity
            {
                Class = new[] {"status"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"currentUtcTime", DateTime.UtcNow}
                },
                Entities = new Entity[] {},
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = Request.GetAbsoluteAddress()
                    }
                }
            };
        }
    }
}