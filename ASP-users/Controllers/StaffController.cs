using ASP_users.Interfaces;
using ASP_users.Models;
using ASP_users.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP_users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IConfiguration _config; // Добавляем IConfiguration

        public StaffController(IStaffRepository staffRepository, IConfiguration config)
        {
            _staffRepository = staffRepository;
            _config = config; // Внедряем IConfiguration через DI
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var staffAccount = await _staffRepository.GetStaffAccountByEmail(loginDto.Email);

            // Перевірка користувача
            if (staffAccount == null)
            {
                return Unauthorized("Invalid email or password."); // Повертаємо помилку
            }

            // Перевірка пароля 
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, staffAccount.PasswordHash);

            if (!isValidPassword)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Генерація JWT токена
            var token = GenerateJwtToken(staffAccount);

            // Повернення токена клієнту
            return Ok(new
            {
                Token = token, // Повертаємо токен
                staffAccount.Nickname,
                staffAccount.Role,
                staffAccount.CreatedAt,
                staffAccount.Email
            });
        }



        // HALPERS =========================================================================
        private string GenerateJwtToken(StaffAccount staffAccount)
        {
            var claims = new[] // полезная информация о пользователе которая будет в содержаться токене
            {
                new Claim(ClaimTypes.NameIdentifier, staffAccount.StaffID.ToString()),
                new Claim(ClaimTypes.Email, staffAccount.Email),
                new Claim(ClaimTypes.Role, staffAccount.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); // Ключ для шифрования токена
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // создаем токен
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token); // возвращаем токен в виде строки
        }
    }
}
