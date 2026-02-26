using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Enums;
using MovieStreaming.Domain.ValueObjects;

namespace MovieStreaming.Domain.Aggregates.Users
{
    public class User:AggregateRoot
    {
        public string Name { get; private set;}
        public string Email{ get; private set;}
        public string PasswordHash { get; private set;}
        public Money WalletBallance { get; private set;}
        public SubscriptionType SubscriptionType { get; private set;}
        internal User(string name, string email, string password, SubscriptionType subscriptionType)
        {
            name = Name;
            email = Email;
            PasswordHash = password;
            SubscriptionType = subscriptionType;
        }
        public void CreateUser(User user)
        {
            var newuser = new User(user.Name, user.Email, user.PasswordHash,user.SubscriptionType);
        }

    }
}
