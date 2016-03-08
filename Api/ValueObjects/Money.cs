using System;

namespace Api.ValueObjects
{
    public class Money
    {
        public Money(string currency, double units)
        {
            if (currency == null) throw new ArgumentNullException(nameof(currency));

            Currency = currency.ToUpper();
            Units = units;
        }

        public string Currency { get; }
        public double Units { get; }

        public static Money operator +(Money a, Money b)
        {
            if (!a.Currency.Equals(b.Currency)) throw new NotSupportedException();

            return new Money(a.Currency, a.Units + b.Units);
        }

        public static Money operator -(Money a, Money b)
        {
            if (!a.Currency.Equals(b.Currency)) throw new NotSupportedException();

            return new Money(a.Currency, a.Units - b.Units);
        }
    }
}