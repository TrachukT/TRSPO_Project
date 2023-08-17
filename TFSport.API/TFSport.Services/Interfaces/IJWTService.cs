namespace TFSport.Services.Interfaces
{
	public interface IJWTService
	{
		public Task<string> GenerateAccessTokenAsync(string email,string id);

		public Task<string> GenerateRefreshTokenAsync(string email, string id);

		public Task<string> GetEmailFromToken(string token);
		public Task<string> GetIdFromToken(string token);
    }
}
