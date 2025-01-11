﻿using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IStaffSearchHistoryRepository
    {
        Task<IEnumerable<StaffSearchHistory>> GetAllSearchHistory();
        Task<IEnumerable<StaffSearchHistory>> GetDetailedSearchHistoryByStaffId(int staffId);
        Task AddSearchHistory(int staffId, string searchQuery, string searchFilters, string searchType);
    }
}
