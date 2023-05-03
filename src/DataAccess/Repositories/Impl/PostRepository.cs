using Dapper;
using Models;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Repositories.Impl
{
    internal class PostRepository : IPostRepository
    {
        private const string InsertPostQuery = 
"INSERT INTO post (user_id, text, created_at) VALUES (@UserId, @Text, @CreatedAt) RETURNING  id;";
        private const string UpdatePostQuery = 
"UPDATE post set text=@text where id = @Id AND user_id = @UserId;";
        private const string DeletePostQuery = "DELETE FROM post where id = @Id;";

        private const string SelectPostQuery = 
"SELECT p.id as Id, a.last_name || ' ' || a.first_name as UserName, p.\"text\" as \"Text\", p.created_at as CreatedAt FROM post p JOIN app_user a ON p.user_id = a.id where p.id = @Id";
        
        private const string FeedPostQuery =
@"select p.id as Id, p.""text"" as ""Text"", au.last_name || ' ' || au.first_name as UserName, p.created_at as ""CreatedAt""
from post p 
join friend f 
	ON p.user_id = f.friend_id 
join app_user au 
	ON p.user_id = au.id
where f.user_id = @UserId
order by p.Id
limit @Limit
offset @Offset;";

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PostRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<long> Create(Guid userId, string text, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var command = new CommandDefinition(InsertPostQuery, parameters: new { UserId=userId, Text=text, CreatedAt=now }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            return await connection.ExecuteScalarAsync<long>(command);
        }

        public async Task Delete(Guid userId, long postId, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(DeletePostQuery, parameters: new { UserId=userId, Id=postId }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            await connection.ExecuteScalarAsync(command);
        }


        public async Task<PostDto?> Get(long postId, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(SelectPostQuery, parameters: new { Id=postId }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            return await connection.QueryFirstOrDefaultAsync<PostDto>(command);
        }

        public async Task Update(Guid userId, long postId, string text, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(UpdatePostQuery, parameters: new { UserId=userId, Text=text, Id=postId }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            await connection.ExecuteScalarAsync(command);
        }

        public async Task<IReadOnlyList<PostDto>> Feed(Guid userId, int offset, int limit, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(FeedPostQuery, parameters: new { UserId=userId, Limit=limit, Offset=offset }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            return (await connection.QueryAsync<PostDto>(command)).ToList();
        }
    }
}
