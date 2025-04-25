using AdminService.Insfrastructure.Databases;
using System.Threading.Tasks;
using DataUtils;
namespace AdminService.Business.Roles
{
    public interface IRoleHandler
    {
        Task<Response<IdmRoles>> GetRolesByIdAsync(Guid id);
        Task<Response<IdmRoles>> CreateRoleAsync(RoleModel model);
    }
}
