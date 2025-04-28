using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataUtils
{
    public interface IDatabaseFactory
    {
        DbContext GetDbContext();
    }
}
    