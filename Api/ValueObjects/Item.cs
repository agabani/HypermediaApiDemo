namespace Api.ValueObjects
{
    public class Item
    {
        public Item(string id, Money value)
        {
            Id = id;
            Value = value;
        }

        public string Id { get; }
        public Money Value { get; }
    }
}