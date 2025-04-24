namespace AdminService.Business.Jwt.Token
{
    public interface ITokenStoreHandler
    {
        void SaveToken(string userId, string token);
        bool IsTokenValid(string userId, string token);
        void RevokeToken(string userId);
    }
}
