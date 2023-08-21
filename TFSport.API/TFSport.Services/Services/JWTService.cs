using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TFSport.Models;
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

		public async Task<string> GenerateAccessTokenAsync(string email, string id)
		{
			var authentificationSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var claims = new List<Claim>
			{
				new Claim("email", email),
				new Claim(ClaimTypes.NameIdentifier, id),
			}; 

			var token = new JwtSecurityToken(
				_configuration["JWT:ValidAudience"],
				_configuration["JWT:ValidIssuer"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:AccessTokenExpirationMinutes"])),
				signingCredentials: new SigningCredentials(authentificationSigningKey, SecurityAlgorithms.HmacSha256)
			);

			var handler = new JwtSecurityTokenHandler();
			return handler.WriteToken(token);
		}

		public async Task<string> GenerateRefreshTokenAsync(string email,string id)
		{
            var refreshSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:RefreshSecret"]));

            var claims = new List<Claim>
                {
                    new Claim("email", email),
                    new Claim(ClaimTypes.NameIdentifier, id),
                };

            var refreshToken = new JwtSecurityToken(
                _configuration["JWT:ValidAudience"],
                _configuration["JWT:ValidIssuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JWT:RefreshTokenExpirationDays"])),
                signingCredentials: new SigningCredentials(refreshSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(refreshToken);
        }

		public async Task<string> GetIdFromToken(string token)
		{
			var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
			var id = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
			return id;
		}
		public async Task<string> GetEmailFromToken(string token)
		{
			var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
			var email = jwtToken.Claims.First(claim => claim.Type == "email").Value;
			return email;
		}
	}
}
