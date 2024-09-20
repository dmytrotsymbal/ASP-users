namespace ASP_users.Models.DTO
{
    public class AddressToUserDTO
    {
        public Guid UserID { get; set; }
        public DateTime MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
    }
}
