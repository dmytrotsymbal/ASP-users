using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class UserAddressRepository : BaseRepository, IUserAddressRepository
    {
        public UserAddressRepository(MySqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<UserAddressDetails>> GetUserAddresses(Guid userId)
        {
            var userAddressDetails = new List<UserAddressDetails>();

            var command = CreateCommand(
                @"SELECT addresses.AddressID,
                 useraddresses.UserID,
                 addresses.StreetAddress,
                 addresses.HouseNumber,  -- Здесь добавил HouseNumber
                 addresses.ApartmentNumber,
                 addresses.City,
                 addresses.State,
                 addresses.PostalCode,
                 addresses.Country,
                 addresses.Latitude,
                 addresses.Longitude,
                 useraddresses.MoveInDate,
                 useraddresses.MoveOutDate
         FROM 
	             Addresses addresses INNER JOIN UserAddresses useraddresses ON addresses.AddressID = useraddresses.AddressID
         WHERE 
             useraddresses.UserID = @UserID");

            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                userAddressDetails.Add(new UserAddressDetails
                {
                    AddressID = reader.GetInt32(0),
                    UserID = reader.GetGuid(1),
                    StreetAddress = reader.GetString(2),
                    HouseNumber = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3), // Чтение HouseNumber
                    ApartmentNumber = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                    City = reader.GetString(5),
                    State = reader.GetString(6),
                    PostalCode = reader.GetString(7),
                    Country = reader.GetString(8),
                    Latitude = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9),
                    Longitude = reader.IsDBNull(10) ? (decimal?)null : reader.GetDecimal(10),
                    MoveInDate = reader.GetDateTime(11),
                    MoveOutDate = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12)
                });
            }

            _connection.Close();

            return userAddressDetails;
        }


        public async Task AddUserAddress(UserAddress userAddress)
        {
            var command = CreateCommand(
                @"INSERT INTO UserAddresses (UserID, AddressID, MoveInDate, MoveOutDate)
              VALUES (@UserID, @AddressID, @MoveInDate, @MoveOutDate)");
            command.Parameters.AddWithValue("@UserID", userAddress.UserID);
            command.Parameters.AddWithValue("@AddressID", userAddress.AddressID);
            command.Parameters.AddWithValue("@MoveInDate", userAddress.MoveInDate);
            command.Parameters.AddWithValue("@MoveOutDate", userAddress.MoveOutDate);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }

        public async Task RemoveUserAddress(int userAddressID)
        {
            var command = CreateCommand(
                @"DELETE FROM UserAddresses WHERE UserAddressID = @UserAddressID");
            command.Parameters.AddWithValue("@UserAddressID", userAddressID);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }
    }

}
