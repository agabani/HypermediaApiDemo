using System.Collections.Generic;
using Api.Factories;
using Api.Repositories;
using Api.Siren;
using Api.ValueObjects;
using Microsoft.AspNet.Http;
using Action = Api.Siren.Action;

namespace Api.Modules
{
    public class ItemModule : Module
    {
        private static readonly ItemRepository ItemRepository = new ItemRepository();
        protected readonly string Id;

        public ItemModule(HttpRequest request, string id) : base(request)
        {
            Id = id;
        }

        public Entity Handle()
        {
            var item = ItemRepository.Get(Id);

            return new Entity
            {
                Class = BuildClass(),
                Properties = BuildProperties(item),
                Entities = BuildEntities(),
                Links = BuildLinks(),
                Actions = BuildActions(item)
            };
        }

        private static string[] BuildClass()
        {
            return new[] {"item"};
        }

        private static Dictionary<string, dynamic> BuildProperties(Item item)
        {
            return new Dictionary<string, dynamic>
            {
                {"id", item.Id},
                {"value", item.Value.Units}
            };
        }

        private static Entity[] BuildEntities()
        {
            return new Entity[] {};
        }

        protected virtual Action[] BuildActions(Item item)
        {
            var actionFactory = new ActionFactory(BaseAddress);

            return new[]
            {
                actionFactory.Create("basket", "post", item)
            };
        }

        protected virtual Link[] BuildLinks()
        {
            return new[]
            {
                LinkFactory.Create("item", Id, true),
                LinkFactory.Create("items", false),
                LinkFactory.Create("basket", false)
            };
        }
    }
}