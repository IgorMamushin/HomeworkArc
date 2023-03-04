using Dapper;
using Models;
using System.Data;
using System.Data.Common;

namespace DataAccess.Repositories.Impl
{
    internal class UserRepository : IUserRepository
    {
        private const string TableName = "app_user";
        private const string InsertQuery = $@"
INSERT INTO {TableName} (id, first_name, last_name, age, biography, city, password_hash) 
        VALUES (@Id, @FirstName, @LastName, @Age, @Biography, @City, crypt(@PasswordHash, gen_salt('md5')))";

        private const string SelectByIdQuery = $@"select 
t.first_name FirstName, t.last_name LastName, t.age, t.biography, t.city, t.password_hash Passwordhash from {TableName} t where t.id = @Id";
        private const string AuthQuery = $"select count(1) from {TableName} t where t.Id=@Id AND t.password_hash=crypt(@Password, password_hash)";

        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid?> Login(Guid id, string password, CancellationToken cancellationToken)
        {
            OpenConnectIfNeeded();
            var command = new CommandDefinition(AuthQuery, parameters: new { Id=id, Password=password }, cancellationToken: cancellationToken);
            var exists = await _dbConnection.ExecuteScalarAsync<bool>(command);
            if (exists)
            {
                return Guid.NewGuid();
            }

            return null;
        }

        public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
        {
            OpenConnectIfNeeded();
            var id = Guid.NewGuid();
            user.Id = id;

            var command = new CommandDefinition(InsertQuery, parameters: user, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteScalarAsync(command);
            return id;
        }

        public async Task<User?> GetUser(Guid id, CancellationToken cancellationToken)
        {
            OpenConnectIfNeeded();

            var command = new CommandDefinition(SelectByIdQuery, parameters: new { Id = id }, cancellationToken: cancellationToken);
            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(command);
            return user;
        }

        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        private void OpenConnectIfNeeded()
        {
            if (_dbConnection.State == ConnectionState.Open)
                return;

            _dbConnection.Open();
        }
    }
}
