using AdminService.Insfrastructure.Databases;
using DataUtils;
namespace AdminService.Business.User
{
    public partial class UserHandler : IUserHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserHandler(IDatabaseFactory factory)
        {
            _unitOfWork = new UnitOfWork(factory);
        }

        public async Task<Response<IEnumerable<IdmUsers>>> GetAllUsersAsync()
        {
            var repo = _unitOfWork.GetRepository<IdmUsers>();
            var result = await repo.GetAllAsync();
            return Response<IEnumerable<IdmUsers>>.Ok(result.ToList());
        }
        public async Task<Response<IdmUsers>> GetUserByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<IdmUsers>();
            var User = await repo.GetByIdAsync(id);
            return User is null
       ? Response<IdmUsers>.Fail("User not found")
       : Response<IdmUsers>.Ok(User);
        }

        public async Task<Response<UserModel>> CreateUserAsync(UserModel User)
        {
            var repo = _unitOfWork.GetRepository<UserModel>();
            await repo.AddAsync(User);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
       ? Response<UserModel>.Ok(User, "User created successfully")
       : Response<UserModel>.Fail("Failed to create User");
        }

        public async Task<Response<UserModel>> UpdateUserAsync(UserModel User)
        {
            var repo = _unitOfWork.GetRepository<UserModel>();
            repo.Update(User);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? Response<UserModel>.Ok(User, "User updated successfully")
                : Response<UserModel>.Fail("Failed to update User");
        }

        public async Task<Response<bool>> DeleteUserAsync(Guid UserId)
        {
            var repo = _unitOfWork.GetRepository<UserModel>();
            var User = await repo.GetByIdAsync(UserId);
            if (User == null)
                return Response<bool>.Fail("User not found");

            repo.Delete(User);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? Response<bool>.Ok(true, "User deleted")
                : Response<bool>.Fail("Failed to delete User");
        }
    }
}
