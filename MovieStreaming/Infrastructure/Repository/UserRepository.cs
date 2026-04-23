using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.CreateUser(user);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(Guid id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<IEnumerable<User>> GetUserByName(string name) => await _context.Users.Where(u => u.Name == name)
                                                                                  .ToListAsync();
        public async Task<IEnumerable<User>> GetUserByEmail(string email) => await _context.Users.Where(u => u.Email == email)
                                                                                  .ToListAsync();

        public async Task UpdateUser(User user)
        {
            if (_context.Users.Any(u => u.Id == user.Id)) await _context.SaveChangesAsync();
        }
    }
}
