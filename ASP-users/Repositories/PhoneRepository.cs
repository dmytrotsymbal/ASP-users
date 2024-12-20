﻿using ASP_users.Interfaces;
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


        public async Task<Phone> GetPhoneById(int phoneId)
        {
            Phone phone = null;

            var command = CreateCommand(
                @"SELECT 
                    PhoneID,
                    UserID,
                    PhoneNumber
                FROM 
                    Phones
                WHERE 
                    PhoneID = @PhoneID"
            );

            command.Parameters.AddWithValue("@PhoneID", phoneId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                phone = new Phone
                {
                    PhoneID = reader.GetInt32(0),
                    UserID = reader.GetGuid(1),
                    PhoneNumber = reader.GetString(2),
                };
            }

            _connection.Close();

            return phone;
        }


        public async Task UpdatePhone(int phoneId, Phone phone)
        {
            var command = CreateCommand(
                @"UPDATE 
                    Phones
                  SET 
                    PhoneNumber = @PhoneNumber
                  WHERE 
                    PhoneID = @PhoneID"
            );

            command.Parameters.AddWithValue("@PhoneNumber", phone.PhoneNumber);
            command.Parameters.AddWithValue("@PhoneID", phoneId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }


        public async Task AddPhoneToUser(Guid userId, Phone phone)
        {
            var command = CreateCommand(
                @"INSERT INTO 
                    Phones (UserID, PhoneNumber)
                  VALUES 
                    (@UserID, @PhoneNumber)"
            );

            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@PhoneNumber", phone.PhoneNumber);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }


        public async Task DeletePhone(int phoneId)
        {
            var command = CreateCommand(
                @"DELETE FROM 
                    Phones
                  WHERE 
                    PhoneID = @PhoneID"
            );

            command.Parameters.AddWithValue("@PhoneID", phoneId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }
    }
}
