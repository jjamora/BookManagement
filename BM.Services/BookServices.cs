using BM.Core.Model;
using BM.Core.Repositories;

namespace BM.Services
{
    public interface IBookServices
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task AddBook(Book book);
    }

    public class BookServices(IRepositoryManager repository) : IBookServices
    {
        private readonly IRepositoryManager repository =
            repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task AddBook(Book book)
        {
            await repository.BookRepository.AddAsync(book);
            await repository.CommitAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await repository.BookRepository.GetAllBooks();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await repository.BookRepository.GetBookById(id);
        }
    }
}
