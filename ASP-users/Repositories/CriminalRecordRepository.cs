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
                    criminalrecords.CriminalRecordID,
                    criminalrecords.UserID,
                    criminalrecords.Article,
                    criminalrecords.ConvictionDate,
                    criminalrecords.ReleaseDate,
                    criminalrecords.Sentence,
                    criminalrecords.CaseDetailsURL,
                    criminalrecords.Details,
                    prisons.PrisonID,
                    prisons.PrisonName,
                    prisons.Location,
                    prisons.Capacity,
                    prisons.SecurityLevel
                  FROM 
                    criminalrecords LEFT JOIN prisons ON criminalrecords.PrisonID = prisons.PrisonID
                  WHERE 
                    criminalrecords.UserID = @UserID;"
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
                        Details = reader.GetString(7),
                        Prison = new Prison
                        {
                            PrisonID = reader.GetInt32(8),
                            PrisonName = reader.GetString(9),
                            Location = reader.GetString(10),
                            Capacity = reader.GetInt32(11),
                            SecurityLevel = Enum.Parse<Prison.SecurityLevelEnum>(reader.GetString(12))
                        }
                    };
                    usersCrimes.Add(criminalRecord);
                }   
            }
            _connection.Close();

            return usersCrimes;
        }


        public async Task<CriminalRecord> GetCriminalRecordById(int criminalRecordId)
        {
            CriminalRecord criminalRecord = null;

            var command = CreateCommand(
                @"SELECT 
                    criminalrecords.CriminalRecordID,
                    criminalrecords.UserID,
                    criminalrecords.Article,
                    criminalrecords.ConvictionDate,
                    criminalrecords.ReleaseDate,
                    criminalrecords.Sentence,
                    criminalrecords.CaseDetailsURL,
                    criminalrecords.Details,
                    prisons.PrisonID,
                    prisons.PrisonName,
                    prisons.Location,
                    prisons.Capacity,
                    prisons.SecurityLevel
                  FROM 
                    criminalrecords LEFT JOIN prisons ON criminalrecords.PrisonID = prisons.PrisonID
                  WHERE 
                    criminalrecords.CriminalRecordID = @CriminalRecordID;"
            );

            command.Parameters.AddWithValue("@CriminalRecordID", criminalRecordId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
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
                    Details = reader.GetString(7),
                    Prison = new Prison
                    {
                        PrisonID = reader.GetInt32(8),
                        PrisonName = reader.GetString(9),
                        Location = reader.GetString(10),
                        Capacity = reader.GetInt32(11),
                        SecurityLevel = Enum.Parse<Prison.SecurityLevelEnum>(reader.GetString(12))
                    }
                };
            }
            _connection.Close();

            return criminalRecord;
        }
    }
}
