using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface ICriminalRecordRepository
    {
        Task<IEnumerable<CriminalRecord>> GetAllUsersCriminalRecords(Guid userId);
    }
}
