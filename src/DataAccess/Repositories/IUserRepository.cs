using Models;

namespace DataAccess.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<Guid> CreateUser(User user, CancellationToken cancellationToken);
        Task<User?> GetUser(Guid id, CancellationToken cancellationToken);

        Task<Guid?> Login(Guid id, string password, CancellationToken cancellationToken);
    }
}
