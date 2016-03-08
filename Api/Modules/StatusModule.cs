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
                Class = BuildClass(),
                Properties = BuildProperties(),
                Entities = BuildEntities(),
                Links = BuildLinks()
            };
        }

        private static string[] BuildClass()
        {
            return new[] {"status"};
        }

        private static Dictionary<string, dynamic> BuildProperties()
        {
            return new Dictionary<string, dynamic>
            {
                {"currentUtcTime", DateTime.UtcNow}
            };
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
                    Href = Request.GetAbsoluteAddress()
                }
            };
        }
    }
}