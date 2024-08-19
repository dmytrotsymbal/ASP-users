namespace ASP_users.Models
{
    public class Phone
    {
        public int PhoneID { get; set; }
        public Guid UserID { get; set; }
        public string PhoneNumber { get; set; }
    }
}
