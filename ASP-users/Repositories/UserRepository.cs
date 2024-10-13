using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(MySqlConnection connection) : base(connection) { }



        public async Task<IEnumerable<User>> GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {

            var offset = (pageNumber - 1) * pageSize;

            var usersList = new List<User>(); // Створюємо список для зберігання користувачів

            var command = CreateCommand(
                @"SELECT 
                    users.UserID, 
                    users.FirstName, 
                    users.LastName, 
                    users.Email, 
                    users.DateOfBirth, 
                    users.CreatedAt, 
                    photos.ImageID,
                    photos.ImageURL, 
                    photos.AltText, 
                    photos.UploadedAt 
                 FROM 
                    users LEFT JOIN photos ON users.UserID = photos.UserID 
                 GROUP BY 
                    users.UserID
                 ORDER BY 
                    users.UserID
                 LIMIT @offset, @pageSize"
            );


            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@pageSize", pageSize);

            _connection.Open(); // Відкриваємо з'єднання з базою даних

            using var reader = await command.ExecuteReaderAsync(); // Виконуємо команду та отримуємо читач даних (DataReader)

            while (await reader.ReadAsync()) // Читаємо дані з результуючого набору
            {
                var userId = reader.GetGuid(0); // Отримуємо UserID з першої колонки

                var user = usersList.FirstOrDefault(u => u.UserID == userId);  // FirstOrDefault для перевірки, чи існує користувач з певним UserID у списку.

                if (user == null) // Якщо користувача не знайдено
                {
                    user = new User // Створюємо нового користувача
                    {
                        UserID = userId, // Встановлюємо UserID
                        FirstName = reader.GetString(1), // Встановлюємо FirstName
                        LastName = reader.GetString(2), // Встановлюємо LastName
                        Email = reader.GetString(3), // Встановлюємо Email
                        DateOfBirth = reader.GetDateTime(4), // Встановлюємо DateOfBirth
                        CreatedAt = reader.GetDateTime(5), // Встановлюємо CreatedAt
                        Photos = new List<Photo>() // Ініціалізуємо список Photos
                    };
                    usersList.Add(user); // Додаємо тільки що створеного користувача до списку
                }

                if (!reader.IsDBNull(6)) // Якщо у користувача є фото
                {
                    user.Photos.Add(new Photo // Додаємо фото до списку Photos користувача
                    {
                        ImageID = reader.GetInt32(6), // Встановлюємо ImageID
                        UserID = userId, // Встановлюємо UserID
                        ImageURL = reader.GetString(7), // Встановлюємо ImageURL
                        AltText = reader.GetString(8), // Встановлюємо AltText
                        UploadedAt = reader.GetDateTime(9) // Встановлюємо UploadedAt
                    });
                }
            }

            _connection.Close(); // Закриваємо з'єднання з базою даних

            return usersList; // Повертаємо список користувачів з фото
        }


        public async Task<User> GetUserById(Guid userId)
        {
            User user = null;  // Створюємо змінну для зберігання користувача

            var command = CreateCommand(   // Створюємо SQL-запит для отримання користувача та його фото
                @"SELECT 
                    users.UserID, 
                    users.FirstName, 
                    users.LastName, 
                    users.Email, 
                    users.DateOfBirth, 
                    users.CreatedAt, 
                    photos.ImageID, 
                    photos.ImageURL, 
                    photos.AltText, 
                    photos.UploadedAt 
                 FROM 
                    users LEFT JOIN photos ON users.UserID = photos.UserID 
                 WHERE users.UserID = @UserID"
            );

            command.Parameters.AddWithValue("@UserID", userId); // Додаємо параметр @UserID

            _connection.Open(); // Відкриваємо з'єднання з базою даних

            using var reader = await command.ExecuteReaderAsync(); // Виконуємо команду та отримуємо читач даних (DataReader)

            while (await reader.ReadAsync()) // читаэмо reader
            {
                if (user == null) // Якщо користувача ще не створено
                {
                    user = new User // Створюємо нового користувача
                    {
                        UserID = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        DateOfBirth = reader.GetDateTime(4),
                        CreatedAt = reader.GetDateTime(5),
                        Photos = new List<Photo>() // Ініціалізуємо список Photos
                    };
                }

                if (!reader.IsDBNull(6)) // Якщо у користувача є фото
                {
                    user.Photos.Add(new Photo // Додаємо фото до списку Photos користувача який ініціалізували попередньо
                    {
                        ImageID = reader.GetInt32(6),
                        UserID = reader.GetGuid(0),
                        ImageURL = reader.GetString(7),
                        AltText = reader.GetString(8),
                        UploadedAt = reader.GetDateTime(9)
                    });
                }
            }

            _connection.Close(); // Закриваємо з'єднання з базою даних
            return user; // Повертаємо користувача з фото
        }


        public async Task<IEnumerable<User>> SearchUsers(string searchQuery)
        {
            var searchedUsers = new List<User>(); // Створюємо список для зберігання користувачів

            var command = CreateCommand( // Створюємо SQL-запит для пошуку користувачів за ім'ям та прізвищем
                @"SELECT 
                    users.UserID, 
                    users.FirstName, 
                    users.LastName, 
                    users.Email, 
                    users.DateOfBirth, 
                    users.CreatedAt, 
                    photos.ImageID, 
                    photos.ImageURL, 
                    photos.AltText, 
                    photos.UploadedAt 
                 FROM 
                    users LEFT JOIN photos ON users.UserID = photos.UserID 
                 WHERE 
                    users.FirstName LIKE @SearchQuery OR users.LastName LIKE @SearchQuery OR users.UserID LIKE @SearchQuery
                 ORDER BY 
                    users.UserID"
            );

            command.Parameters.AddWithValue("@SearchQuery", $"%{searchQuery}%"); // Додаємо параметр @SearchQuery

            _connection.Open(); // Відкриваємо з'єднання з базою даних

            using var reader = await command.ExecuteReaderAsync(); // Виконуємо команду та отримуємо читач даних (DataReader)

            while (await reader.ReadAsync()) // Читаємо дані з результуючого набору
            {
                var userId = reader.GetGuid(0); // Отримуємо UserID з першої колонки

                var user = searchedUsers.FirstOrDefault(u => u.UserID == userId);  // FirstOrDefault для перевірки, чи існує користувач з певним UserID у списку.

                if (user == null) // Якщо користувача не знайдено
                {
                    user = new User // Створюємо нового користувача
                    {
                        UserID = userId, // Встановлюємо UserID
                        FirstName = reader.GetString(1), // Встановлю
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        DateOfBirth = reader.GetDateTime(4),
                        CreatedAt = reader.GetDateTime(5),
                        Photos = new List<Photo>() // Ініціалізуємо список Photos
                    };

                    searchedUsers.Add(user); // Додаємо тільки що створеного користувача до списку
                }

                if (!reader.IsDBNull(6)) // Якщо у користувача є фото
                {
                    user.Photos.Add(new Photo // Додаємо фото до списку Photos користувача який ініціалізували попередньо
                    {
                        ImageID = reader.GetInt32(6),
                        UserID = reader.GetGuid(0),
                        ImageURL = reader.GetString(7),
                        AltText = reader.GetString(8),
                        UploadedAt = reader.GetDateTime(9)
                    });
                }
            }
            _connection.Close(); // Закриваємо з'єднання з базою даних
            return searchedUsers; // Повертаємо список користувачів з фото
        }


        public async Task CreateUser(User user)
        {
            var userId = Guid.NewGuid();

            var createdAt = DateTime.Now;

            var command = CreateCommand(
                @"INSERT INTO Users 
                    (UserID, FirstName, LastName, Email, DateOfBirth, CreatedAt) 
                VALUES 
                    (@UserID, @FirstName, @LastName, @Email, @DateOfBirth, @CreatedAt)"
            );

            command.Parameters.AddWithValue("@UserID", user.UserID);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
            command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }


        public async Task UpdateUser(Guid userId, User user)
        {
            var command = new MySqlCommand(   // Створюємо SQL-запит для оновлення даних користувача
                @"UPDATE Users  
                SET 
                    FirstName = @FirstName, 
                    LastName = @LastName, 
                    Email = @Email, 
                    DateOfBirth = @DateOfBirth 
                WHERE 
                    UserID = @UserID", _connection);

            command.Parameters.AddWithValue("@UserID", userId);   // Додаємо параметри до команди
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

            _connection.Open(); // Відкриваємо з'єднання з базою даних

            await command.ExecuteNonQueryAsync(); // Виконуємо команду для оновлення даних користувача

            if (user.Photos != null && user.Photos.Count > 0) // якщо у юзера є фото 
            {
                foreach (var photo in user.Photos) // для кожного фото
                {
                    var updatePhotoCommand = new MySqlCommand( // стоврюємо SQL-запит для оновлення фото
                        @"UPDATE Photos 
                        SET 
                            ImageURL = @ImageURL, 
                            AltText = @AltText, 
                            UploadedAt = @UploadedAt 
                        WHERE 
                            ImageID = @ImageID", _connection);

                    updatePhotoCommand.Parameters.AddWithValue("@ImageID", photo.ImageID); // де також додаємо параметри
                    updatePhotoCommand.Parameters.AddWithValue("@ImageURL", photo.ImageURL);
                    updatePhotoCommand.Parameters.AddWithValue("@AltText", photo.AltText);
                    updatePhotoCommand.Parameters.AddWithValue("@UploadedAt", photo.UploadedAt);

                    await updatePhotoCommand.ExecuteNonQueryAsync(); // виконуємо команду для оновлення фото
                }
            }
            _connection.Close(); // закриваємо з'єднання з базою даних
        }


        public async Task DeleteUser(Guid userId)
        {
            var command = CreateCommand(
                @"DELETE FROM Users 
                WHERE UserID = @UserID");

            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }



        // HELPERS

        public async Task<User> GetUserByEmail(string email)
        {
            User user = null;

            var command = CreateCommand(
                @"SELECT 
                     UserID, FirstName, LastName, Email, DateOfBirth, CreatedAt 
                FROM Users 
                WHERE 
                     Email = @Email"
            );

            command.Parameters.AddWithValue("@Email", email);

            _connection.Open();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                user = new User
                {
                    UserID = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    DateOfBirth = reader.GetDateTime(4),
                    CreatedAt = reader.GetDateTime(5)
                };
            }
            _connection.Close();

            return user;
        }


        public async Task<int> GetUsersCount()
        {
            var command = CreateCommand(@"SELECT COUNT(*) FROM Users");

            _connection.Open();

            var count = (int)(long)await command.ExecuteScalarAsync();

            _connection.Close();

            return count;
        }


        public async Task<IEnumerable<Guid>> GetAllUsersIDs()
        {
            var idsList = new List<Guid>();

            var command = CreateCommand(
                @"SELECT UserID FROM Users"
            );

            _connection.Open();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                idsList.Add(reader.GetGuid(0));
            }

            _connection.Close();

            return idsList;
        }
    }
}