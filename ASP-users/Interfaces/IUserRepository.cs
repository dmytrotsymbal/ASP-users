using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(int pageNumber = 1, int pageSize = 10, string sortBy = "UserID", string sortDirection = "asc");
        Task<User> GetUserById(Guid userId);
        Task<IEnumerable<User>> SearchUsers(
            string? searchQuery,
            int? minAge,
            int? maxAge,
            DateTime? createdFrom,
            DateTime? createdTo,
            bool? onlyAdults,
            bool? onlyWithPhoto);
        Task CreateUser(User user);
        Task UpdateUser(Guid userId, User user);
        Task DeleteUser(Guid userId);



        // HELPERS
        Task<User> CheckEmailExists(string email);

        Task<int> GetUsersCount();

        Task<IEnumerable<Guid>> GetAllUsersIDs();
    }
}
