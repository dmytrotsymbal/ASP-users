using ASP_users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffSearchHistoryController : ControllerBase
    {
        private readonly IStaffSearchHistoryRepository _staffSearchHistoryRepository;

        public StaffSearchHistoryController(IStaffSearchHistoryRepository staffSearchHistoryRepository)
        {
            _staffSearchHistoryRepository = staffSearchHistoryRepository;
        }


        [HttpGet("get-all-staff-search-history")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetStaffSearchHistory()
        {
            try
            {
                var history = await _staffSearchHistoryRepository.GetStaffSearchHistory();
                if (history == null)
                {
                    return NotFound();
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
