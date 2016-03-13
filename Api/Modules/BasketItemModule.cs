using Api.Builders;
using Api.Repositories;
using Api.Siren;
using Microsoft.AspNet.Http;

namespace Api.Modules
{
    public class BasketItemModule : Module
    {
        private static readonly ItemRepository ItemRepository = new ItemRepository();
        private readonly string _id;

        public BasketItemModule(HttpRequest request, string id) : base(request)
        {
            _id = id;
        }

        public Entity Handle()
        {
            var item = ItemRepository.Get(_id);

            return new EntityBuilder()
                .WithClass("item")
                .WithProperty("id", item.Id)
                .WithProperty("value", item.Value.Units)
                .WithLink(() => LinkFactory.Create("item", _id, true))
                .WithAction(() => ActionFactory.Create("basket", "post", item))
                .WithAction(() => ActionFactory.Create("basket", "delete", item))
                .Build();
        }
    }
}