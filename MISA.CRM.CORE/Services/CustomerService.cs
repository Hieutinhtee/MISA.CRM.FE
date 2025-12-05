using MISA.CRM.CORE.Entities;
using MISA.CRM.CORE.Exceptions;
using MISA.CRM.CORE.Interfaces.Repositories;
using MISA.CRM.CORE.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.CORE.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> CreateAsync(Customer customer)
        {
            await ValidateCustomerAsync(customer);

            return await _repo.InsertAsync(customer);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var customer = await _repo.GetById(id);
            if (customer == null)
                throw new NotFoundException("Customer not found", "Không tìm thấy khách hàng");

            return await _repo.DeleteAsync(id);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<List<Customer>> GetAllSortedAsync(string sortField, bool asc = true)
        {
            var data = await _repo.GetAllAsync();

            var prop = typeof(Customer).GetProperty(sortField);
            if (prop == null)
                throw new ValidateException("Invalid sort field", "Trường sắp xếp không hợp lệ");

            return asc
                ? data.OrderBy(x => prop.GetValue(x)).ToList()
                : data.OrderByDescending(x => prop.GetValue(x)).ToList();
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            var customer = await _repo.GetById(id);

            if (customer == null)
                throw new NotFoundException("Customer not found", "Không tìm thấy khách hàng");

            return customer;
        }

        public async Task<Guid> UpdateAsync(Guid id, Customer customer)
        {
            var oldCustomer = await _repo.GetById(id);
            if (oldCustomer == null)
                throw new NotFoundException("Customer not found", "Không tìm thấy khách hàng");

            await ValidateCustomerAsync(customer, id);

            customer.CustomerId = id;

            return await _repo.UpdateAsync(id, customer);
        }

        private async Task ValidateCustomerAsync(Customer customer, Guid? ignoreId = null)
        {
            if (customer == null)
                throw new ValidateException("Customer object is null", "Dữ liệu khách hàng không được để trống");

            // 1. Full name
            if (string.IsNullOrWhiteSpace(customer.FullName))
                throw new ValidateException("FullName is required", "Tên khách hàng không được để trống");

            // 2. Phone validate: chỉ số, 10-11 digits
            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new ValidateException("Phone required", "Số điện thoại không được để trống");

            if (!customer.Phone.All(char.IsDigit))
                throw new ValidateException("Phone invalid", "Số điện thoại chỉ được chứa chữ số");

            if (customer.Phone.Length < 10 || customer.Phone.Length > 11)
                throw new ValidateException("Phone length invalid", "Số điện thoại phải dài 10-11 số");

            // 3. Email validate bằng regex
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ValidateException("Email required", "Email không được để trống");

            var emailRegex = @"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,4}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(customer.Email, emailRegex))
                throw new ValidateException("Invalid email format", "Email không đúng định dạng");

            // 4. Check unique - dùng repo
            if (await _repo.IsValueExistAsync(nameof(Customer.Phone), customer.Phone, ignoreId))
                throw new ValidateException("Phone duplicate", "Số điện thoại đã tồn tại");

            if (await _repo.IsValueExistAsync(nameof(Customer.Email), customer.Email, ignoreId))
                throw new ValidateException("Email duplicate", "Email đã tồn tại");
        }
    }
}