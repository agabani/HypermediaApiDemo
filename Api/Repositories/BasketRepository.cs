using System;
using System.Collections.Generic;
using Api.DomainObjects;

namespace Api.Repositories
{
    public class BasketRepository
    {
        private static readonly Dictionary<Guid, Basket> Baskets = new Dictionary<Guid, Basket>();

        public void Save(Account account, Basket basket)
        {
            Baskets[account.Id] = new Basket(basket);
        }

        public Basket Get(Account account)
        {
            return Baskets.ContainsKey(account.Id)
                ? new Basket(Baskets[account.Id])
                : new Basket();
        }
    }
}