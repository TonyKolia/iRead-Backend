namespace iRead.API.Utilities.Interfaces
{
    public interface IAuthenticationUtilities
    {
        string HashPassword(string password, out string salt);
        public bool VerifyPasswordHash(string password, string passwordHash, string salt);
        string GenerateToken(User user);
    }
}
