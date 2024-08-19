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


        [HttpGet("GetAllCars")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carRepository.GetAllCars();
            return Ok(cars);
        }


        [HttpGet("GetCarById/{carId}")]
        public async Task<IActionResult> GetCarById(int carId) 
        {
            var car = await _carRepository.GetCarById(carId);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }


        [HttpGet("GetAllUsersCars/{userId}")]
        public async Task<IActionResult> GetAllUsersCars(Guid userId)
        {
            var userCars = await _carRepository.GetAllUsersCars(userId);
            if (userCars == null)
            {
                return NotFound();
            }
            return Ok(userCars);
        }



        [HttpPut("UpdateCar/{CarId}")]
        public async Task<IActionResult> UpdateCar(int carId, Car car)
        {
            if (car.CarID != carId)
            {
                return BadRequest("User ID mismatch");
            }
            await _carRepository.UpdateCar(carId, car);
            return NoContent();
        }


        [HttpPost("AddCarToUser/{userId}")]
        public async Task<IActionResult> AddCarToUser(Guid userId, [FromBody] Car car)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            car.UserID = userId; // Призначаємо UserID машині
            await _carRepository.AddCarToUser(userId, car);
            return Ok();
        }


        [HttpDelete("DeleteCar/{CarId}")]
        public async Task<IActionResult> DeleteCar(int CarId)
        {
            await _carRepository.DeleteCar(CarId);
            return NoContent();
        }
    }
}
