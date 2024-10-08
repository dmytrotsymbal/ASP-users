using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface ICriminalRecordRepository
    {
        Task<IEnumerable<CriminalRecord>> GetAllUsersCriminalRecords(Guid userId);

        Task<CriminalRecord> GetCriminalRecordById(int criminalRecordID);

        Task UpdateCriminalRecord(int criminalRecordID, CriminalRecord criminalRecord);

        Task AddCrimeToUser(Guid userId, CriminalRecord criminalRecord);

        Task DeleteCriminalRecord(int criminalRecordID);
    }
}
