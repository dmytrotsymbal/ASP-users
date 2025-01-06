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

        // Получаем подробную историю для текущего пользователя
        [HttpGet("get-my-search-history")]
        public async Task<IActionResult> GetMyDetailedHistory()
        {
            try
            {
                // Получаем ID текущего пользователя
                var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                // Получаем историю из репозитория
                var history = await _staffSearchHistoryRepository.GetDetailedSearchHistoryByStaffId(staffId);

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
    }
}
