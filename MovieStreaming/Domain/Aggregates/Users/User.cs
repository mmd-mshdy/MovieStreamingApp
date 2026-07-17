using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Enums;

namespace MovieStreaming.Domain.Aggregates.Users
{
    public class User : AggregateRoot
    {
        private readonly List<WatchList> _watchLists = new();

        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? PasswordHash { get; private set; }
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
        public void HashPassword(string hashedPass)
        {
            PasswordHash = hashedPass;
        }
        public void AddWalletBalance(string money)
        {
           
        }
        public void DeductWalletBallance(string money)
        {

        }
    }
}
