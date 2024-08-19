namespace ASP_users.Models
{
    public class User
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }


        // список фотографій користувача
        public List<Photo> Photos { get; set; } = new List<Photo> ();
    }
}
