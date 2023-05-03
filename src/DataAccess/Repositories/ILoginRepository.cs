using Models;

namespace DataAccess.Repositories
{
    public interface ILoginRepository
    {
        Task<Guid?> Login(Guid id, string password, CancellationToken cancellationToken);

        Task<AuthUserInfo?> GetAuthUserInfo(Guid token, CancellationToken cancellationToken); 
    }
}
