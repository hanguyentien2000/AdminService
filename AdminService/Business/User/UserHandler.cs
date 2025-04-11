using AdminService.Insfrastructure;
using Azure;
using DataUtils;

namespace AdminService.Business.User
{
    public partial class UserHandler : IUserHandler
    {
        public UserHandler() { }

        public Task<Response> Create(UserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetAll()
        {
            try
            {
                using var uow = new UnitOfWork(new DatabaseFactory())
                {

                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
