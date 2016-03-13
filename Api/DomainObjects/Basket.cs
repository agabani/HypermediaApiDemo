using System.Collections.Generic;
using System.Linq;
using Api.ValueObjects;

namespace Api.DomainObjects
{
    public class Basket
    {
        private readonly List<Item> _items;

        public Basket()
        {
            _items = new List<Item>();
        }

        public Basket(Basket basket)
        {
            _items = basket.Items.ToList();
        }

        public IReadOnlyList<Item> Items => _items;

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }
    }
}