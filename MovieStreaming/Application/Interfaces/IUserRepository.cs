using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Application.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
        Task<User> GetUserById(Guid id);
        Task<IEnumerable<User>> GetUserByName(string name);
        Task<IEnumerable<User>> GetUserByEmail(string email);
    }
}
