using MovieStreaming.Domain.Common;

namespace MovieStreaming.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency {get;}
        private Money() { }
        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }
        public Money(decimal amount, string currency)
        {
            if(amount < 0)  throw new InvalidOperationException();
            this.Amount = decimal.Round(amount,2,MidpointRounding.ToEven);
            if (string.IsNullOrWhiteSpace(currency)) throw new InvalidOperationException();
            this.Currency = currency;
        }
        public void Add(Money money)
        {
            if (money == null) throw new ArgumentNullException(nameof(money));
            if(money.Amount < 0) throw new InvalidOperationException();
            if(money.Currency != Currency) throw new InvalidOperationException();
            Amount += money.Amount;
        }
        public void Deduct (Money money)
        {
            if(money == null) throw new ArgumentNullException(nameof(money));
            if (money.Currency != Currency) throw new InvalidOperationException();
            if (money.Amount< this.Amount)throw new InvalidOperationException();
            Amount -= money.Amount;
        }

    }
}
