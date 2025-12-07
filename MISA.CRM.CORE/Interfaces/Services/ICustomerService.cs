using MISA.CRM.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.CORE.Interfaces.Services
{
    public interface ICustomerService : IBaseService<Customer>
    {
        /// <summary>
        /// Lấy mã khách hàng tiếp theo
        /// Created by: TMHieu (07/12/2025)
        /// </summary>
        /// <returns>Mã khách hàng tiếp theo</returns>
        Task<string> NextCodeAsync();
    }
}