using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Repositories;
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

        [HttpGet("GetAllUsersPhones/{userId}")]
        public async Task<IActionResult> GetAllUsersPhones(Guid userId)
        {
            var usersPhones = await _phoneRepository.GetAllUsersPhones(userId);
            if (usersPhones == null)
            {
                return NotFound();
            }
            return Ok(usersPhones);
        }
    }
}
