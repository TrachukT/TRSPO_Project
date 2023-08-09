namespace TFSport.Services.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateAccessTokenAsync(string email);
        Task<string> GenerateRefreshTokenAsync();
    }
}
