using System;
using System.Collections.Generic;
using Api.Extensions;
using Api.Repositories;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;
using Action = Api.Siren.Action;

namespace Api.Modules
{
    public class ItemModule : Module
    {
        private readonly bool _anemic;
        private readonly Uri _href;
        private readonly string _id;

        private readonly ItemRepository _itemRepository = new ItemRepository();

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

        public Entity BuildEntity()
        {
            var item = _itemRepository.Get(_id);

            var links = !_anemic
                ? new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = _href
                    },
                    new Link
                    {
                        Rel = new[] {"items"},
                        Href = new Uri(Request.GetBaseAddress(), "items")
                    },
                    new Link
                    {
                        Rel = new[] {"basket"},
                        Href = new Uri(Request.GetBaseAddress(), "basket")
                    }
                }
                : new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = _href
                    }
                };


            return new Entity
            {
                Class = new[] {"item"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"id", item.Id},
                    {"value", item.Value.Units}
                },
                Entities = new Entity[] {},
                Links = links,
                Actions = _anemic
                    ? null
                    : new[]
                    {
                        new Action
                        {
                            Name = "basket-add",
                            Href = new Uri(_href, "/basket"),
                            Method = "POST",
                            Type = "application/x-www-form-urlencoded",
                            Fields = new[]
                            {
                                new Field
                                {
                                    Type = "text",
                                    Name = "id",
                                    Value = item.Id
                                }
                            },
                            Title = "Add to basket"
                        }
                    }
            };
        }
    }
}