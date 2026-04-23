using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieStreaming.Application.Interfaces;

namespace MovieStreaming.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;

        private bool disposedValue;

        public UnitOfWork(ApplicationDbContext context, IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _context = context;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        public IMovieRepository MovieRepository =>_movieRepository;

        public IUserRepository UserRepository => _userRepository;

        public ChangeTracker ChangeTracker => _context.ChangeTracker;

        public DbContext Context => _context;

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            var affectedRows = await _context.SaveChangesAsync(cancellationToken);
            _context.ChangeTracker.Clear();
            return affectedRows;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            _context.Dispose();
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
