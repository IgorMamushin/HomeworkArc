using Dapper;

namespace DataAccess.Repositories.Impl
{
    internal class FriendRepository : IFriendRepository
    {
        private const string InsertFriendQuery = "INSERT INTO friend (user_id, friend_id) VALUES (@UserId, @FriendId) ON CONFLICT DO NOTHING";
        private const string DeleteFriendQuery = "DELETE FROM friend where user_id = @UserId AND friend_id = @FriendId";

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public FriendRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> DeleteFriend(Guid userId, Guid friendId, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(DeleteFriendQuery, parameters: new { UserId=userId, FriendId=friendId }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            return await connection.ExecuteScalarAsync<bool>(command);
        }

        public async Task SetFriend(Guid userId, Guid friendId, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(InsertFriendQuery, parameters: new { UserId=userId, FriendId=friendId }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            return await connection.ExecuteScalarAsync<bool>(command);
        }
    }
}
