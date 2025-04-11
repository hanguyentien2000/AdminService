using Azure;

namespace AdminService.Business.User
{
    public interface IUserHandler 
    {
        Task<Response> GetAll();
        Task<Response> Create(UserModel model);
    }
}
