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
    }
}
