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

        public async Task<Response<IdmUserInRoleModel>> CreateUserAsync(IdmUserInRoleModel model)
        {
            var userInRole = new IdmUsersInRoles();
            userInRole.Id = Guid.NewGuid();
            userInRole.UserId = Guid.NewGuid();
            userInRole.RoleId = model.RoleId;
            userInRole.CreateDate = DateTime.Now;
            userInRole.DeleteDate = null;
            userInRole.User = new IdmUsers();

            userInRole.User.UserName = model.User.UserName;
            userInRole.User.FullName = model.User.FullName;
            userInRole.User.Address = model.User.Address;
            userInRole.User.Email = model.User.Email;
            userInRole.User.OtherEmail = model.User.OtherEmail;
            userInRole.User.IdentityNumber = model.User.IdentityNumber;
            userInRole.User.Birthday = model.User.Birthday;
            userInRole.User.Gender = model.User.Gender;
            userInRole.User.MobilePin = model.User.MobilePin;
            userInRole.User.NickName = model.User.NickName;
            userInRole.User.OtherEmail = model.User.OtherEmail;
            userInRole.User.Avatar = model.User.Avatar;

            userInRole.User.CreatedByFullName = userInRole.User.FullName;
            userInRole.User.CreatedByUserId = Guid.Empty;
            userInRole.User.CreatedOnDate = DateTime.Now;

            var repo = _unitOfWork.GetRepository<IdmUsersInRoles>();
            await repo.AddAsync(userInRole);
            if (await _unitOfWork.CommitAsync() > 0)
                return Response<IdmUserInRoleModel>.Ok(null, "User created successfully");
            else
                return Response<IdmUserInRoleModel>.Fail("Failed to create User");
        }

        public async Task<Response<UserModel>> UpdateUserAsync(UserModel model)
        {
            var repo = _unitOfWork.GetRepository<UserModel>();
            repo.Update(model);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? Response<UserModel>.Ok(model, "User updated successfully")
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
