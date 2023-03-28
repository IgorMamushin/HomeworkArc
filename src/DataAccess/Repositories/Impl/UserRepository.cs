using Dapper;
using Models;

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
        private const string SearchQuery = $"select t.id, t.first_name FirstName, t.last_name LastName, t.age, t.biography, t.city from {TableName} t where t.last_name ilike @ln AND t.first_name ilike @fn";


        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Guid?> Login(Guid id, string password, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(AuthQuery, parameters: new { Id=id, Password=password }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            var exists = await connection.ExecuteScalarAsync<bool>(command);
            if (exists)
            {
                return Guid.NewGuid();
            }

            return null;            
        }

        public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            user.Id = id;
            var command = new CommandDefinition(InsertQuery, parameters: user, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();
            await connection.ExecuteScalarAsync(command);

            return id;
        }

        public async Task<User?> GetUser(Guid id, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(SelectByIdQuery, parameters: new { Id = id }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();

            var user = await connection.QuerySingleOrDefaultAsync<User>(command);
            return user;
        }

        public async Task<IReadOnlyList<UserDto>> Search(string lastName, string firstName, CancellationToken cancellationToken)
        {
            var command = new CommandDefinition(SearchQuery, parameters: new { ln = lastName + "%", fn = firstName + "%" }, cancellationToken: cancellationToken);

            using var connection = _dbConnectionFactory.CreateConnection();
            connection.Open();

            var users = await connection.QueryAsync<UserDto>(command);
            return users.ToList();
        }
    }
}
