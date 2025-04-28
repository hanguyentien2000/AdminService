using AdminService.Business.User;
using AdminService.Insfrastructure.Databases;
using DataUtils;
namespace AdminService.Business.Roles
{
    public class RoleHandler : IRoleHandler
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleHandler(IDatabaseFactory factory)
        {
            _unitOfWork = new UnitOfWork(factory);
        }

        public async Task<Response<IdmRoles>> GetRolesByIdAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<IdmRoles>();
            var User = await repo.GetByIdAsync(id);
            return User is null
       ? Response<IdmRoles>.Fail("Role not found")
       : Response<IdmRoles>.Ok(User);
        }

        public async Task<Response<IdmRoles>> CreateRoleAsync(RoleModel model)
        {
            var roleCreate = new IdmRoles();
            roleCreate.RoleId = Guid.NewGuid();
            roleCreate.RoleName = model.RoleName;
            roleCreate.RoleCode = model.RoleCode;
            roleCreate.LoweredRoleName = model.LoweredRoleName;
            roleCreate.CreatedByUserId = model.CreatedByUserId;
            roleCreate.CreatedOnDate = DateTime.Now;
            roleCreate.Description = model.Description;
            roleCreate.EnableDelete = model.EnableDelete;
            roleCreate.LastModifiedByFullName = model.LastModifiedByFullName;
            roleCreate.LastModifiedByUserId = model.LastModifiedByUserId;
            roleCreate.LastModifiedOnDate = model.LastModifiedOnDate;
            var repo = _unitOfWork.GetRepository<IdmRoles>();
            await repo.AddAsync(roleCreate);
            if (await _unitOfWork.CommitAsync() > 0)
                return Response<IdmRoles>.Ok(roleCreate, "User created successfully");
            else
                return Response<IdmRoles>.Fail("Failed to create User");
        }
    }
}
