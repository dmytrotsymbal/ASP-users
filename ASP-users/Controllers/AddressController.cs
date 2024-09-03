using ASP_users.Interfaces;
using ASP_users.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }


        [HttpGet("GetAllUsersAddresses/{userId}")]
        public async Task<IActionResult> GetAllUsersAddresses(Guid userId)
        {
            var usersAddresses = await _addressRepository.GetAllUsersAddresses(userId);
            if (usersAddresses == null)
            {
                return NotFound();
            }
            return Ok(usersAddresses);
        }


        
        
        [HttpGet("GetUserAddressById/{addressId}")]
        public async Task<IActionResult> GetUserAddressById(int addressId)
        {
            var address = await _addressRepository.GetUserAddressById(addressId);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }



        
        [HttpPut("UpdateAddress/{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, Address address)
        {
            if (address.AddressID != addressId)
            {
                return BadRequest("User ID mismatch");
            }
            await _addressRepository.UpdateAddress(addressId, address);
            return NoContent();
        }




        [HttpPost("AddAddressToUser/{userId}")]
        public async Task<IActionResult> AddAddressToUser(Guid userId, Address address)
        {
            await _addressRepository.AddAddressToUser(userId, address);
            return CreatedAtAction("GetUserAddressById", new { addressId = address.AddressID }, address);
        }




        [HttpDelete("DeleteAddress/{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            await _addressRepository.DeleteAddress(addressId);
            return NoContent();
        }
    }
}
