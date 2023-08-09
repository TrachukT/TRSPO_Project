﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TFSport.Services.Interfaces;

namespace TFSport.Services.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateAccessTokenAsync(string email)
        {
            var authentificationSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:ValidAudience"],
                _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:AccessTokenExpirationMinutes"])),
                signingCredentials: new SigningCredentials(authentificationSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var refreshSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:RefreshSecret"]));

            var refreshToken = new JwtSecurityToken(
                _configuration["JWT:ValidAudience"],
                _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JWT:RefreshTokenExpirationDays"])),
                signingCredentials: new SigningCredentials(refreshSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(refreshToken);
        }
    }
}
