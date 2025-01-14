using ASP_users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaffSearchHistoryController : ControllerBase
    {
        private readonly IStaffSearchHistoryRepository _staffSearchHistoryRepository;

        public StaffSearchHistoryController(IStaffSearchHistoryRepository staffSearchHistoryRepository)
        {
            _staffSearchHistoryRepository = staffSearchHistoryRepository;
        }


        [HttpGet("get-all-search-history")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllSearchHistory(int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var allHistory = await _staffSearchHistoryRepository.GetAllSearchHistory(pageNumber, pageSize);
                if (allHistory == null)
                {
                    return NotFound("Історія пошуку відсутня.");
                }
                return Ok(allHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-current-staff-search-history/{staffId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> CurrentStaffSearchHistory(int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                var history = await _staffSearchHistoryRepository.CurrentStaffSearchHistory(staffId, pageNumber, pageSize);

                if (history == null || !history.Any())
                {
                    return NotFound("Історія пошуку відсутня.");
                }

                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        // HALPERS

        [HttpGet("all-search-history-quantity")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> AllSearchHistoryQantity()
        {
            try
            {
                var quantity = await _staffSearchHistoryRepository.AllSearchHistoryQantity();
                return Ok(quantity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quantity: {ex.Message}");
                return BadRequest("Помилка отримання кількості записів.");
            }
        }


        [HttpGet("current-staff-search-history-quantity/{staffId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> CurrentStaffSearchHistoryQantity()
        {
            try
            {
                var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var quantity = await _staffSearchHistoryRepository.CurrentStaffSearchHistoryQantity(staffId);
                return Ok(quantity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching quantity: {ex.Message}");
                return BadRequest("Помилка отримання кількості записів.");
            }
        }
    }
}
