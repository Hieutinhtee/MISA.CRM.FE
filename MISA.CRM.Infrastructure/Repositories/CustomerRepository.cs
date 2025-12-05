using Dapper;
using MISA.CRM.CORE.Entities;
using MISA.CRM.CORE.Interfaces.Repositories;
using MISA.CRM.Infrastructure.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MySqlConnectionFactory factory)
        : base(factory)
        {
        }

        //public async Task<Customer?> GetByCustomerCodeAsync(string customerCode)
        //{
        //    var sql = "SELECT * FROM Customer WHERE CustomerCode = @code LIMIT 1";
        //    using var conn = Connection;
        //    return await conn.QueryFirstOrDefaultAsync<Customer>(sql, new { code = customerCode });
        //}

        protected override HashSet<string> GetSortableFields()
        {
            return new HashSet<string>
            {
                "CustomerCode",
                "CustomerName",
                "CreatedDate"
            };
        }
    }
}