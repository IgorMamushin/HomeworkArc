using Models;

namespace DataAccess.Repositories
{
    public interface IPostRepository
    {
        Task<long> Create(Guid userId, string text, CancellationToken cancellationToken);
        Task Update(Guid userId, long postId, string text, CancellationToken cancellationToken);

        Task Delete(Guid userId, long postId, CancellationToken cancellationToken);

        Task<PostDto?> Get(long postId, CancellationToken cancellationToken);

        Task<IReadOnlyList<PostDto>> Feed(Guid userId, int offset, int limit, CancellationToken cancellationToken);
    }
}
