using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class PhoneRepository : BaseRepository, IPhoneRepository
    {

        public PhoneRepository(MySqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<Phone>> GetAllUsersPhones(Guid userId)
        {
            var usersPhones = new List<Phone>();
            var command = CreateCommand(
                @"SELECT 
                    PhoneID,
                    UserID,
                    PhoneNumber
                FROM 
                    Phones
                WHERE 
                    UserID = @UserID"
            );
            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var phoneId = reader.GetInt32(0);
                var phone = usersPhones.FirstOrDefault(x => x.PhoneID == phoneId);
                if (phone == null)
                {
                    phone = new Phone
                    {
                        PhoneID = phoneId,
                        UserID = userId,
                        PhoneNumber = reader.GetString(2),
                    };
                    usersPhones.Add(phone);
                }
            }
            _connection.Close();
            return usersPhones;
        }
    }
}
