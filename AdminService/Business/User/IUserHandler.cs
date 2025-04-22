using AdminService.Insfrastructure.Databases;
using DataUtils;

namespace AdminService.Business.User
{
    public interface IUserHandler 
    {
        Task<Response<IEnumerable<IdmUsers>>> GetAllUsersAsync();
        Task<Response<IdmUsers>> GetUserByIdAsync(Guid id);
        Task<Response<UserModel>> CreateUserAsync(UserModel User);
        Task<Response<UserModel>> UpdateUserAsync(UserModel User);
        Task<Response<bool>> DeleteUserAsync(Guid UserId);
    }
}
