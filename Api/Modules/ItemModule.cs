using System;
using System.Collections.Generic;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;
using Action = Api.Siren.Action;

namespace Api.Modules
{
    public class ItemModule : Module
    {
        private readonly Uri _href;
        private readonly string _id;
        private readonly bool _anemic;

        public ItemModule(HttpRequest request, string path, string id) : base(request)
        {
            _id = id;
            _href = new Uri($"{request.Scheme}://{request.Host.Value}/{path}/{id}");
            _anemic = true;
        }

        public ItemModule(HttpRequest request, string id) : base(request)
        {
            _href = new Uri(Request.GetDisplayUrl());
            _id = id;
            _anemic = false;
        }

        public override Entity BuildEntity()
        {
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
                        Href = _href
                    }
                },
                Actions = _anemic ? null : new []
                {
                    new Action
                    {
                        Name = "basket-add",
                        Href = new Uri(_href, "/basket"),
                        Method = "POST",
                        Type = "application/x-www-form-urlencoded",
                        Fields = new []
                        {
                            new Field
                            {
                                Type = "text",
                                Name = "id",
                                Value = _id
                            }
                        },
                        Title = "Add to basket"
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