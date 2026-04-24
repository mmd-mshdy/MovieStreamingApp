using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Enums;
using MovieStreaming.Domain.ValueObjects;

namespace MovieStreaming.Domain.Aggregates.Users
{
    public class User : AggregateRoot
    {
        private readonly List<WatchList> _watchLists = new();

        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? PasswordHash { get; private set; }
        public Money? WalletBallance { get; private set; }
        public SubscriptionType SubscriptionType { get; private set; }

        public IReadOnlyCollection<WatchList> WatchLists => _watchLists;

        // EF Core requires this if you're using a parameterized constructor:
        protected User() { }

        public User(Guid id, string name, string email, SubscriptionType subscriptionType)
            : base(id)
        {
            Name = name;
            Email = email;
            SubscriptionType = subscriptionType;
        }


        public void CreateUser(User user)
        {
            var newuser = new User(user.Id,user.Name, user.Email,user.SubscriptionType);
        }
        public void HashPsasword(string hashedPass)
        {
            PasswordHash = hashedPass;
        }
        public void AddWalletBalance(Money money)
        {
            WalletBallance.Add(money);
           
        }
        public void DeductWalletBallance(Money money)
        {
            WalletBallance.Deduct(money);

        }
    }
}
