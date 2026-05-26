using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemilleroGR3.API.Data;
using SemilleroGR3.API.DTOs;
using SemilleroGR3.API.Services;
using System.Threading.Tasks;

namespace SemilleroGR3.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SemilleroContext _context;
        private readonly JwtService _jwtService;

        public AuthController(SemilleroContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Si el usuario no existe o está inactivo
            if (usuario == null || !usuario.Activo)
                return Unauthorized(new { message = "Credenciales incorrectas." });

            // Verificar contraseña con BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);

            if (!isPasswordValid)
                return Unauthorized(new { message = "Credenciales incorrectas." });

            // Generar JWT
            var token = _jwtService.GenerateToken(usuario);

            var response = new AuthResponse
            {
                Id = usuario.Id,
                RolId = usuario.RolId,
                Email = usuario.Email,
                NombreCompleto = usuario.NombreCompleto,
                Telefono = usuario.Telefono,
                Token = token
            };

            return Ok(response);
        }
    }
}