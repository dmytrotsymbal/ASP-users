//using ASP_users.Interfaces;
//using ASP_users.Models;
//using MySqlConnector;

//namespace ASP_users.Repositories
//{
//    public class AddressRepository : BaseRepository, IAddressRepository
//    {
//        public AddressRepository(MySqlConnection connection) : base(connection) { }


//        public async Task<IEnumerable<Address>> GetAllUsersAddresses(Guid userId)
//        {
//            var usersAddresses = new List<Address>();
//            var command = CreateCommand(
//                @"SELECT 
//                    AddressID,
//                    UserID,
//                    StreetAddress,
//                    HouseNumber,
//                    ApartmentNumber,
//                    City,
//                    State,
//                    PostalCode,
//                    Country,
//                    Latitude,
//                    Longitude
//                FROM 
//                    Addresses
//                WHERE 
//                    UserID = @UserID"
//            );
//            command.Parameters.AddWithValue("@UserID", userId);

//            _connection.Open();

//            var reader = await command.ExecuteReaderAsync();

//            while (await reader.ReadAsync())
//            {
//                var addressId = reader.GetInt32(0);
//                var address = usersAddresses.FirstOrDefault(x => x.AddressID == addressId);
//                if (address == null)
//                {
//                    address = new Address
//                    {
//                        AddressID = addressId,
//                        UserID = userId,
//                        StreetAddress = reader.GetString(2),
//                        HouseNumber = reader.GetInt32(3),
//                        ApartmentNumber = reader.GetInt32(4),
//                        City = reader.GetString(5),
//                        State = reader.GetString(6),
//                        PostalCode = reader.GetString(7),
//                        Country = reader.GetString(8),
//                        Latitude = reader.GetDecimal(9),
//                        Longitude = reader.GetDecimal(10),
//                    };
//                    usersAddresses.Add(address);
//                }
//            }
//            _connection.Close();
//            return usersAddresses;
//        }


//        public async Task<Address> GetUserAddressById(int addressId)
//        {
//            Address address = null;

//            var command = CreateCommand(
//                @"SELECT 
//                    AddressID,
//                    UserID,
//                    StreetAddress,
//                    HouseNumber,
//                    ApartmentNumber,
//                    City,
//                    State,
//                    PostalCode,
//                    Country,
//                    Latitude,
//                    Longitude
//                FROM 
//                    Addresses
//                WHERE 
//                    AddressID = @AddressID"
//            );
//            command.Parameters.AddWithValue("@AddressID", addressId);

//            _connection.Open();

//            var reader = await command.ExecuteReaderAsync();

//            if (await reader.ReadAsync())
//            {
//                address = new Address
//                {
//                    AddressID = reader.GetInt32(0),
//                    UserID = reader.GetGuid(1),
//                    StreetAddress = reader.GetString(2),
//                    HouseNumber = reader.GetInt32(3),
//                    ApartmentNumber = reader.GetInt32(4),
//                    City = reader.GetString(5),
//                    State = reader.GetString(6),
//                    PostalCode = reader.GetString(7),
//                    Country = reader.GetString(8),
//                    Latitude = reader.GetDecimal(9),
//                    Longitude = reader.GetDecimal(10),
//                };
//            }
//            _connection.Close();

//            return address;
//        }


//        public async Task UpdateAddress(int addressId, Address address)
//        {
//            var command = CreateCommand(
//                @"UPDATE 
//                    Addresses
//                SET
//                    StreetAddress = @StreetAddress,
//                    HouseNumber = @HouseNumber,
//                    ApartmentNumber = @ApartmentNumber,
//                    City = @City,
//                    State = @State,
//                    PostalCode = @PostalCode,
//                    Country = @Country,
//                    Latitude = @Latitude,
//                    Longitude = @Longitude
//                WHERE
//                    AddressID = @AddressID"
//            );
//            command.Parameters.AddWithValue("@AddressID", addressId);
//            command.Parameters.AddWithValue("@StreetAddress", address.StreetAddress);
//            command.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
//            command.Parameters.AddWithValue("@ApartmentNumber", address.ApartmentNumber);
//            command.Parameters.AddWithValue("@City", address.City);
//            command.Parameters.AddWithValue("@State", address.State);
//            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
//            command.Parameters.AddWithValue("@Country", address.Country);
//            command.Parameters.AddWithValue("@Latitude", address.Latitude);
//            command.Parameters.AddWithValue("@Longitude", address.Longitude);

//            _connection.Open();

//            await command.ExecuteNonQueryAsync();

//            _connection.Close();
//        }


//        public async Task AddAddressToUser(Guid userId, Address address)
//        {
//            var command = CreateCommand(
//                @"INSERT INTO Addresses 
//                    (UserID, StreetAddress, HouseNumber, ApartmentNumber, City, State, PostalCode, Country, Latitude, Longitude)
//                VALUES 
//                    (@UserID, @StreetAddress, @HouseNumber, @ApartmentNumber, @City, @State, @PostalCode, @Country, @Latitude, @Longitude)"
//            );
//            command.Parameters.AddWithValue("@UserID", userId);
//            command.Parameters.AddWithValue("@StreetAddress", address.StreetAddress);
//            command.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
//            command.Parameters.AddWithValue("@ApartmentNumber", address.ApartmentNumber);
//            command.Parameters.AddWithValue("@City", address.City);
//            command.Parameters.AddWithValue("@State", address.State);
//            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
//            command.Parameters.AddWithValue("@Country", address.Country);
//            command.Parameters.AddWithValue("@Latitude", address.Latitude);
//            command.Parameters.AddWithValue("@Longitude", address.Longitude);

//            _connection.Open();

//            await command.ExecuteNonQueryAsync();

//            _connection.Close();
//        }


//        public async Task DeleteAddress(int addressId)
//        {
//            var command = CreateCommand(
//                @"DELETE FROM Addresses 
//                WHERE AddressID = @AddressID"
//            );
//            command.Parameters.AddWithValue("@AddressID", addressId);

//            _connection.Open();

//            await command.ExecuteNonQueryAsync();

//            _connection.Close();
//        }
//    }
//}
