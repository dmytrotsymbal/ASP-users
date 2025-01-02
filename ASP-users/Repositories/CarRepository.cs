using ASP_users.Interfaces;
using ASP_users.Models;
using MySqlConnector;

namespace ASP_users.Repositories
{
    public class CarRepository : BaseRepository, ICarRepository
    {
        public CarRepository(MySqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<Car>> GetAllCars(int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            var offset = (pageNumber - 1) * pageSize;

            switch (sortBy)
            {
                case "Firm":
                    sortBy = "cars.Firm";
                    break;
                case "Model":
                    sortBy = "cars.Model";
                    break;
                case "Year":
                    sortBy = "cars.Year";
                    break;
            };

            switch (sortDirection)
            {
                case "asc":
                    sortDirection = "ASC";
                    break;
                case "desc":
                    sortDirection = "DESC";
                    break;
            };

            var carsList = new List<Car>();

            var command = CreateCommand(
                $@"SELECT 
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
                ORDER BY
                    {sortBy} {sortDirection}
                LIMIT 
                    @offset, @pageSize"
            );

            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@pageSize", pageSize);
            command.Parameters.AddWithValue("@sortBy", sortBy);
            command.Parameters.AddWithValue("@sortDirection", sortDirection);

            _connection.Open();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var carId = reader.GetInt32(0);

                var car = carsList.FirstOrDefault(x => x.CarID == carId); // Перевіряємо чи машину вже знайдено

                if (car == null) // Якщо машини не знайдено
                {
                    car = new Car // Створюємо нову машину
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
                    carsList.Add(car); // Додаємо машину до списку
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

        public async Task<IEnumerable<Car>> SearchCars(
            string? searchQuery,
            int? minYear,
            int? maxYear,
            string? carColor,
            bool? onlyWithPhoto)
        {
            var searchedCars = new List<Car>();

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
                    (@searchQuery IS NULL OR Firm LIKE @searchQuery OR Model LIKE @searchQuery OR LicensePlate LIKE @searchQuery)
                    AND (@minYear IS NULL OR Year >= @minYear)
                    AND (@maxYear IS NULL OR Year <= @maxYear)
                    AND (@carColor IS NULL OR Color LIKE @carColor)
                    AND (@OnlyWithPhoto = 0 OR (CarPhotoURL IS NOT NULL AND CarPhotoURL != ''))
                ORDER BY 
                    CarID"
            );

            command.Parameters.AddWithValue("@searchQuery", string.IsNullOrEmpty(searchQuery) ? DBNull.Value : $"%{searchQuery}%");
            command.Parameters.AddWithValue("@MinYear", minYear ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MaxYear", maxYear ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CarColor", carColor ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@OnlyWithPhoto", onlyWithPhoto ?? (object)DBNull.Value);

            _connection.Open();

            var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var carID = reader.GetInt32(0);

                var car = searchedCars.FirstOrDefault(x => x.CarID == carID);

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
                        CarPhotoURL = reader.GetString(7)
                    };
                    searchedCars.Add(car);
                }
            }
            _connection.Close();

            return searchedCars;
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


        // HALPERS
        public async Task<int> GetCarsCount()
        {
            var command = CreateCommand(
                @"SELECT 
                    COUNT(*) 
                FROM 
                    Cars"
            );

            _connection.Open();

            var carsCount = (int)(long)await command.ExecuteScalarAsync();

            _connection.Close();

            return carsCount;
        }


        public async Task<Car> CheckLicensePlateExist(string licensePlate)
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
                    LicensePlate = @LicensePlate"
            );

            command.Parameters.AddWithValue("@LicensePlate", licensePlate);

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
    }
}
