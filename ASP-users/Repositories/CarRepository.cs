using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class CarRepository : BaseRepository, ICarRepository
    {
        public CarRepository(MySqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<Car>> GetAllCars()
        {
            var carsList = new List<Car>();

            var command = CreateCommand(
                @"SELECT 
	                CarID,
	                UserID,
	                Firm,
	                Model, 
	                Color,
	                Year,
	                LicensePlate,
	                CarPhotoURL
                FROM 
                    Cars"
            );

            _connection.Open();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var carId = reader.GetInt32(0);

                var car = carsList.FirstOrDefault(x => x.CarID == carId);

                if (car == null) // Якщо користувача не знайдено
                {
                    car = new Car // Створюємо нового користувача
                    {
                        CarID = carId,
                        UserID = reader.GetGuid(1),
                        Firm = reader.GetString(2),
                        Model = reader.GetString(3),
                        Color = reader.GetString(4),
                        Year = reader.GetInt32(5),
                        LicensePlate = reader.GetString(6),
                        CarPhotoURL = reader.IsDBNull(7) ? null : reader.GetString(7)
                    };
                    carsList.Add(car);
                }
            }

            _connection.Close();

            return carsList;
        }

        public async Task<Car> GetCarById(int carId)
        {
            Car car = null;

            var command = CreateCommand(
                @"SELECT 
	                CarID,
	                UserID,
	                Firm,
	                Model, 
	                Color,
	                Year,
	                LicensePlate,
	                CarPhotoURL
                FROM 
                    Cars
                WHERE 
                    CarID = @CarID"
            );

            command.Parameters.AddWithValue("@CarID", carId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (car == null)
                {
                    car = new Car
                    {
                        CarID = reader.GetInt32(0),
                        UserID = reader.GetGuid(1),
                        Firm = reader.GetString(2),
                        Model = reader.GetString(3),
                        Color = reader.GetString(4),
                        Year = reader.GetInt32(5),
                        LicensePlate = reader.GetString(6),
                        CarPhotoURL = reader.IsDBNull(7) ? null : reader.GetString(7)
                    };
                }
            }
            _connection.Close();

            return car;
        }

        public async Task<IEnumerable<Car>> GetAllUsersCars(Guid userId)
        {
            var usersCars = new List<Car>();

            var command = CreateCommand(
                @"SELECT 
	                CarID,
	                UserID,
	                Firm,
	                Model, 
	                Color,
	                Year,
	                LicensePlate,
	                CarPhotoURL
                FROM 
                    Cars
                WHERE 
                    UserID = @UserID"
            );

            command.Parameters.AddWithValue("@UserID", userId);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var carID = reader.GetInt32(0);

                var car = usersCars.FirstOrDefault(x => x.CarID == carID);

                if (car == null)
                {
                    car = new Car
                    {
                        CarID = carID,
                        UserID = reader.GetGuid(1),
                        Firm = reader.GetString(2),
                        Model = reader.GetString(3),
                        Color = reader.GetString(4),
                        Year = reader.GetInt32(5),
                        LicensePlate = reader.GetString(6),
                        CarPhotoURL = reader.IsDBNull(7) ? null : reader.GetString(7)
                    };
                    usersCars.Add(car);
                }
            }

            _connection.Close();

            return usersCars;
        }

        public async Task UpdateCar(int carId, Car car)
        {
            var command = CreateCommand(
                @"UPDATE 
                    Cars
                SET 
                    Firm = @Firm,
                    Model = @Model,
                    Color = @Color,
                    Year = @Year,
                    LicensePlate = @LicensePlate,
                    CarPhotoURL = @CarPhotoURL
                WHERE 
                    CarID = @CarID"
            );

            command.Parameters.AddWithValue("@CarID", carId);
            command.Parameters.AddWithValue("@Firm", car.Firm);
            command.Parameters.AddWithValue("@Model", car.Model);
            command.Parameters.AddWithValue("@Color", car.Color);
            command.Parameters.AddWithValue("@Year", car.Year);
            command.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
            command.Parameters.AddWithValue("@CarPhotoURL", car.CarPhotoURL);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }

        public async Task AddCarToUser(Guid userId, Car car)
        {
            var command = CreateCommand(
                @"INSERT INTO Cars 
                    (UserID, Firm, Model, Color, Year, LicensePlate, CarPhotoURL) 
                VALUES 
                    (@UserID, @Firm, @Model, @Color, @Year, @LicensePlate, @CarPhotoURL)"
            );

            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@Firm", car.Firm);
            command.Parameters.AddWithValue("@Model", car.Model);
            command.Parameters.AddWithValue("@Color", car.Color);
            command.Parameters.AddWithValue("@Year", car.Year);
            command.Parameters.AddWithValue("@LicensePlate", car.LicensePlate);
            command.Parameters.AddWithValue("@CarPhotoURL", car.CarPhotoURL);

            _connection.Open();
            await command.ExecuteNonQueryAsync();
            _connection.Close();
        }

        public async Task DeleteCar(int carId)
        {
            var command = CreateCommand(
                @"DELETE FROM 
                    Cars
                WHERE 
                    CarID = @CarID"
            );

            command.Parameters.AddWithValue("@CarID", carId);

            _connection.Open();

            await command.ExecuteNonQueryAsync();

            _connection.Close();
        }
    }
}
