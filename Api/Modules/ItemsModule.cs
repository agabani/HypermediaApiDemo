using System;
using System.Collections.Generic;
using System.Linq;
using Api.Builders;
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

        public Entity Handle()
        {
            return new EntityBuilder()
                .WithClass("items")
                .WithClass("collection")
                .WithEntity(BuildItems().Select<Entity, Func<Entity>>(entity => () => entity))
                .WithLink(() => LinkFactory.Create("items", true))
                .WithLink(() => LinkFactory.Create("basket", false)).Build();
        }

        private IEnumerable<Entity> BuildItems()
        {
            return _itemRepository.Get()
                .Select(item => new AnemicItemModule(Request, item.Id).Handle());
        }
    }
}