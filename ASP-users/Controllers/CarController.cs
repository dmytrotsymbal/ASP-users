using ASP_users.Interfaces;
using ASP_users.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarRepository _carRepository;

        public CarController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }


        [HttpGet("get-all-cars")]
        public async Task<IActionResult> GetAllCars(int pageNumber, int pageSize)
        {
            try
            {
                var cars = await _carRepository.GetAllCars(pageNumber, pageSize);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-by-id/{carId}")]
        public async Task<IActionResult> GetCarById(int carId)
        {
            try
            {
                var car = await _carRepository.GetCarById(carId);
                if (car == null)
                {
                    return NotFound();
                }
                return Ok(car);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-all-users-cars/{userId}")]
        public async Task<IActionResult> GetAllUsersCars(Guid userId)
        {
            try
            {
                var userCars = await _carRepository.GetAllUsersCars(userId);
                if (userCars == null)
                {
                    return NotFound();
                }
                return Ok(userCars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("search-cars")]
        public async Task<IActionResult> SearchCars(string searchQuery)
        {
            try
            {
                var cars = await _carRepository.SearchCars(searchQuery);
                if (cars == null)
                {
                    return NotFound();
                }
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update/{CarId}")]
        public async Task<IActionResult> UpdateCar(int carId, Car car)
        {
            try
            {
                if (car.CarID != carId)
                {
                    return BadRequest("User ID mismatch");
                }
                await _carRepository.UpdateCar(carId, car);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddCarToUser(Guid userId, [FromBody] Car car)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                car.UserID = userId; // Призначаємо UserID машині
                await _carRepository.AddCarToUser(userId, car);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete/{CarId}")]
        public async Task<IActionResult> DeleteCar(int CarId)
        {
            try
            {
                await _carRepository.DeleteCar(CarId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // HALPERS

        [HttpGet("quantity")]
        public async Task<IActionResult> GetCarsCount()
        {
            try
            {
                int carsCount = await _carRepository.GetCarsCount();
                return Ok(carsCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
