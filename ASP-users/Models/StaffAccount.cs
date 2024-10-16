namespace ASP_users.Models
{
    public class StaffAccount
    {
        public int StaffID { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public RoleEnum Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum RoleEnum
    {
        visitor,
        moderator,
        admin
    }
}
