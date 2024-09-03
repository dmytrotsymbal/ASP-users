using ASP_users.Models;

namespace ASP_users.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCars(int pageNumber, int pageSize);

        Task<Car> GetCarById(int carId);

        Task<IEnumerable<Car>> GetAllUsersCars(Guid userId);

        Task<IEnumerable<Car>> SearchCars(string searchQuery);

        Task UpdateCar(int carId, Car car);

        Task AddCarToUser(Guid userId, Car car);

        Task DeleteCar(int CarId);


        // HALPERS
        Task<int> GetCarsCount();
    }
}
