using DataUtils;

namespace AdminService.Business.User
{
    public class UserModel : IEntity
    {
        public Guid Id { get; set; } 
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
