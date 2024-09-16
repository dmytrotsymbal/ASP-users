using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Models.Helpers;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class UserAddressRepository : BaseRepository, IAddressRepository
    {
        public UserAddressRepository(MySqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<Address>> GetUserAddresses(Guid userId)
        {
            var userAddressDetails = new List<Address>();

            var command = CreateCommand(
                @"SELECT addresses.AddressID,
                         useraddresses.UserID,
                         addresses.StreetAddress,
                         addresses.HouseNumber,
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
                         useraddresses.UserID = @UserID
                 ORDER BY
                         useraddresses.MoveInDate DESC");

            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                userAddressDetails.Add(new Address
                {
                    AddressID = reader.GetInt32(0),
                    UserID = reader.GetGuid(1),
                    StreetAddress = reader.GetString(2),
                    HouseNumber = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
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



        public async Task<Address> GetUserAddressById(int addressId)
        {
            Address detailedAddress = null;

            var command = CreateCommand(
                @"SELECT 
                    addresses.AddressID,
                    useraddresses.UserID,
                    addresses.StreetAddress,
                    addresses.HouseNumber,
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
                    useraddresses.AddressID = @AddressID"
            );
            command.Parameters.AddWithValue("@AddressID", addressId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                detailedAddress = new Address
                {
                    AddressID = reader.GetInt32(0),
                    UserID = reader.GetGuid(1),
                    StreetAddress = reader.GetString(2),
                    HouseNumber = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                    ApartmentNumber = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                    City = reader.GetString(5),
                    State = reader.GetString(6),
                    PostalCode = reader.GetString(7),
                    Country = reader.GetString(8),
                    Latitude = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9),
                    Longitude = reader.IsDBNull(10) ? (decimal?)null : reader.GetDecimal(10),
                    MoveInDate = reader.GetDateTime(11),
                    MoveOutDate = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12)
                };
            }
            _connection.Close();

            return detailedAddress;
        }



        public async Task<IEnumerable<Resident>> GetAddressLivingHistory(int addressId)
        {
            var addressLivingHistory = new List<Resident>();

            var command = CreateCommand(
                @"SELECT 
	                u.UserID, 
	                u.FirstName, 
	                u.LastName, 
	                u.Email,
	                ua.MoveInDate,
	                ua.MoveOutDate
                FROM 
	                users u JOIN UserAddresses ua ON u.UserID = ua.UserID
	                JOIN Addresses a ON ua.AddressID = a.AddressID
                WHERE 
	                a.AddressID = @AddressID
                ORDER BY 
	                ua.MoveInDate DESC;");

            command.Parameters.AddWithValue("@AddressID", addressId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                addressLivingHistory.Add(new Resident
                {
                    UserID = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    MoveInDate = reader.GetDateTime(4),
                    MoveOutDate = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
                });
            }

            _connection.Close(); 
            
            return addressLivingHistory;
        }
    }
}
