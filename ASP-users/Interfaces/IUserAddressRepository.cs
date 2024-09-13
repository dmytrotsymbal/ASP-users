﻿using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<IEnumerable<DetailedUserAddress>> GetUserAddresses(Guid userId);
        Task AddUserAddress(UserAddress userAddress);
        Task RemoveUserAddress(int userAddressID);
    }
}