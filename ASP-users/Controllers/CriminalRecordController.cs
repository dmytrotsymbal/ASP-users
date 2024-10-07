using ASP_users.Interfaces;
using ASP_users.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CriminalRecordController : ControllerBase
    {
        private readonly ICriminalRecordRepository _criminalRecordRepository;

        public CriminalRecordController(ICriminalRecordRepository criminalRecordRepository)
        {
            _criminalRecordRepository = criminalRecordRepository;
        }


        [HttpGet("get-all-users-crimes/{userId}")]
        public async Task<IActionResult> GetAllUsersCriminalRecords(Guid userId)
        {
            try
            {
                var criminalRecords = await _criminalRecordRepository.GetAllUsersCriminalRecords(userId);
                if (criminalRecords == null)
                {
                    return NotFound("No criminal records found for the user.");
                }
                return Ok(criminalRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("get-crime-by-id/{criminalRecordId}")]
        public async Task<IActionResult> GetCriminalRecordById(int criminalRecordId)
        {
            try
            {
                var criminalRecord = await _criminalRecordRepository.GetCriminalRecordById(criminalRecordId);
                if (criminalRecord == null)
                {
                    return NotFound("Criminal record not found.");
                }
                return Ok(criminalRecord);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
