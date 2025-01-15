using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class StaffRepository : BaseRepository, IStaffRepository
    {
        public StaffRepository(MySqlConnection connection) : base(connection) { }



        public async Task<StaffAccount?> GetStaffAccountByEmail(string email)
        {
            StaffAccount? staffAccount = null;

            var command = CreateCommand(
                @"SELECT 
                    StaffID,
                    Nickname,
                    Email,
                    PasswordHash,
                    Role,
                    CreatedAt
                FROM 
                    staffaccounts
                WHERE 
                    Email = @Email"
            );

            command.Parameters.AddWithValue("@Email", email);

            _connection.Open();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                staffAccount = new StaffAccount
                {
                    StaffID = reader.GetInt32(0),
                    Nickname = reader.GetString(1),
                    Email = reader.GetString(2),
                    PasswordHash = reader.GetString(3),
                    Role = Enum.Parse<RoleEnum>(reader.GetString(4)),
                    CreatedAt = reader.GetDateTime(5)
                };
            }

            _connection.Close();

            return staffAccount;
        }



        public async Task<IEnumerable<StaffSearchSummary>> GetTopSearchers()
        {
            var topSearchers = new List<StaffSearchSummary>();

            var command = CreateCommand(
                @"SELECT 
                    staffaccounts.StaffID,
                    staffaccounts.Nickname,
                    staffaccounts.Role,
                    COUNT(staffsearchhistory.SearchID) AS SearchCount
                 FROM 
                    staffaccounts LEFT JOIN staffsearchhistory ON staffaccounts.StaffID = staffsearchhistory.StaffID
                 GROUP BY 
                    staffaccounts.StaffID, staffaccounts.Nickname, staffaccounts.Email, staffaccounts.Role
                 ORDER BY 
                    SearchCount DESC");

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                topSearchers.Add(new StaffSearchSummary
                {
                    StaffID = reader.GetInt32(0),
                    Nickname = reader.GetString(1),
                    Role = (RoleEnum)Enum.Parse(typeof(RoleEnum), reader.GetString(2)),
                    SearchCount = reader.GetInt32(3)
                });
            }
            _connection.Close();

            return topSearchers;
        }

    }
}
