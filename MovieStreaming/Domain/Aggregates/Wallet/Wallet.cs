using MovieStreaming.Domain.ValueObjects;
using MovieStreaming.Domain.Aggregates.Transactions;
using MovieStreaming.Domain.Enums;

namespace MovieStreaming.Domain.Aggregates.Wallet
{
    public sealed class Wallet : AggregateRoot
    {
        public Guid UserId { get; private set; }
        public Money Balance { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Wallet() { } // For EF Core

        public Wallet(Guid userId, string Toman)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Toman = "Toman";
            Balance = Money.Zero(Toman);
        }

        public void Deposit(Money amount)
        {
            Balance.Add(amount);
            _transactions.Add(new Transaction(Guid.NewGuid(),Id, amount, TransactionType.Deposit));
        }

        public void ProcessPayment(Money amount)
        {
            // Subtract method handles the "Insufficient Funds" domain rule natively
            Balance.Deduct(amount);
            _transactions.Add(new Transaction(Guid.NewGuid(),Id, amount, TransactionType.SubscriptionPayment));
        }
    }
}
