using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public PhotoRepository(MySqlConnection connection) : base(connection) { }

        public async Task AddPhoto(Guid userId, Photo photo)
        {
            var command = new MySqlCommand(
                @"INSERT INTO Photos 
                    (UserID, ImageURL, AltText, UploadedAt) 
                VALUES 
                    (@UserID, @ImageURL, @AltText, @UploadedAt)", _connection);

            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@ImageURL", photo.ImageURL);
            command.Parameters.AddWithValue("@AltText", photo.AltText);
            command.Parameters.AddWithValue("@UploadedAt", photo.UploadedAt);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }

        public async Task DeletePhoto(int imageID)
        {
            var command = CreateCommand(
                @"DELETE FROM Photos 
                WHERE ImageID = @ImageID"
            );

            command.Parameters.AddWithValue("@ImageID", imageID);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
    }
}