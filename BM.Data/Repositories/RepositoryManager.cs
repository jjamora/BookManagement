using BM.Core.Repositories;

namespace BM.Data.Repositories
{
    public class RepositoryManager(DataContext context) : IRepositoryManager, IDisposable
    {
        private readonly DataContext context =
            context ?? throw new ArgumentNullException(nameof(context));

        public IBookRepository BookRepository => new BookRepository(context);

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
