﻿using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(int pageNumber = 1, int pageSize = 10);
        Task<User> GetUserById(Guid userId);
        Task<IEnumerable<User>> SearchUsers(string searchQuery);
        Task CreateUser(User user);
        Task UpdateUser(Guid userId, User user);
        Task DeleteUser(Guid userId);



        // HELPERS
        Task<User> GetUserByEmail(string email);

        Task<int> GetUsersCount();

        Task<IEnumerable<Guid>> GetAllUsersIDs();
    }
}
