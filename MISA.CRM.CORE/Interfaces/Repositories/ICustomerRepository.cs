using MISA.CRM.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.CORE.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// Lấy mã lớn nhất hiện tại trong database
        /// Created by: TMHieu (07/12/2025)
        /// </summary>
        /// <returns>Mã cuối trong db</returns>
        Task<string?> GetLastCodeAsync();
    }
}