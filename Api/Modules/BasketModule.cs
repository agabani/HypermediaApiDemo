using System;
using System.Collections.Generic;
using System.Linq;
using Api.Builders;
using Api.DomainObjects;
using Api.Repositories;
using Api.Siren;
using Api.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace Api.Modules
{
    public class BasketModule : Module
    {
        private static readonly AccountRepository AccountRepository = new AccountRepository();
        private static readonly BasketRepository BasketRepository = new BasketRepository();
        private static readonly ItemRepository ItemRepository = new ItemRepository();
        private static readonly Checkout Checkout = new Checkout();

        public BasketModule(HttpRequest request) : base(request)
        {
        }

        public Entity Handle(BasketAddModel model)
        {
            Account account;
            var isExistingAccount = TryGetAccount(out account);

            var basket = BasketRepository.Get(account);
            basket.AddItem(ItemRepository.Get(model.Id));
            BasketRepository.Save(account, basket);

            return new EntityBuilder()
                .WithClass("basket")
                .WithClass("collection")
                .WithProperty("price", Checkout.GetTotal(basket).Units)
                .WithEntity(BuildEntities(basket, account, isExistingAccount).Select<Entity, Func<Entity>>(e => () => e))
                .WithLink(() => LinkFactory.Create("basket", true))
                .WithLink(() => LinkFactory.Create("items", false))
                .Build();
        }

        public Entity Handle()
        {
            Account account;
            TryGetAccount(out account);

            var basket = BasketRepository.Get(account);

            return new EntityBuilder()
                .WithClass("basket")
                .WithClass("collection")
                .WithProperty("price", Checkout.GetTotal(basket).Units)
                .WithEntity(BuildEntities(basket).Select<Entity, Func<Entity>>(e => () => e))
                .WithLink(() => LinkFactory.Create("basket", true))
                .WithLink(() => LinkFactory.Create("items", false))
                .Build();
        }

        private bool TryGetAccount(out Account account)
        {
            StringValues stringValues;

            account = Request.Headers.TryGetValue("authorization", out stringValues)
                ? AccountRepository.GetByToken(Guid.Parse(stringValues.First().Split(' ').Last()))
                : AccountRepository.CreateAnonymous();

            return stringValues != default(StringValues);
        }

        private IEnumerable<Entity> BuildEntities(Basket basket, Account account, bool isAuthenticated)
        {
            var entities = basket.Items
                .Select(item => item.Id)
                .Select(itemId => new AnemicItemModule(Request, itemId).Handle())
                .ToArray();

            var httpEntity = new Entity
            {
                Class = new[] {"http"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"authorization", $"Basic {account.Token}"}
                }
            };

            return isAuthenticated ? entities : new[] {httpEntity}.Concat(entities).ToArray();
        }

        private IEnumerable<Entity> BuildEntities(Basket basket)
        {
            return basket.Items
                .Select(item => item.Id)
                .Select(s => new AnemicItemModule(Request, s).Handle())
                .ToArray();
        }
    }
}