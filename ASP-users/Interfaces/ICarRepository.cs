using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCars();

        Task<Car> GetCarById(int carId);

        Task<IEnumerable<Car>> GetAllUsersCars(Guid userId);

        Task UpdateCar(int carId, Car car);

        Task AddCarToUser(Guid userId, Car car);

        Task DeleteCar(int CarId);
    }
}
