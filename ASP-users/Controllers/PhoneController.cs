using ASP_users.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneRepository _phoneRepository;

        public PhoneController(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }

        [HttpGet("get-all-users-phones/{userId}")]
        public async Task<IActionResult> GetAllUsersPhones(Guid userId)
        {
            try
            {
                var usersPhones = await _phoneRepository.GetAllUsersPhones(userId);
                if (usersPhones == null)
                {
                    return NotFound();
                }
                return Ok(usersPhones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-phone-by-id/{phoneId}")]
        public async Task<IActionResult> GetPhoneById(int phoneId)
        {
            try
            {
                var phone = await _phoneRepository.GetPhoneById(phoneId);
                if (phone == null)
                {
                    return NotFound();
                }
                return Ok(phone);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
