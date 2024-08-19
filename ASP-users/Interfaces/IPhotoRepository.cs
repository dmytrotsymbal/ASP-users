using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface IPhotoRepository
    {
        Task AddPhoto(Guid userId, Photo photo);

        Task DeletePhoto(int imageID);
    }
}
