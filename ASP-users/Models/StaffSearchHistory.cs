namespace ASP_users.Models
{
    public class StaffSearchHistory
    {
        public int SearchID { get; set; }
        public string Nickname { get; set; }            // нікнейм стафа
        public string Email { get; set; }               // емейл стафа
        public string Role { get; set; }                // роль стафа
        public string SearchQuery { get; set; }         // запит пошуку
        public string? SearchFilters { get; set; }      // фільтри пошуку (JSON)
        public string SearchType { get; set; }          // тип пошуку ('users' || 'cars')
        public DateTime SearchDate { get; set; }
    }
}
