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

        public UserHandler(DbContextOptions<AdminDataContext> options)
        {
            // Khởi tạo thủ công các thành phần dùng chung
            var context = new AdminDataContext(options);
            var factory = new DatabaseFactory(context);
            _unitOfWork = new UnitOfWork(factory);
        }

        public async Task<DataUtils.Response<IEnumerable<UserModel>>> GetAllCustomersAsync()
        {
            var repo = _unitOfWork.Repository<UserModel>();
            var result = await repo.GetAllAsync();
            return DataUtils.Response<IEnumerable<UserModel>>.Ok(result.ToList());
        }
        public async Task<DataUtils.Response<UserModel>> GetCustomerByIdAsync(Guid id)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            var customer = await repo.GetByIdAsync(id);
            return customer is null
       ? DataUtils.Response<UserModel>.Fail("Customer not found")
       : DataUtils.Response<UserModel>.Ok(customer);
        }

        public async Task<DataUtils.Response<UserModel>> CreateCustomerAsync(UserModel customer)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            await repo.AddAsync(customer);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
       ? DataUtils.Response<UserModel>.Ok(customer, "Customer created successfully")
       : DataUtils.Response<UserModel>.Fail("Failed to create customer");
        }

        public async Task<DataUtils.Response<UserModel>> UpdateCustomerAsync(UserModel customer)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            repo.Update(customer);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? DataUtils.Response<UserModel>.Ok(customer, "Customer updated successfully")
                : DataUtils.Response<UserModel>.Fail("Failed to update customer");
        }

        public async Task<DataUtils.Response<bool>> DeleteCustomerAsync(Guid customerId)
        {
            var repo = _unitOfWork.Repository<UserModel>();
            var customer = await repo.GetByIdAsync(customerId);
            if (customer == null)
                return DataUtils.Response<bool>.Fail("Customer not found");

            repo.Delete(customer);
            var success = await _unitOfWork.CommitAsync() > 0;
            return success
                ? DataUtils.Response<bool>.Ok(true, "Customer deleted")
                : DataUtils.Response<bool>.Fail("Failed to delete customer");
        }
    }
}
