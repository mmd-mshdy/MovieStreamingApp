using Dapper;
using Microsoft.EntityFrameworkCore;
using MovieStreaming.Application.Interfaces;
using MovieStreaming.Domain.Aggregates.Movies;
using System.Data;

namespace MovieStreaming.Infrastructure.Repository
{

    public class CastMemberRepository : ICastMemberRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;

        public CastMemberRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task<CastMembers> CreateAsync(CastMembers member)
        {
            var query = @"SELECT Id , Name FROM Castmembers WHERE Id = @Id";
            var check = _dbConnection.QueryAsync<CastMembers>(query);
            if (member == null) throw new ArgumentNullException(nameof(member));
            if (check != null) throw new InvalidOperationException();
            await _context.AddAsync(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task DeleteAsync(Guid id)
        {
            var member = await GetByIdAsync(id);
            if(member == null) throw new ArgumentNullException(nameof(member));
            _context.Remove(member);
        }

        public async Task<IEnumerable<CastMembers>> GetByFullNameAsync(string name, string familyname)
        {
            var query = @"SELECT * FROM Castmembers WHERE Name = @Name AND FamilyName = @FamilyName";
            IEnumerable<CastMembers> members = await _dbConnection.QueryAsync<CastMembers>(query, new { Name = name , FamilyName = familyname });
            return members;
        }

        public async Task<CastMembers> GetByIdAsync(Guid id)
        {
            var query = @"SELECT * , Name FROM Castmembers WHERE Id = @Id";
            var member = await _dbConnection.QueryFirstOrDefaultAsync(query, new { Id = id });
            if (member == null) throw new ArgumentNullException();
            return member;
        }

        public async Task<CastMembers> UpdateAsync(CastMembers member)
        {
            var modifiedmember = await _context.CastMembers.FirstOrDefaultAsync(m => m.Id == member.Id);
            if (modifiedmember == null) throw new ArgumentNullException();
            await _context.SaveChangesAsync();
            return modifiedmember;
            
        }
    }
}
