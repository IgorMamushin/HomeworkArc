using Dapper;
using Models;

namespace DataAccess.Repositories.Impl
{
    internal class LoginRepository : ILoginRepository
    {
        private const string AuthQuery = 
$"select t.first_name FirstName, t.last_name LastName from app_user t where t.Id=@Id AND t.password_hash=crypt(@Password, password_hash)";
        
        private const string SaveTokenQuery = 
"insert into login_token (token, user_id, user_name) VALUES (@Token, @UserId, @UserName)";

        private const string GetUserInfo = 
"select l.user_id as UserId, l.user_name as UserName from login_token l where l.token = @Token";

        private readonly IDbConnectionFactory _dbConnectionFactory;

        public LoginRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<AuthUserInfo?> GetAuthUserInfo(Guid token, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(GetUserInfo, parameters: new { Token=token }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var user = await connection.QueryFirstOrDefaultAsync<AuthUserInfo>(command);
            return user;
        }

        public async Task<Guid?> Login(Guid id, string password, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(AuthQuery, parameters: new { Id=id, Password=password }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var user = await connection.QueryFirstAsync<User>(command);
            if (user != null)
            {
                var token = Guid.NewGuid();

                var parameter = new
                {
                    Token = token,
                    UserId = id,
                    UserName = $"{user.LastName} {user.FirstName}"
                };

                var saveTokenInfo = new CommandDefinition(SaveTokenQuery, parameters: parameter, cancellationToken: cancellationToken);
                await connection.ExecuteScalarAsync(saveTokenInfo);

                return token;
            }

            return null;            
        }
    }
}
