using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IStaffRepository
    {
        Task<StaffAccount?> GetStaffAccountByEmail(string email); // Возвращаем одного сотрудника или null

        Task<IEnumerable<StaffSearchSummary>> GetTopSearchers();
    }
}
