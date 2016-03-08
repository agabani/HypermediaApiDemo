using System;
using System.Collections.Generic;
using System.Linq;
using Api.DomainObjects;
using Api.Extensions;
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

        public Entity BuildEntity(BasketAddModel model)
        {
            StringValues stringValues;

            var account = Request.Headers.TryGetValue("authorization", out stringValues)
                ? AccountRepository.GetByToken(Guid.Parse(stringValues.First().Split(' ').Last()))
                : AccountRepository.CreateAnonymous();

            var basket = BasketRepository.Get(account);
            basket.AddItem(ItemRepository.Get(model.Id));
            BasketRepository.Save(account, basket);

            var entities =
                basket.Items.Select(item => item.Id)
                    .Select(itemId => new AnemicItemModule(Request, itemId).BuildEntity())
                    .ToArray();

            var httpEntity = new Entity
            {
                Class = new[] {"http"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"authorization", $"Basic {account.Token}"}
                }
            };

            return new Entity
            {
                Class = new[] {"basket", "collection"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"price", Checkout.GetTotal(basket).Units}
                },
                Entities =
                    stringValues == default(StringValues) ? new[] {httpEntity}.Concat(entities).ToArray() : entities,
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = Request.GetAbsoluteAddress()
                    },
                    new Link
                    {
                        Rel = new[] {"items"},
                        Href = new Uri(Request.GetBaseAddress(), "items")
                    }
                }
            };
        }

        public Entity BuildEntity()
        {
            StringValues stringValues;

            var account = Request.Headers.TryGetValue("authorization", out stringValues)
                ? AccountRepository.GetByToken(Guid.Parse(stringValues.First().Split(' ').Last()))
                : AccountRepository.CreateAnonymous();

            var basket = BasketRepository.Get(account);

            return new Entity
            {
                Class = new[] {"basket", "collection"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"price", Checkout.GetTotal(basket).Units}
                },
                Entities =
                    basket.Items.Select(item => item.Id)
                        .Select(s => new AnemicItemModule(Request, s).BuildEntity())
                        .ToArray(),
                Links = new[]
                {
                    new Link
                    {
                        Rel = new[] {"self"},
                        Href = Request.GetAbsoluteAddress()
                    },
                    new Link
                    {
                        Rel = new[] {"items"},
                        Href = new Uri(Request.GetBaseAddress(), "items")
                    }
                }
            };
        }
    }
}