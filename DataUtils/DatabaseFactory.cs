using Microsoft.EntityFrameworkCore;

namespace DataUtils
{
    //DatabaseFactory là một lớp chịu trách nhiệm tạo ra và quản lý vòng đời của
    //DbContext(hoặc một đối tượng tương đương như session trong Hibernate).
    //Mục tiêu chính của nó là đảm bảo rằng trong một đơn vị công việc(unit of work),
    //sử dụng một thể hiện duy nhất của DbContext, từ đó tránh lỗi và tăng hiệu suất.
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly DbContext _dbContext;

        public DatabaseFactory(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext GetDbContext() => _dbContext;
    }
}
