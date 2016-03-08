using System.Collections.Generic;
using System.Linq;
using Api.ValueObjects;

namespace Api.Repositories
{
    public class ItemRepository
    {
        private static readonly IReadOnlyCollection<Item> Items = new[]
        {
            new Item("A", new Money("GBP", 50d)),
            new Item("B", new Money("GBP", 30d)),
            new Item("C", new Money("GBP", 20d)),
            new Item("D", new Money("GBP", 15d))
        };

        public IEnumerable<Item> Get()
        {
            return Items;
        }

        public Item Get(string id)
        {
            return Items.Single(item => item.Id == id);
        }
    }
}