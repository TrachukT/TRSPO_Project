namespace TFSport.Services.Interfaces
{
	public interface IJWTService
	{
		public Task<string> GenerateAccessTokenAsync(string email);

		public Task<string> GenerateRefreshTokenAsync(string email);

		public Task<string> GetEmailFromToken(string token);
    }
}
