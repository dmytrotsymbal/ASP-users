﻿using ASP_users.Interfaces;
using ASP_users.Models;
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



        public async Task<Address> GetUserAddressById(Guid? userId, int addressId)
        {
            Address detailedAddress = null;

            // запит якщо БЕЗ userId
            string query = @"    
                SELECT 
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
                    Addresses addresses 
                INNER JOIN 
                    UserAddresses useraddresses 
                ON 
                    addresses.AddressID = useraddresses.AddressID
                WHERE 
                    useraddresses.AddressID = @AddressID";

            // додається рядок якщо UserID є заданим
            if (userId.HasValue)
            {
                query += " AND useraddresses.UserID = @UserID";
            }

            var command = CreateCommand(query); // формуємо з попереднього запиту ПОВНОЦІННУ команду

            command.Parameters.AddWithValue("@AddressID", addressId); // описуємо параметр який приходить завжди

            if (userId.HasValue)
            {
                command.Parameters.AddWithValue("@UserID", userId.Value); // описуємо параметр який приходить НЕ завжди
            }

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
		            users.UserID, 
		            users.FirstName, 
		            users.LastName,
		            users.Email, 
		            useraddresses.MoveInDate, 
		            useraddresses.MoveOutDate 
                 FROM 
		            users INNER JOIN useraddresses ON users.UserID = useraddresses.UserID 
		            JOIN addresses ON useraddresses.AddressID = addresses.AddressID
                 WHERE 
		            addresses.AddressID = @AddressID
                 ORDER BY
		            useraddresses.MoveInDate DESC;");

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



        public async Task UpdateAddress(int addressId, Address address)
        {
            var updateAddressCommand = CreateCommand(
                @"UPDATE 
                    Addresses
                  SET
                    StreetAddress = @StreetAddress,
                    HouseNumber = @HouseNumber,
                    ApartmentNumber = @ApartmentNumber,
                    City = @City,
                    State = @State,
                    PostalCode = @PostalCode,
                    Country = @Country,
                    Latitude = @Latitude,
                    Longitude = @Longitude
                  WHERE
                    AddressID = @AddressID");

            updateAddressCommand.Parameters.AddWithValue("@AddressID", addressId);
            updateAddressCommand.Parameters.AddWithValue("@StreetAddress", address.StreetAddress);
            updateAddressCommand.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
            updateAddressCommand.Parameters.AddWithValue("@ApartmentNumber", address.ApartmentNumber);
            updateAddressCommand.Parameters.AddWithValue("@City", address.City);
            updateAddressCommand.Parameters.AddWithValue("@State", address.State);
            updateAddressCommand.Parameters.AddWithValue("@PostalCode", address.PostalCode);
            updateAddressCommand.Parameters.AddWithValue("@Country", address.Country);
            updateAddressCommand.Parameters.AddWithValue("@Latitude", address.Latitude);
            updateAddressCommand.Parameters.AddWithValue("@Longitude", address.Longitude);

            _connection.Open();

            await updateAddressCommand.ExecuteNonQueryAsync();

            // Оновлюємо дані в таблиці UserAddresses (MoveInDate, MoveOutDate)
            var updateUserAddressCommand = CreateCommand(
                @"UPDATE 
                    useraddresses 
                  SET
                    MoveInDate = @MoveInDate,
                    MoveOutDate = @MoveOutDate
                  WHERE
                    AddressID = @AddressID AND UserID = @UserID");

            updateUserAddressCommand.Parameters.AddWithValue("@AddressID", addressId);
            updateUserAddressCommand.Parameters.AddWithValue("@UserID", address.UserID);
            updateUserAddressCommand.Parameters.AddWithValue("@MoveInDate", address.MoveInDate);
            updateUserAddressCommand.Parameters.AddWithValue("@MoveOutDate", address.MoveOutDate ?? (object)DBNull.Value);

            await updateUserAddressCommand.ExecuteNonQueryAsync();

            _connection.Close();
        }



        public async Task AddAddressToUser(Guid userId, Address address)
        {
            var command = CreateCommand(
                @"INSERT INTO Addresses (
                    StreetAddress, 
                    HouseNumber, 
                    ApartmentNumber, 
                    City, 
                    State, 
                    PostalCode, 
                    Country, 
                    Latitude, 
                    Longitude)
                VALUES (
                    @StreetAddress, 
                    @HouseNumber, 
                    @ApartmentNumber, 
                    @City, 
                    @State,
                    @PostalCode, 
                    @Country, 
                    @Latitude, 
                    @Longitude);
                SELECT LAST_INSERT_ID();"); // ІД щойно доданої адреси

            command.Parameters.AddWithValue("@StreetAddress", address.StreetAddress);
            command.Parameters.AddWithValue("@HouseNumber", address.HouseNumber);
            command.Parameters.AddWithValue("@ApartmentNumber", address.ApartmentNumber);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@State", address.State);
            command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
            command.Parameters.AddWithValue("@Country", address.Country);
            command.Parameters.AddWithValue("@Latitude", address.Latitude);
            command.Parameters.AddWithValue("@Longitude", address.Longitude);

            _connection.Open();

            // Виконуємо першу команду і отримуємо ID доданої адреси
            var addressId = Convert.ToInt32(await command.ExecuteScalarAsync());

            // Додаємо запис у таблицю UserAddresses
            var secondCommand = CreateCommand(
                @"INSERT INTO UserAddresses (
                    UserID,
                    AddressID,
                    MoveInDate,
                    MoveOutDate)
                VALUES (
                    @UserID,
                    @AddressID,
                    @MoveInDate, 
                    @MoveOutDate)");

            secondCommand.Parameters.AddWithValue("@UserID", userId);
            secondCommand.Parameters.AddWithValue("@AddressID", addressId);
            secondCommand.Parameters.AddWithValue("@MoveInDate", address.MoveInDate);
            secondCommand.Parameters.AddWithValue("@MoveOutDate", address.MoveOutDate ?? (object)DBNull.Value);

            await secondCommand.ExecuteNonQueryAsync();

            _connection.Close();
        }



        public async Task RemoveAddressFromUser(Guid userId, int addressId)
        {
            var command = CreateCommand(
            @"DELETE FROM 
                UserAddresses
              WHERE     
                UserID = @UserID AND AddressID = @AddressID");

            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@AddressID", addressId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }



        public async Task AddExistingUserToLivingHistory(Guid userId, int addressId, DateTime moveInDate, DateTime? moveOutDate)
        {
            var command = CreateCommand(
                @"INSERT INTO UserAddresses (
                    UserID,
                    AddressID,
                    MoveInDate,
                    MoveOutDate)
                VALUES (
                    @UserID,
                    @AddressID,
                    @MoveInDate, 
                    @MoveOutDate)");

            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@AddressID", addressId);
            command.Parameters.AddWithValue("@MoveInDate", moveInDate);
            command.Parameters.AddWithValue("@MoveOutDate", moveOutDate ?? (object)DBNull.Value);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }


        public async Task TotalDeleteWholeAddress(int addressId)
        {
            var command = CreateCommand(
                @"DELETE FROM 
                    Addresses
                  WHERE 
                    AddressID = @AddressID");

            command.Parameters.AddWithValue("@AddressID", addressId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            // Також видаляємо всі записи з таблиці UserAddresses, що відносяться до цієї адреси
            var command2 = CreateCommand(
                @"DELETE FROM 
                    UserAddresses
                  WHERE 
                    AddressID = @AddressID");

            command2.Parameters.AddWithValue("@AddressID", addressId);

            await command2.ExecuteNonQueryAsync();

            _connection.Close();
        }
    }
}