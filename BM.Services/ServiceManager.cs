using BM.Core.Repositories;

namespace BM.Services
{
    public interface IServiceManager
    {
        IBookServices BookServices { get; }
    }

    public class ServiceManager(IRepositoryManager repository) : IServiceManager
    {
        private readonly IRepositoryManager repository =
            repository ?? throw new ArgumentNullException(nameof(repository));

        public IBookServices BookServices => new BookServices(repository);
    }
}
