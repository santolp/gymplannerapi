using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GymPlannerApi.Data;
using GymPlannerApi.Models;
using BCrypt.Net;

namespace GymPlannerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.Role,
                    user.Email,
                    user.Nombre,
                    user.Apellido,
                    user.DNI,
                    user.Telefono,
                    user.Direccion,
                    user.IsEnabled,
                    user.CreatedAt
                }
            });
        }

        private string GenerateJwtToken(User user)
        {
            var key = _config["Jwt:Key"] ?? "VERY_LONG_SECRET_KEY_1234567890123456";
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Endpoint para crear usuarios de prueba completos
        [HttpPost("register-test-users")]
        public IActionResult RegisterTestUsers()
        {
            if (_context.Users.Any()) return BadRequest("Usuarios de prueba ya creados");

            _context.Users.AddRange(
     new User
     {
         Id = Guid.NewGuid(),
         Username = "admin",
         PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
         Role = "admin",
         Email = "admin@example.com",
         Nombre = "Admin",
         Apellido = "Admin",
         DNI = "00000000",
         Telefono = "0000000000",
         Direccion = "N/A",
         IsEnabled = true,
         CreatedAt = DateTime.UtcNow
     },
     new User
     {
         Id = Guid.NewGuid(),
         Username = "profe",
         PasswordHash = BCrypt.Net.BCrypt.HashPassword("profe123"),
         Role = "profe",
         Email = "profe@example.com",
         Nombre = "Profe",
         Apellido = "Profe",
         DNI = "11111111",
         Telefono = "1111111111",
         Direccion = "N/A",
         IsEnabled = true,
         CreatedAt = DateTime.UtcNow
     },
     new User
     {
         Id = Guid.NewGuid(),
         Username = "alumno",
         PasswordHash = BCrypt.Net.BCrypt.HashPassword("alumno123"),
         Role = "alumno",
         Email = "alumno@example.com",
         Nombre = "Alumno",
         Apellido = "Alumno",
         DNI = "22222222",
         Telefono = "2222222222",
         Direccion = "N/A",
         IsEnabled = true,
         CreatedAt = DateTime.UtcNow
     }
 );

            _context.SaveChanges();
            return Ok("Usuarios de prueba creados");
        }

    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
