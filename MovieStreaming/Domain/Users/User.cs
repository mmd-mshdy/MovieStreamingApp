using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Enums;

namespace MovieStreaming.Domain.Users
{
    public class User:AggregateRoot<Guid>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set;}
        public string Email{ get; private set;}
        public string PasswordHash { get; private set;}
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
