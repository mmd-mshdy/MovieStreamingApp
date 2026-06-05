using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking; // For ChangeTracker
using Microsoft.EntityFrameworkCore.Infrastructure; // For GetService

namespace MovieStreaming.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository MovieRepository { get; }
        IUserRepository UserRepository { get; }

        // Access to EF Core's ChangeTracker
        ChangeTracker ChangeTracker { get; }

        // Access to EF Core's DbContext
        DbContext Context { get; }

        // Method to save changes to the database
        Task<int> CommitAsync(CancellationToken cancellationToken = default);

        // Method to rollback changes if needed (often handled by transaction scope)
        // Task RollbackAsync();
    }
}
