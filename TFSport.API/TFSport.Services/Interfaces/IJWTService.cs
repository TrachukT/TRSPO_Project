namespace TFSport.Services.Interfaces
{
	public interface IJWTService
	{
		Task<string> GenerateAccessTokenAsync(string email);
		Task<string> GenerateRefreshTokenAsync();
		public Task<string> GetEmailFromToken(string token);
	}
}
