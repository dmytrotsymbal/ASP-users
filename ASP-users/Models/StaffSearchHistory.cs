namespace ASP_users.Models
{
    public class StaffSearchHistory
    {
        public int SearchID { get; set; }
        public int StaffID { get; set; }
        public string SearchQuery { get; set; }
        public string? SearchFilters { get; set; } // JSON
        public string SearchType { get; set; } // 'users' || 'cars'
        public DateTime SearchDate { get; set; }
    }
}
