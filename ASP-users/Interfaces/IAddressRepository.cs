using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetUserAddresses(Guid userId);

        Task<Address> GetUserAddressById(int andressId);
    }
}
