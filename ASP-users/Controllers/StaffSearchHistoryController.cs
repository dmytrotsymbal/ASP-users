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


        [HttpGet("get-all-history-search")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllHistorySearch()
        {
            try
            {
                var allHistory = await _staffSearchHistoryRepository.GetAllSearchHistory();
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


        [HttpGet("get-my-search-history/{staffId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetMyDetailedHistory()
        {
            try
            {
                var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

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
