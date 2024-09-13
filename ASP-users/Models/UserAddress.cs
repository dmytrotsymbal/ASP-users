namespace ASP_users.Models
{
    public class UserAddress
    {
        public int UserAddressID { get; set; }
        public Guid UserID { get; set; }
        public int AddressID { get; set; }
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
    }
}
