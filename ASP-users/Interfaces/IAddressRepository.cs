﻿using ASP_users.Models;
using ASP_users.Models.Helpers;

namespace ASP_users.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetUserAddresses(Guid userId);

        Task<Address> GetUserAddressById(int andressId);

        Task<IEnumerable<Resident>> GetAddressLivingHistory(int andressId);


        Task UpdateAddress(int addressId, Address address);


        Task AddAddressToUser(Guid userId, Address address);
         
    }
}
