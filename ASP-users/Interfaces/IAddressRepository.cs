using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllUsersAddresses(Guid userId);
    }
}
