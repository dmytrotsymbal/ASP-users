using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Repositories;
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



        [HttpGet("get-by-id/{addressId}")]
        public async Task<IActionResult> GetUserAddressById(int addressId)
        {
            try
            {
                var usersAddress = await _userAddressRepository.GetUserAddressById(addressId);
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

    }
}
