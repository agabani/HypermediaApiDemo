using System;
using System.Linq;
using Api.Extensions;
using Api.Repositories;
using Api.Siren;
using Microsoft.AspNet.Http;

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

            return new Entity
            {
                Class = new[] {"items", "collection"},
                Entities = items
                    .Select(item => new AnemicItemModule(Request, item.Id).BuildEntity()).ToArray(),
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = Request.GetAbsoluteAddress()
                    },
                    new Link
                    {
                        Rel = new[] {"basket"},
                        Href = new Uri(Request.GetBaseAddress(), "/basket")
                    }
                }
            };
        }
    }
}