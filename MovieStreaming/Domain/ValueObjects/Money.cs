using MovieStreaming.Domain.Common;

namespace MovieStreaming.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount {  get; }
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


    }
}
