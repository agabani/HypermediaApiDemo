using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Repositories
{
    public class BasketRepository
    {
        private static readonly List<Basket> Baskets = new List<Basket>();

        public void AddItem(AccountRepository.Account account, string itemId)
        {
            var basket = Baskets.SingleOrDefault(b => b.AccountId == account.Id);

            if (basket == null)
            {
                basket = new Basket(account);
                Baskets.Add(basket);
            }

            basket.ItemIds.Add(itemId);
        }

        public List<string> GetItems(AccountRepository.Account account)
        {
            var basket = Baskets.SingleOrDefault(b => b.AccountId == account.Id);

            if (basket == null)
            {
                basket = new Basket(account);
                Baskets.Add(basket);
            }

            return basket.ItemIds;
        }

        public class Basket
        {
            public Basket(AccountRepository.Account account)
            {
                AccountId = account.Id;
                ItemIds = new List<string>();
            }

            public Guid AccountId { get; }
            public List<string> ItemIds { get; }
        }
    }
}