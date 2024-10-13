using MySqlConnector;

namespace ASP_users.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly MySqlConnection _connection;

        // Конструктор базового репозиторію приймає об'єкт MySqlConnection
        public BaseRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        // Метод для створення команди SQL з параметрами ( використовується в кожному репозиторію для створення запиту до БД )
        protected MySqlCommand CreateCommand(string query, params MySqlParameter[] parameters)
        {
            var command = new MySqlCommand(query, _connection);
            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command;
        }
    }
}
