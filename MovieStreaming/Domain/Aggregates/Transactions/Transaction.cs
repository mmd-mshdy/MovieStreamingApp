using MovieStreaming.Domain.Enums;
using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.ValueObjects;

namespace MovieStreaming.Domain.Aggregates.Transactions
{
    public sealed class Transaction : Entity
    {
        public Guid WalletId { get; private set; }
        public Money Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Transaction(Guid id) : base(id) { } // For EF Core

        internal Transaction(Guid id ,Guid walletId, Money amount, TransactionType type) :base(id)
        {
            id = Guid.NewGuid();
            WalletId = walletId;
            Amount = amount;
            Type = type;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
