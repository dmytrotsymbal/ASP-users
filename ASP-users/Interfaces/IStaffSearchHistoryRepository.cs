using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IStaffSearchHistoryRepository
    {
        Task<IEnumerable<StaffSearchHistory>> GetStaffSearchHistory();
        Task AddSearchHistory(int staffId, string searchQuery, string searchFilters, string searchType);
    }
}
