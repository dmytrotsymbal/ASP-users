using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _userAddressRepository;

        public AddressController(IAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }


        [HttpGet("get-all/{userId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetUserAddresses(Guid userId)
        {
            try
            {
                var usersAddresses = await _userAddressRepository.GetUserAddresses(userId);
                if (usersAddresses == null)
                {
                    return NotFound();
                }
                return Ok(usersAddresses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        // ЮРЛ для получения адреса по ID без указания юзера (для отдельной страницы)
        [HttpGet("get-by-id/{addressId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            try
            {
                var address = await _userAddressRepository.GetUserAddressById(null, addressId);
                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ЮРЛ для получения адреса по ID в котором указіваться ID юзера (для форми редактирования)
        [HttpGet("get-by-id/{addressId}/for-user/{userId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetUserAddressById(Guid userId, int addressId)
        {
            try
            {
                var address = await _userAddressRepository.GetUserAddressById(userId, addressId);
                if (address == null)
                {
                    return NotFound();
                }
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





        [HttpGet("get-living-history/{addressId}")]
        [Authorize(Roles = "admin, moderator, visitor")]
        public async Task<IActionResult> GetAddressLivingHistory(int addressId)
        {
            try
            {
                var addressLivingHistory = await _userAddressRepository.GetAddressLivingHistory(addressId);
                if (addressLivingHistory == null)
                {
                    return NotFound();
                }
                return Ok(addressLivingHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPut("update/{addressId}")]
        [Authorize(Roles = "admin, moderator")]
        public async Task UpdateAddress(int addressId, [FromBody] Address address)
        {
            try
            {
                if (address.AddressID != addressId)
                {
                    throw new Exception("Address ID mismatch");
                }
                await _userAddressRepository.UpdateAddress(addressId, address);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [HttpPost("add/{userId}")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddAddressToUser(Guid userId, Address address)
        {
            try
            {
                await _userAddressRepository.AddAddressToUser(userId, address);
                return CreatedAtAction("GetUserAddressById", new { addressId = address.AddressID }, address);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding address: {ex.Message}");

                return StatusCode(500, new { message = "An error occurred while adding the address.", details = ex.Message });
            }
        }



        [HttpDelete("remove/{addressId}/from-user/{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveAddressFromUser(Guid userId, int addressId)
        {
            try
            {
                await _userAddressRepository.RemoveAddressFromUser(userId, addressId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting address: {ex.Message}");

                return StatusCode(500, new { message = "An error occurred while deleting the address.", details = ex.Message });
            }
        }



        [HttpPost("add-existing-user/{addressId}")]
        [Authorize(Roles = "admin, moderator")]
        public async Task<IActionResult> AddExistingUserToLivingHistory(int addressId, [FromBody] AddressToUserDTO addressToUserDTO)
        {
            try
            {
                await _userAddressRepository.AddExistingUserToLivingHistory(
                    addressToUserDTO.UserID,
                    addressId,
                    addressToUserDTO.MoveInDate,
                    addressToUserDTO.MoveOutDate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpDelete("total-delete/{addressId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> TotalDeleteAddress(int addressId)
        {
            try
            {
                await _userAddressRepository.TotalDeleteWholeAddress(addressId);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting address: {ex.Message}");

                return StatusCode(500, new { message = "An error occurred while deleting the address.", details = ex.Message });
            }
        }
    }
}
