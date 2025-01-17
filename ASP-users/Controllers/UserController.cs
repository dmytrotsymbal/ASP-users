﻿using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IStaffSearchHistoryRepository _staffSearchHistoryRepository;

        public UserController(IUserRepository userRepository, IStaffSearchHistoryRepository staffSearchHistoryRepository)
        {
            _userRepository = userRepository;
            _staffSearchHistoryRepository = staffSearchHistoryRepository;
        }


        [HttpGet]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetAllUsers(int pageNumber = 1, int pageSize = 10, string sortBy = "UserID", string sortDirection = "asc")
        {
            try
            {
                var users = await _userRepository.GetAllUsers(pageNumber, pageSize, sortBy, sortDirection);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{userId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
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



        [HttpGet("search-users")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> SearchUser(
            [FromQuery] string? searchQuery,
            [FromQuery] int? minAge,
            [FromQuery] int? maxAge,
            [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo,
            [FromQuery] bool? onlyAdults,
            [FromQuery] bool? onlyWithPhoto)
        {
            try
            {
                if (string.IsNullOrEmpty(searchQuery) && !minAge.HasValue && !maxAge.HasValue && !createdFrom.HasValue && !createdTo.HasValue && !onlyAdults.HasValue && !onlyWithPhoto.HasValue)
                {
                    return BadRequest("Не задано жодного пошукового запиту");
                }

                var staffId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");

                var searchedUsers = await _userRepository.SearchUsers(
                    searchQuery,
                    minAge,
                    maxAge,
                    createdFrom,
                    createdTo,
                    onlyAdults,
                    onlyWithPhoto);

                if (!searchedUsers.Any())
                {
                    return NotFound("Користувачів за вишим запитом не знайдено");
                }

                var searchFilters = System.Text.Json.JsonSerializer.Serialize(new
                {
                    minAge = minAge ?? 0,
                    maxAge = maxAge ?? 120,
                    createdFrom = createdFrom ?? DateTime.MinValue,
                    createdTo = createdTo ?? DateTime.MaxValue,
                    onlyAdults = onlyAdults ?? false,
                    onlyWithPhoto = onlyWithPhoto ?? false
                });


                await _staffSearchHistoryRepository.AddSearchHistory(
                     staffId,
                     searchQuery ?? "",
                     searchFilters,
                     "users"
                 );


                return Ok(searchedUsers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return BadRequest(ex.Message);

            }
        }



        [HttpPost]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> CreateUser(UserDTO userDto)
        {
            try
            {
                if (!ModelState.IsValid) // перевірка валідності вхідних даних
                {
                    return BadRequest(ModelState);
                }

                var createdUser = new User     // cтворення нового користувача на основі UserDTO
                {
                    UserID = Guid.NewGuid(),  // генерується автоматично
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    DateOfBirth = userDto.DateOfBirth,
                    CreatedAt = DateTime.Now   // генерується автоматично
                };

                await _userRepository.CreateUser(createdUser);
                return CreatedAtAction(nameof(GetAllUsers), new { id = createdUser.UserID }, createdUser.UserID);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpPut("{userId}")]
        [Authorize(Roles = "admin, moderator")]
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
        [Authorize(Roles = "admin")]
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
                var user = await _userRepository.CheckEmailExists(email); // викликаємо метод для пошуку користувача по email
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
        [Authorize(Roles = "admin, moderator, visitor")]
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
        [Authorize(Roles = "admin, moderator, visitor")]
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
