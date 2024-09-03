using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;


namespace ASP_users.Repositories
{
    public class AddressRepository : BaseRepository, IAddressRepository
    {
        public AddressRepository(MySqlConnection connection) : base(connection) { }


        public async Task<IEnumerable<Address>> GetAllUsersAddresses(Guid userId)
        {
            var usersAddresses = new List<Address>();
            var command = CreateCommand(
                @"SELECT 
                    AddressID,
                    UserID,
                    StreetAddress,
                    City,
                    State,
                    PostalCode,
                    Country
                FROM 
                    Addresses
                WHERE 
                    UserID = @UserID"
            );
            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var addressId = reader.GetInt32(0);
                var address = usersAddresses.FirstOrDefault(x => x.AddressID == addressId);
                if (address == null)
                {
                    address = new Address
                    {
                        AddressID = addressId,
                        UserID = userId,
                        StreetAddress = reader.GetString(2),
                        City = reader.GetString(3),
                        State = reader.GetString(4),
                        PostalCode = reader.GetString(5),
                        Country = reader.GetString(6),
                    };
                    usersAddresses.Add(address);
                }
            }
            _connection.Close();
            return usersAddresses;
        }


        public async Task<Address> GetUserAddressById(int addressId)
        {
            Address address = null;

            var command = CreateCommand(
                @"SELECT 
                    AddressID,
                    UserID,
                    StreetAddress,
                    City,
                    State,
                    PostalCode,
                    Country
                FROM 
                    Addresses
                WHERE 
                    AddressID = @AddressID"
            );
            command.Parameters.AddWithValue("@AddressID", addressId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                address = new Address
                {
                    AddressID = reader.GetInt32(0),
                    UserID = reader.GetGuid(1),
                    StreetAddress = reader.GetString(2),
                    City = reader.GetString(3),
                    State = reader.GetString(4),
                    PostalCode = reader.GetString(5),
                    Country = reader.GetString(6),
                };
            }
            _connection.Close();

            return address;
        }


        public async Task UpdateAddress(int addressId, Address address)
        {
            var command = CreateCommand(
                @"UPDATE 
                    Addresses
                SET
                    StreetAddress = @StreetAddress,
                    City = @City,
                    State = @State,
                    PostalCode = @PostalCode,
                    Country = @Country
                WHERE
                    AddressID = @AddressID"
            );
            command.Parameters.AddWithValue("@AddressID", addressId);
            command.Parameters.AddWithValue("@StreetAddress", address.StreetAddress);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@State", address.State);
            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
            command.Parameters.AddWithValue("@Country", address.Country);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }


        public async Task AddAddressToUser(Guid userId, Address address)
        {
            var command = CreateCommand(
                @"INSERT INTO Addresses 
                    (UserID, StreetAddress, City, State, PostalCode, Country) 
                VALUES 
                    (@UserID, @StreetAddress, @City, @State, @PostalCode, @Country)"
            );
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@StreetAddress", address.StreetAddress);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@State", address.State);
            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
            command.Parameters.AddWithValue("@Country", address.Country);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }


        public async Task DeleteAddress(int addressId)
        {
            var command = CreateCommand(
                @"DELETE FROM Addresses 
                WHERE AddressID = @AddressID"
            );
            command.Parameters.AddWithValue("@AddressID", addressId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }
    }
}
