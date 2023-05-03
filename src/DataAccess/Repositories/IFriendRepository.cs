namespace DataAccess.Repositories
{
    public interface IFriendRepository
    {
        Task SetFriend(Guid userId, Guid friendId, CancellationToken cancellationToken);
        Task<bool> DeleteFriend(Guid userId, Guid friendId, CancellationToken cancellationToken);
    }
}
