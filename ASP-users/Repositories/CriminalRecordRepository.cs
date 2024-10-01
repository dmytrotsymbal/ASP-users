using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class CriminalRecordRepository : BaseRepository, ICriminalRecordRepository
    {
        public CriminalRecordRepository(MySqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<CriminalRecord>> GetAllUsersCriminalRecords(Guid userId)
        {
            var usersCrimes = new List<CriminalRecord>();

            var command = CreateCommand(
                @"SELECT 
                    CriminalRecordID,
                    UserID,
                    Article,
                    ConvictionDate,
                    ReleaseDate,
                    Sentence,
                    CaseDetailsURL,
                    PrisonID,
                    Details
                  FROM 
                    Criminalrecords 
                  WHERE 
                    UserID = @UserID"
            );

            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var CriminalRecordID = reader.GetInt32(0);

                var criminalRecord = usersCrimes.FirstOrDefault(x => x.CriminalRecordID == CriminalRecordID);

                if (criminalRecord == null)
                {
                    criminalRecord = new CriminalRecord
                    {
                        CriminalRecordID = reader.GetInt32(0),
                        UserID = reader.GetGuid(1),
                        Article = reader.GetString(2),
                        ConvictionDate = reader.GetDateTime(3),
                        ReleaseDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                        Sentence = reader.GetString(5),
                        CaseDetailsURL = reader.IsDBNull(6) ? (string?)null : reader.GetString(6),
                        PrisonID = reader.GetInt32(7),
                        Details = reader.GetString(8),
                    };
                    usersCrimes.Add(criminalRecord);
                }   
            }
            _connection.Close();

            return usersCrimes;
        }


    }
}
