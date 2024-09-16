namespace ASP_users.Models.Helpers
{
    public class Resident
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
    }
}


// модель для методу GetAddressLivingHistory (вивести історію всіх жителів певної адреси)