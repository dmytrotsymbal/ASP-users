using ASP_users.Models;
using ASP_users.Models.DTO;

namespace ASP_users.Interfaces
{
    public interface ICriminalRecordRepository
    {
        Task<IEnumerable<CriminalRecord>> GetAllUsersCriminalRecords(Guid userId);

        Task<CriminalRecord> GetCriminalRecordById(int criminalRecordID);

        Task UpdateCriminalRecord(int criminalRecordID, CrimeDTO criminalRecord);

        Task AddCrimeToUser(Guid userId, CriminalRecord criminalRecord);

        Task DeleteCriminalRecord(int criminalRecordID);
    }
}
