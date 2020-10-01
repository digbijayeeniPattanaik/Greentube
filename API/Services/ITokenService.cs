namespace API.Services
{
    public interface ITokenService
    {
        string CreateToken(string email, string resetCode);
        Outcome<string> ValidateToken(string token);
    }
}
