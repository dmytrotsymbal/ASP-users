using ASP_users.Interfaces;
using ASP_users.Models;
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


        [HttpPut("update-phone/{phoneId}")]
        public async Task<IActionResult> UpdatePhone(int phoneId, Phone phone)
        {
            try
            {
                if (phone.PhoneID != phoneId)
                {
                    return BadRequest("User ID mismatch");
                }
                await _phoneRepository.UpdatePhone(phoneId, phone);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("add-phone-to/{userId}")]
        public async Task<IActionResult> AddPhoneToUser(Guid userId, Phone phone)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                phone.UserID = userId;
                await _phoneRepository.AddPhoneToUser(userId, phone);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-phone/{phoneId}")]
        public async Task<IActionResult> DeletePhone(int phoneId)
        {
            try
            {
                await _phoneRepository.DeletePhone(phoneId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
