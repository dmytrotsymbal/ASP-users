using ASP_users.Interfaces;
using ASP_users.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetAllCars(int pageNumber, int pageSize, string sortBy, string sortDirection)
        {
            try
            {
                var cars = await _carRepository.GetAllCars(pageNumber, pageSize, sortBy, sortDirection);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-by-id/{carId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
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
        [Authorize(Roles = "admin, moderator, visitor")]
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
        [Authorize(Roles = "admin, moderator, visitor")]
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
        [Authorize(Roles = "admin, moderator")]
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
        [Authorize(Roles = "admin, moderator")]
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
        [Authorize(Roles = "admin")]
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


        // HALPERS =========================================================================
        [HttpGet("quantity")]
        [Authorize(Roles = "admin, moderator, visitor")]
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



        [HttpGet("check-licensePlate")]
        public async Task<IActionResult> CheckLicensePlate(string licensePlate)
        {
            try
            {
                var car = await _carRepository.CheckLicensePlateExist(licensePlate);
                if (car != null)
                {
                    return Conflict(new { message = "Машина з таким номером вже існує" });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
