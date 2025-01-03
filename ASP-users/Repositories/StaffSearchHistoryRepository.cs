using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class StaffSearchHistoryRepository : BaseRepository, IStaffSearchHistoryRepository
    {
        public StaffSearchHistoryRepository(MySqlConnection connection) : base(connection) { }


        public async Task<IEnumerable<StaffSearchHistory>> GetStaffSearchHistory()
        {
            var staffHistory = new List<StaffSearchHistory>();

            var command = CreateCommand(
                @"SELECT 
                    StaffID, 
                    SearchQuery, 
                    SearchFilters, 
                    SearchType, 
                    SearchDate
                  FROM 
                    StaffSearchHistory"
            );

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                staffHistory.Add(new StaffSearchHistory
                {
                    StaffID = reader.GetInt32(0),
                    SearchQuery = reader.GetString(1),
                    SearchFilters = reader.IsDBNull(2) ? "{}" : reader.GetString(2), // Фікс перевірки NULL
                    SearchType = reader.GetString(3),
                    SearchDate = reader.GetDateTime(4)
                });
            }

            _connection.Close();
            return staffHistory;
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
    }
}
