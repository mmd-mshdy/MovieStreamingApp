using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;
using System.Data;
using Dapper;

namespace MovieStreaming.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;
        public UserRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }
        public async Task CreateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.CreateUser(user);
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(Guid id,CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            ArgumentNullException.ThrowIfNull(user);

            return user;
        }

        public async Task<IEnumerable<User>> GetUserByName(string name)
        {

            var query = @"SELECT * FROM Users WHERE Name = @Name ";
            IEnumerable<User> users = await _dbConnection.QueryAsync<User>(query, new { Name = name });
            if (users == null) throw new ArgumentNullException($"{nameof(users)}");
            return users;

        }
        public async Task<User> FindByEmailAsync(string email)
        {
            // EF Core cleanly populates private setters and backing fields!
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
                

            return user;
        }

        public async Task UpdateUser(User user)
        {
            if (_context.Users.Any(u => u.Id == user.Id)) await _context.SaveChangesAsync();
        }
    }
}
