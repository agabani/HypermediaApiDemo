using System.Collections.Generic;
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
                Properties = BuildProperties(),
                Entities = BuildEntities(),
                Links = BuildLinks()
            };
        }

        private static string[] BuildClass()
        {
            return new[] {"root"};
        }

        private static Dictionary<string, dynamic> BuildProperties()
        {
            return new Dictionary<string, dynamic>();
        }

        private static Entity[] BuildEntities()
        {
            return new Entity[] {};
        }

        private Link[] BuildLinks()
        {
            return new[]
            {
                LinkFactory.Create("root", true),
                LinkFactory.Create("items", false),
                LinkFactory.Create("basket", false),
                LinkFactory.Create("status", false)
            };
        }
    }
}