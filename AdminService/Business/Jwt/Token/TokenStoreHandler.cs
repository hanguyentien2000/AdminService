namespace AdminService.Business.Jwt.Token
{
    public class TokenStoreHandler : ITokenStoreHandler
    {
        private readonly Dictionary<string, string> _tokens = new();

        public void SaveToken(string userId, string token)
        {
            _tokens[userId] = token;
        }

        public bool IsTokenValid(string userId, string token)
        {
            return _tokens.TryGetValue(userId, out var storedToken) && storedToken == token;
        }

        public void RevokeToken(string userId)
        {
            _tokens.Remove(userId);
        }
    }
}
