using BM.Core.DTO;
using BM.Core.Mapper;
using BM.Core.Repositories;

namespace BM.Services
{
    public interface IBookServices
    {
        Task<IEnumerable<BookDTO>> GetAllBooks();
        Task<BookDTO> GetBookById(int id);
        Task AddBook(BookDTO book);
    }

    public class BookServices(IRepositoryManager repository) : IBookServices
    {
        private readonly IRepositoryManager repository =
            repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task AddBook(BookDTO book)
        {
            await repository.BookRepository.AddAsync(book.DtoToModel());
            await repository.CommitAsync();
        }

        public async Task<IEnumerable<BookDTO>> GetAllBooks()
        {
            var list = await repository.BookRepository.GetAllBooks();
            return list.Select(o => o.ModelToDTO());
        }

        public async Task<BookDTO> GetBookById(int id)
        {
            var result = await repository.BookRepository.GetBookById(id);
            return result.ModelToDTO();
        }
    }
}
