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
            var account = GetAccount();

            var basket = BasketRepository.Get(account);
            basket.AddItem(ItemRepository.Get(model.Id));
            BasketRepository.Save(account, basket);

            return BuildEntity(basket, account);
        }

        public Entity Handle()
        {
            var account = GetAccount();

            var basket = BasketRepository.Get(account);

            return BuildEntity(basket, account);
        }

        private Entity BuildEntity(Basket basket, Account account)
        {
            return new EntityBuilder()
                .WithClass("basket")
                .WithClass("collection")
                .WithProperty("price", Checkout.GetTotal(basket).Units)
                .WithEntity(BuildEntities(basket, account).Select<Entity, Func<Entity>>(e => () => e))
                .WithLink(() => LinkFactory.Create("basket", true))
                .WithLink(() => LinkFactory.Create("items", false))
                .Build();
        }

        private Account GetAccount()
        {
            StringValues stringValues;

            return Request.Headers.TryGetValue("authorization", out stringValues)
                ? AccountRepository.GetByToken(Guid.Parse(stringValues.Single().Split(' ').Last()))
                : AccountRepository.CreateAnonymous();
        }

        private IEnumerable<Entity> BuildEntities(Basket basket, Account account)
        {
            var builder = new EntityBuilder();

            if (!Request.Headers.ContainsKey("authorization"))
            {
                builder.WithEntity(() =>
                    new EntityBuilder()
                        .WithClass("http")
                        .WithProperty("authorization", $"Basic {account.Token}")
                        .Build());
            }

            builder
                .WithEntity(basket.Items
                    .Select(item => item.Id)
                    .Select(id => new AnemicItemModule(Request, id).Handle())
                    .Select<Entity, Func<Entity>>(entity => () => entity));

            return builder.Build().Entities;
        }
    }
}