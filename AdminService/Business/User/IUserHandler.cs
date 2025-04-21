using Azure;

namespace AdminService.Business.User
{
    public interface IUserHandler 
    {
        Task<DataUtils.Response<IEnumerable<UserModel>>> GetAllUsersAsync();
        Task<DataUtils.Response<UserModel>> GetUserByIdAsync(Guid id);
        Task<DataUtils.Response<UserModel>> CreateUserAsync(UserModel User);
        Task<DataUtils.Response<UserModel>> UpdateUserAsync(UserModel User);
        Task<DataUtils.Response<bool>> DeleteUserAsync(Guid UserId);
    }
}
