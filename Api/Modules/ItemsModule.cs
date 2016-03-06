using System;
using System.Linq;
using Api.Repositories;
using Api.Siren;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Extensions;

namespace Api.Modules
{
    public class ItemsModule : Module
    {
        private readonly ItemRepository _itemRepository = new ItemRepository();

        public ItemsModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity()
        {
            var items = _itemRepository.Get();

            var href = new Uri(Request.GetDisplayUrl());

            return new Entity
            {
                Class = new[] {"items", "collection"},
                Entities = items
                    .Select(item => new ItemModule(Request, "items", item.Id).BuildEntity()).ToArray(),
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