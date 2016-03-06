using System.Collections.Generic;
using System.Linq;

namespace Api.Repositories
{
    public class ItemRepository
    {
        private static readonly IReadOnlyCollection<Item> Items = new[]
        {
            new Item {Id = "A", Value = 50d},
            new Item {Id = "B", Value = 30d},
            new Item {Id = "C", Value = 20d},
            new Item {Id = "D", Value = 15d}
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
            public string Id { get; set; }
            public double Value { get; set; }
        }
    }
}