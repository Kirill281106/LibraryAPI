using LibraryAPI.DB;
using LibraryAPI.DTOs.Auth;
using LibraryAPI.Models;
using LibraryAPI.Service.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JWTService _jwtService;

        public AuthController(AppDbContext db, JWTService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Логин и пароль обязательны" });

            var exists = await _db.Users
                .AnyAsync(u => u.UserName == request.Username);
            if (exists)
                return Conflict(new { message = "Пользователь уже существует" });

            var user = new User
            {
                UserName = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "User"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Пользователь создан" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.UserName == request.Username);

            if (user is null)
                return Unauthorized(new { message = "Неверный логин или пароль" });

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Неверный логин или пароль" });

            var token = _jwtService.GenerateToken(user);
            return Ok(new LoginResponse { Token = token });
        }
    }
}