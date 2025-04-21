using AdminService.Insfrastructure;
using AdminService.Insfrastructure.Databases;
using Azure;
using DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace AdminService.Business.User
{
    public partial class UserHandler : IUserHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserHandler(IDatabaseFactory factory)
        {
            // Khởi tạo thủ công các thành phần dùng chung

            _unitOfWork = new UnitOfWork(factory);
        }

        public async Task<DataUtils.Response<IEnumerable<UserModel>>> GetAllUsersAsync()
        {
            var repo = _unitOfWork.Repository<UserModel>();
            var result = await repo.GetAllAsync();
            return DataUtils.Response<IEnumerable<UserModel>>.Ok(result.ToList());
        }
        public async Task<DataUtils.Response<UserModel>> GetUserByIdAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            var User = await repo.GetByIdAsync(id);
            return User is null
       ? DataUtils.Response<UserModel>.Fail("User not found")
       : DataUtils.Response<UserModel>.Ok(User);
        }

        public async Task<DataUtils.Response<UserModel>> CreateUserAsync(UserModel User)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            await repo.AddAsync(User);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
       ? DataUtils.Response<UserModel>.Ok(User, "User created successfully")
       : DataUtils.Response<UserModel>.Fail("Failed to create User");
        }

        public async Task<DataUtils.Response<UserModel>> UpdateUserAsync(UserModel User)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            repo.Update(User);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? DataUtils.Response<UserModel>.Ok(User, "User updated successfully")
                : DataUtils.Response<UserModel>.Fail("Failed to update User");
        }

        public async Task<DataUtils.Response<bool>> DeleteUserAsync(Guid UserId)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            var User = await repo.GetByIdAsync(UserId);
            if (User == null)
                return DataUtils.Response<bool>.Fail("User not found");

            repo.Delete(User);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? DataUtils.Response<bool>.Ok(true, "User deleted")
                : DataUtils.Response<bool>.Fail("Failed to delete User");
        }
    }
}
