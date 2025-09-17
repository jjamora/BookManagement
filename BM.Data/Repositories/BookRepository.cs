using BM.Core.Model;
using BM.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BM.Data.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private readonly DataContext context;
        public BookRepository(DataContext context) : base(context)
            => this.context = context ?? throw new ArgumentNullException(nameof(context));

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            var list = await context.Books
                .ToListAsync();

            return list;
        }

        public async Task<Book> GetBookById(int id)
        {
            var result = await context.Books
                .FirstOrDefaultAsync(b => b.Id == id);

            return result!;
        }
    }
}
