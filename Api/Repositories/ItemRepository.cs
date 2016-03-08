using System.Collections.Generic;
using System.Linq;

namespace Api.Repositories
{
    public class ItemRepository
    {
        private static readonly IReadOnlyCollection<Item> Items = new[]
        {
            new Item("A", 50d),
            new Item("B", 30d),
            new Item("C", 20d),
            new Item("D", 15d)
        };

        public IEnumerable<Item> Get()
        {
            return Items;
        }

        public Item Get(string id)
        {
            return Items.Single(item => item.Id == id);
        }

        public class Item
        {
            public Item(string id, double value)
            {
                Id = id;
                Value = value;
            }

            public string Id { get; }
            public double Value { get; }
        }
    }
}