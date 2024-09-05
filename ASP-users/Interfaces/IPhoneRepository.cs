using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IPhoneRepository
    {
        Task<IEnumerable<Phone>> GetAllUsersPhones(Guid userId);
    }
}
