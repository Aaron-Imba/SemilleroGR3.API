using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SemilleroGR3.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SemilleroGR3.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Información incrustada dentro del token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("RolId", usuario.RolId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetValue<string>("Issuer"),
                audience: jwtSettings.GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.GetValue<int>("ExpirationMinutes")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}