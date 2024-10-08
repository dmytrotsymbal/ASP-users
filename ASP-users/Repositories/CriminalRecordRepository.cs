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


        public async Task UpdateCriminalRecord(int criminalRecordId, CriminalRecord criminalRecord)
        {
            var command = CreateCommand(
                @"UPDATE 
                    criminalrecords 
                  SET 
                    UserID = @UserID,
                    Article = @Article,
                    ConvictionDate = @ConvictionDate,
                    ReleaseDate = @ReleaseDate,
                    Sentence = @Sentence,
                    CaseDetailsURL = @CaseDetailsURL,
                    Details = @Details,
                    PrisonID = @PrisonID
                  WHERE 
                    CriminalRecordID = @CriminalRecordID;"
            );

            command.Parameters.AddWithValue("@CriminalRecordID", criminalRecordId);
            command.Parameters.AddWithValue("@UserID", criminalRecord.UserID);
            command.Parameters.AddWithValue("@Article", criminalRecord.Article);
            command.Parameters.AddWithValue("@ConvictionDate", criminalRecord.ConvictionDate);
            command.Parameters.AddWithValue("@ReleaseDate", criminalRecord.ReleaseDate);
            command.Parameters.AddWithValue("@Sentence", criminalRecord.Sentence);
            command.Parameters.AddWithValue("@CaseDetailsURL", criminalRecord.CaseDetailsURL);
            command.Parameters.AddWithValue("@Details", criminalRecord.Details);
            command.Parameters.AddWithValue("@PrisonID", criminalRecord.Prison.PrisonID);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }


        public async Task AddCrimeToUser(Guid userId, CriminalRecord criminalRecord)
        {
            var command = CreateCommand(
                @"INSERT INTO 
                    criminalrecords (
                        UserID,
                        Article,
                        ConvictionDate,
                        ReleaseDate,
                        Sentence,
                        CaseDetailsURL,
                        Details,
                        PrisonID
                    )
                  VALUES 
                    (
                        @UserID,
                        @Article,
                        @ConvictionDate,
                        @ReleaseDate,
                        @Sentence,
                        @CaseDetailsURL,
                        @Details,
                        @PrisonID
                    );"
            );

            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@Article", criminalRecord.Article);
            command.Parameters.AddWithValue("@ConvictionDate", criminalRecord.ConvictionDate);
            command.Parameters.AddWithValue("@ReleaseDate", criminalRecord.ReleaseDate);
            command.Parameters.AddWithValue("@Sentence", criminalRecord.Sentence);
            command.Parameters.AddWithValue("@CaseDetailsURL", criminalRecord.CaseDetailsURL);
            command.Parameters.AddWithValue("@Details", criminalRecord.Details);
            command.Parameters.AddWithValue("@PrisonID", criminalRecord.Prison.PrisonID);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }



        public async Task DeleteCriminalRecord(int criminalRecordId)
        {
            var command = CreateCommand(
                @"DELETE FROM 
                    criminalrecords 
                  WHERE 
                    CriminalRecordID = @CriminalRecordID;"
            );

            command.Parameters.AddWithValue("@CriminalRecordID", criminalRecordId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }
    }
}
