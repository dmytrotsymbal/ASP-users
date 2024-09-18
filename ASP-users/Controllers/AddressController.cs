using ASP_users.Interfaces;
using ASP_users.Models;
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
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }



        [HttpGet("get-by-id/{addressId}/for-user/{userId}")]
        public async Task<IActionResult> GetUserAddressById(Guid userId, int addressId)
        {
            try
            {
                var usersAddress = await _userAddressRepository.GetUserAddressById(userId, addressId);
                if (usersAddress == null)
                {
                    return NotFound();
                }
                return Ok(usersAddress);
            } 
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }



        [HttpGet("get-living-history/{addressId}")]
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
            catch (Exception ex) {
                return BadRequest(ex);
            }
        }



        [HttpPut("update/{addressId}")]
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
        public async Task<IActionResult> AddExistingUserToLivingHistory(int addressId, [FromBody] AddUserToAddressRequest request)
        {
            try
            {
                await _userAddressRepository.AddExistingUserToLivingHistory(request.UserID, addressId, request.MoveInDate, request.MoveOutDate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class AddUserToAddressRequest
        {
            public Guid UserID { get; set; }
            public DateTime MoveInDate { get; set; }
            public DateTime? MoveOutDate { get; set; }
        }
    }
}
