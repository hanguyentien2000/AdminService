using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUtils
{
    /// <summary>
    // UnitOfWork là một mẫu thiết kế dùng để gom nhóm nhiều thay đổi vào cùng một giao dịch, giúp:

    // Tăng tính nhất quán dữ liệu.

    // Quản lý commit/rollback dễ dàng.

    // Tách biệt logic nghiệp vụ và truy cập dữ liệu.

    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> CommitAsync();
    }
}
