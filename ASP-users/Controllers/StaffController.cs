using ASP_users.Interfaces;
using ASP_users.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;

        public StaffController(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var staffAccount = await _staffRepository.GetStaffAccountByEmail(loginDto.Email);

            if (staffAccount == null)
                return Unauthorized("Invalid email or password.");

            // Проверка пароля
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, staffAccount.PasswordHash);
            if (!isValidPassword)
                return Unauthorized("Invalid email or password.");

            // Если логин успешен, возвращаем информацию о роли
            return Ok(new
            {
                staffAccount.Nickname,
                staffAccount.Role,
                staffAccount.CreatedAt,
                staffAccount.Email
            });
        }
    }
}
