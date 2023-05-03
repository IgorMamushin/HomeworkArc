using Models;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<Guid> CreateUser(User user, CancellationToken cancellationToken);
        Task<User?> GetUser(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserDto>> Search(string lastName, string firstName, CancellationToken cancellationToken);

        Task<bool> UserExist(Guid id, CancellationToken cancellationToken);
    }
}
