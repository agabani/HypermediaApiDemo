using System;
using System.Collections.Generic;
using System.Linq;
using Api.Repositories;
using Api.Siren;
using Api.ValueObjects;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class ItemsModule : Module
    {
        private readonly ItemRepository _itemRepository = new ItemRepository();

        public ItemsModule(HttpRequest request) : base(request)
        {
        }

        public Entity Handle()
        {
            var items = _itemRepository.Get();

            return new Entity
            {
                Class = BuildClass(),
                Entities = BuildEntities(items),
                Links = BuildLinks()
            };
        }

        private static string[] BuildClass()
        {
            return new[] {"items", "collection"};
        }

        private Entity[] BuildEntities(IEnumerable<Item> items)
        {
            return items.Select(item => new AnemicItemModule(Request, item.Id).Handle()).ToArray();
        }

        private Link[] BuildLinks()
        {
            return new[]
            {
                new Link
                {
                    Rel = new[] {"self"},
                    Href = new Uri(BaseAddress, "/items")
                },
                new Link
                {
                    Rel = new[] {"basket"},
                    Href = new Uri(BaseAddress, "/basket")
                }
            };
        }
    }
}