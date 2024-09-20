namespace ASP_users.Models.DTO
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}

// містить тільки ті поля, які необхідня для створення нового ЮЗЕРА (ті які треба заповнювати юзеру)

// UserID i CreatedAt генеруються автоматично