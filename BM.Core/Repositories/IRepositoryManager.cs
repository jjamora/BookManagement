namespace BM.Core.Repositories
{
    public interface IRepositoryManager : IDisposable
    {
        IBookRepository BookRepository { get; }

        Task<int> CommitAsync();
    }
}
