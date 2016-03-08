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
        private static readonly List<IPriceDeltaRule> PriceDeltaRules = new List<IPriceDeltaRule>
        {
            new PriceDeltaRule("A", 50),
            new PriceDeltaRule("B", 30),
            new PriceDeltaRule("C", 20),
            new PriceDeltaRule("D", 15),
            new MultiPriceDeltaRule("A", 3, -20),
            new MultiPriceDeltaRule("B", 2, -15)
        };

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
//                    {"price", GetPrice(basket.Items.Select(item => item.Id))}
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
//                    {"price", GetPrice(basket.Items.Select(item => item.Id))}
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

        [Obsolete]
        private static double GetPrice(IEnumerable<string> items)
        {
            return PriceDeltaRules.Sum(rule => rule.CalculatePriceDelta(items));
        }

        public static Money GetPrice(Basket basket)
        {
            return new Checkout().GetTotal(basket);
        }

        private interface IPriceDeltaRule
        {
            double CalculatePriceDelta(IEnumerable<string> items);
        }

        private sealed class MultiPriceDeltaRule : IPriceDeltaRule
        {
            private readonly double _discount;
            private readonly string _item;
            private readonly int _quantity;

            public MultiPriceDeltaRule(string item, int quantity, double discount)
            {
                _item = item;
                _quantity = quantity;
                _discount = discount;
            }

            public double CalculatePriceDelta(IEnumerable<string> items)
            {
                return items.Count(item => item.Equals(_item))/_quantity*_discount;
            }
        }

        private sealed class PriceDeltaRule : IPriceDeltaRule
        {
            private readonly string _item;
            private readonly double _price;

            public PriceDeltaRule(string item, double price)
            {
                _item = item;
                _price = price;
            }

            public double CalculatePriceDelta(IEnumerable<string> items)
            {
                return items.Count(item => item.Equals(_item))*_price;
            }
        }
    }
}