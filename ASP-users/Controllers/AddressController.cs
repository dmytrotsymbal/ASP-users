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
        private readonly IUserAddressRepository _userAddressRepository;

        public AddressController(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }





        [HttpGet("GetAllUsersAddresses/{userId}")]
        public async Task<IActionResult> GetUserAddresses(Guid userId)
        {
            var usersAddresses = await _userAddressRepository.GetUserAddresses(userId);
            if (usersAddresses == null)
            {
                return NotFound();
            }
            return Ok(usersAddresses);
        }
    }
}
