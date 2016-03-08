using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Repositories
{
    public class BasketRepository
    {
        private static readonly List<Basket> Baskets = new List<Basket>();

        public void AddItem(Guid accountId, string itemId)
        {
            var basket = Baskets.SingleOrDefault(b => b.AccountId == accountId);

            if (basket == null)
            {
                basket = new Basket
                {
                    AccountId = accountId,
                    ItemIds = new List<string>()
                };

                Baskets.Add(basket);
            }

            basket.ItemIds.Add(itemId);
        }

        public List<string> GetBasket(Guid accountId)
        {
            var single = Baskets.SingleOrDefault(basket => basket.AccountId == accountId);

            if (single == null)
            {
                single = new Basket
                {
                    AccountId = accountId,
                    ItemIds = new List<string>()
                };

                Baskets.Add(single);
            }

            return single.ItemIds;
        }

        public class Basket
        {
            public Guid AccountId { get; set; }
            public List<string> ItemIds { get; set; }
        }
    }
}