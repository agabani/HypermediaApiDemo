using System.Collections.Generic;
using System.Linq;
using Api.ValueObjects;

namespace Api.DomainObjects
{
    public class Checkout
    {
        private static readonly List<DiscoutRule> DiscoutRules = new List<DiscoutRule>
        {
            new DiscoutRule("A", 3, new Money("GBP", 20)),
            new DiscoutRule("B", 2, new Money("GBP", 15))
        };

        public Money GetTotal(Basket basket)
        {
            var price = new Money("GBP", 0);

            foreach (var item in basket.Items)
            {
                price += item.Value;
            }

            foreach (var discoutRule in DiscoutRules)
            {
                price -= discoutRule.CalculateDiscout(basket.Items);
            }

            return price;
        }

        private sealed class DiscoutRule
        {
            private readonly Money _discount;
            private readonly string _item;
            private readonly int _quantity;

            public DiscoutRule(string item, int quantity, Money discount)
            {
                _item = item;
                _quantity = quantity;
                _discount = discount;
            }

            public Money CalculateDiscout(IEnumerable<Item> items)
            {
                var numberOfDiscounts = items.Count(item => item.Id.Equals(_item))/_quantity;
                return _discount*numberOfDiscounts;
            }
        }
    }
}