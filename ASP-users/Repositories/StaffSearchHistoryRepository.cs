using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class StaffSearchHistoryRepository : BaseRepository, IStaffSearchHistoryRepository
    {
        public StaffSearchHistoryRepository(MySqlConnection connection) : base(connection) { }


        public async Task<IEnumerable<StaffSearchHistory>> GetAllSearchHistory(int pageNumber = 1, int pageSize = 15)
        {
            var offset = (pageNumber - 1) * pageSize;

            var allHistory = new List<StaffSearchHistory>();

            var command = CreateCommand(
                @"SELECT 
                    staffsearchhistory.SearchID,
                    staffaccounts.Nickname,
                    staffaccounts.Email,
                    staffaccounts.Role, 
                    staffsearchhistory.SearchQuery, 
                    staffsearchhistory.SearchFilters, 
                    staffsearchhistory.SearchType, 
                    staffsearchhistory.SearchDate
                  FROM 
                    staffsearchhistory 
                  JOIN 
                    staffaccounts ON staffaccounts.StaffID = staffsearchhistory.StaffID
                  ORDER BY 
                    staffsearchhistory.SearchDate DESC
                  LIMIT @offset, @pageSize"
             );

            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@pageSize", pageSize);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                allHistory.Add(new StaffSearchHistory
                {
                    SearchID = reader.GetInt32(0),
                    Nickname = reader.GetString(1),
                    Email = reader.GetString(2),
                    Role = reader.GetString(3),
                    SearchQuery = reader.GetString(4),
                    SearchFilters = reader.IsDBNull(5) ? "{}" : reader.GetString(5),
                    SearchType = reader.GetString(6),
                    SearchDate = reader.GetDateTime(7)
                });
            }

            _connection.Close();
            return allHistory;
        }


        public async Task<IEnumerable<StaffSearchHistory>> CurrentStaffSearchHistory(int staffId, int pageNumber = 1, int pageSize = 15)
        {
            var offset = (pageNumber - 1) * pageSize;

            var history = new List<StaffSearchHistory>();

            var command = CreateCommand(
                @"SELECT 
                    staffsearchhistory.SearchID,
                    staffaccounts.Nickname,
                    staffaccounts.Email,
                    staffaccounts.Role, 
                    staffsearchhistory.SearchQuery, 
                    staffsearchhistory.SearchFilters, 
                    staffsearchhistory.SearchType, 
                    staffsearchhistory.SearchDate
                  FROM 
                    staffsearchhistory 
                  JOIN 
                    staffaccounts ON staffaccounts.StaffID = staffsearchhistory.StaffID
                  WHERE 
                    staffaccounts.StaffID = @StaffID
                  ORDER BY 
                    staffsearchhistory.SearchDate DESC
                  LIMIT @offset, @pageSize");

            command.Parameters.AddWithValue("@StaffID", staffId);
            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@pageSize", pageSize);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                history.Add(new StaffSearchHistory
                {
                    SearchID = reader.GetInt32(0),
                    Nickname = reader.GetString(1),
                    Email = reader.GetString(2),
                    Role = reader.GetString(3),
                    SearchQuery = reader.GetString(4),
                    SearchFilters = reader.IsDBNull(5) ? "{}" : reader.GetString(5),
                    SearchType = reader.GetString(6),
                    SearchDate = reader.GetDateTime(7)
                });
            }

            _connection.Close();
            return history;
        }



        public async Task AddSearchHistory(int staffId, string searchQuery, string searchFilters, string searchType)
        {
            var command = CreateCommand(
                @"INSERT INTO StaffSearchHistory (
                    StaffID, 
                    SearchQuery, 
                    SearchFilters, 
                    SearchType)
                  VALUES (
                    @StaffID, 
                    @SearchQuery, 
                    @SearchFilters, 
                    @SearchType)"
                );

            command.Parameters.AddWithValue("@StaffID", staffId);
            command.Parameters.AddWithValue("@SearchQuery", searchQuery ?? "");
            command.Parameters.AddWithValue("@SearchFilters", searchFilters ?? "{}");
            command.Parameters.AddWithValue("@SearchType", searchType ?? "");

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }




        // HALPERS
        public async Task<int> AllSearchHistoryQantity()
        {
            var command = CreateCommand(@"SELECT COUNT(*) FROM StaffSearchHistory");

            _connection.Open();

            var quantity = Convert.ToInt32(await command.ExecuteScalarAsync());

            _connection.Close();

            return quantity;
        }


        public async Task<int> CurrentStaffSearchHistoryQantity(int staffId)
        {
            var command = CreateCommand(@"SELECT COUNT(*) FROM StaffSearchHistory WHERE StaffID = @StaffID");

            _connection.Open();

            command.Parameters.AddWithValue("@StaffID", staffId);

            var quantity = Convert.ToInt32(await command.ExecuteScalarAsync());

            _connection.Close();

            return quantity;
        }
    }
}
