using System;
using System.Collections.Generic;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class ItemModule : Module
    {
        private readonly string _id;

        public ItemModule(HttpRequest request, string id) : base(request)
        {
            _id = id;
        }

        public override Entity BuildEntity()
        {
            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"item"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"id", _id},
                    {"value", Value(_id)}
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

        private static double Value(string id)
        {
            switch (id)
            {
                case "A":
                    return 50d;
                case "B":
                    return 30d;
                case "C":
                    return 20d;
                case "D":
                    return 15d;
                default:
                    throw new ArgumentNullException();
            }
        }
    }
}