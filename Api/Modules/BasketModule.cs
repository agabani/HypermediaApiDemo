using System;
using System.Collections.Generic;
using System.Linq;
using Api.DomainObjects;
using Api.Extensions;
using Api.Repositories;
using Api.Siren;
using Api.ValueObjects;
using Api.ViewModels;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Primitives;

namespace Api.Modules
{
    public class BasketModule : Module
    {
        private readonly AccountRepository _accountRepository = new AccountRepository();
        private readonly BasketRepository _basketRepository = new BasketRepository();
        private readonly ItemRepository _itemRepository = new ItemRepository();

        public BasketModule(HttpRequest request) : base(request)
        {
        }

        public Entity BuildEntity(BasketAddModel model)
        {
            StringValues stringValues;

            var account = Request.Headers.TryGetValue("authorization", out stringValues)
                ? _accountRepository.GetByToken(Guid.Parse(stringValues.First().Split(' ').Last()))
                : _accountRepository.CreateAnonymous();

            var basket = _basketRepository.Get(account);
            basket.AddItem(_itemRepository.Get(model.Id));
            _basketRepository.Save(account, basket);

            var entities =
                basket.Items.Select(item => item.Id)
                    .Select(itemId => new ItemModule(Request, "items", itemId).BuildEntity())
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
                    {"price", GetPrice(basket).Units}
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
                ? _accountRepository.GetByToken(Guid.Parse(stringValues.First().Split(' ').Last()))
                : _accountRepository.CreateAnonymous();

            var basket = _basketRepository.Get(account);

            return new Entity
            {
                Class = new[] {"basket", "collection"},
                Properties = new Dictionary<string, dynamic>
                {
                    {"price", GetPrice(basket).Units}
                },
                Entities =
                    basket.Items.Select(item => item.Id)
                        .Select(s => new ItemModule(Request, "items", s).BuildEntity())
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

        public static Money GetPrice(Basket basket)
        {
            return new Checkout().GetTotal(basket);
        }
    }
}