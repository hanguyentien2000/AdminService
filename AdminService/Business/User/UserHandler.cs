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

        public async Task<Response<IdmUsers>> CreateUserAsync(UserModel model)
        {
            var userCreate = new IdmUsers();
            userCreate.UserId = Guid.NewGuid();
            userCreate.UserName = model.UserName;
            userCreate.FullName = model.FullName;
            userCreate.Address = model.Address;
            userCreate.Email = model.Email;
            userCreate.OtherEmail = model.OtherEmail;
            userCreate.IdentityNumber = model.IdentityNumber;
            userCreate.Birthday = model.Birthday;
            userCreate.Gender = model.Gender;
            userCreate.MobilePin = model.MobilePin;
            userCreate.NickName = model.NickName;
            userCreate.OtherEmail = model.OtherEmail;
            userCreate.Avatar = model.Avatar;

            userCreate.IdmUsersInRoles = model.IdmUsersInRoles;
            foreach (var item in model.IdmUsersInRoles)
            {
                var uio = new IdmUsersInRoles();
                uio.Id = Guid.NewGuid();
                uio.UserId = userCreate.UserId;
                uio.RoleId = item.RoleId;
                uio.CreateDate = DateTime.Now;
                var repoUio = _unitOfWork.GetRepository<IdmUsersInRoles>();
                await repoUio.AddAsync(uio);
            }
            userCreate.CreatedByFullName = model.CreatedByFullName;
            userCreate.CreatedByUserId = model.CreatedByUserId;
            userCreate.CreatedOnDate = DateTime.Now;

            var repo = _unitOfWork.GetRepository<IdmUsers>();
            await repo.AddAsync(userCreate);
            if (await _unitOfWork.CommitAsync() > 0)
                return Response<IdmUsers>.Ok(userCreate, "User created successfully");
            else
                return Response<IdmUsers>.Fail("Failed to create User");
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
