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

            while( await reader.ReadAsync())
            {
                var addressId = reader.GetInt32(0);
                var address = usersAddresses.FirstOrDefault(x => x.AddressID == addressId);
                if(address == null)
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
    }
}
