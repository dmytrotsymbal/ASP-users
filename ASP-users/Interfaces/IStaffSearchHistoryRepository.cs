using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IStaffSearchHistoryRepository
    {
        Task<IEnumerable<StaffSearchHistory>> GetAllSearchHistory(int pageNumber = 1, int pageSize = 15);
        Task<IEnumerable<StaffSearchHistory>> CurrentStaffSearchHistory(int staffId, int pageNumber = 1, int pageSize = 15);
        Task AddSearchHistory(int staffId, string searchQuery, string searchFilters, string searchType);


        // HALPERS
        Task<int> AllSearchHistoryQantity();

        Task<int> CurrentStaffSearchHistoryQantity(int staffId);
    }
}
