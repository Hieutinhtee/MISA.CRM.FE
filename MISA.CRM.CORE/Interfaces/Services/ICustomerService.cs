using MISA.CRM.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.CORE.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();

        Task<List<Customer>> GetAllSortedAsync(string sortField, bool asc = true);

        Task<Customer> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(Customer customer);

        Task<int> DeleteAsync(Guid id);

        Task<Guid> UpdateAsync(Guid id, Customer customer);
    }
}