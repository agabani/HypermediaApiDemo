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

        public Entity Handle(string itemId)
        {
            var account = GetAccount();

            var basket = BasketRepository.Get(account);
            basket.RemoveItem(basket.Items.First(item => item.Id == itemId));
            BasketRepository.Save(account, basket);

            return BuildEntity(basket, account);
        }

        private Entity BuildEntity(Basket basket, Account account)
        {
            var entityBuilder = new EntityBuilder()
                .WithClass("basket")
                .WithClass("collection")
                .WithProperty("price", Checkout.GetTotal(basket).Units)
                .WithLink(() => LinkFactory.Create("basket", true))
                .WithLink(() => LinkFactory.Create("items", false));

            return WithSubEntities(entityBuilder, basket, account).Build();
        }

        private Account GetAccount()
        {
            StringValues stringValues;

            return Request.Headers.TryGetValue("authorization", out stringValues)
                ? AccountRepository.GetByToken(Guid.Parse(stringValues.Single().Split(' ').Last()))
                : AccountRepository.CreateAnonymous();
        }

        private EntityBuilder WithSubEntities(EntityBuilder entityBuilder, Basket basket, Account account)
        {
            if (!Request.Headers.ContainsKey("authorization"))
            {
                entityBuilder.WithEntity(() =>
                    new EntityBuilder()
                        .WithClass("http")
                        .WithProperty("authorization", $"Basic {account.Token}")
                        .Build());
            }

            entityBuilder
                .WithEntity(basket.Items
                    .Select(item => item.Id)
                    .Select(id => new BasketItemModule(Request, id).Handle())
                    .GroupBy(item => item.Properties["id"])
                    .Select(grouping =>
                    {
                        var entity = grouping.First();
                        entity.Properties["quantity"] = grouping.Count();
                        return entity;
                    })
                    .Select<Entity, Func<Entity>>(entity => () => entity));

            return entityBuilder;
        }
    }
}