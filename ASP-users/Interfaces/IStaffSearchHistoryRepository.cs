using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IStaffSearchHistoryRepository
    {
        Task<IEnumerable<StaffSearchHistory>> GetAllSearchHistory(int pageNumber = 1, int pageSize = 20);
        Task<IEnumerable<StaffSearchHistory>> GetDetailedSearchHistoryByStaffId(int staffId);
        Task AddSearchHistory(int staffId, string searchQuery, string searchFilters, string searchType);


        // HALPERS
        Task<int> AllSearchHistoryQantity();
    }
}
