using ASP_users.Interfaces;
using ASP_users.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var users = await _userRepository.GetAllUsers(pageNumber, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        //метод для пошуку користувача по інмені і прізвищу
        [HttpGet("search")]
        public async Task<IActionResult> SearchUser(string searchQuery)
        {
            try
            {
                var user = await _userRepository.SearchUsers(searchQuery);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDto)
        {
            if (!ModelState.IsValid) // перевірка валідності вхідних даних
            {
                return BadRequest(ModelState); 
            }

            // Створення нового користувача на основі UserDTO
            var user = new User
            {
                UserID = Guid.NewGuid(),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                DateOfBirth = userDto.DateOfBirth,
                CreatedAt = DateTime.Now // Додаємо дату створення
            };

            await _userRepository.CreateUser(user); // викликаємо метод для створення користувача
            return CreatedAtAction(nameof(GetAllUsers), new { id = user.UserID }, user.UserID); // повертаємо створеного користувача
        }

        public class UserDTO 
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime DateOfBirth { get; set; }
        }




        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] User user)
        {
            try
            {
                if (user.UserID != userId)
                {
                    return BadRequest("User ID mismatch");
                }

                await _userRepository.UpdateUser(userId, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            try
            {
                await _userRepository.DeleteUser(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        // ========================================================================================================
        // HELPERS

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(email); // викликаємо метод для пошуку користувача по email
                if (user != null) // якщо користувач знайдений
                {
                    return Conflict(new { message = "Такий Email вже існує" }); // повертаємо конфлікт з повідомленням
                }
                return Ok(); // якщо користувача не знайдено, повертаємо ОК
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("quantity")]
        public async Task<IActionResult> GetUsersCount()
        {
            try
            {
                var count = await _userRepository.GetUsersCount(); // викликаємо метод для знаходження кількості користувачів
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("get-all-ids")]
        public async Task<IActionResult> GetAllUsersIDs()
        {
            try
            {
                var ids = await _userRepository.GetAllUsersIDs();
                return Ok(ids);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
