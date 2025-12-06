using MISA.CRM.CORE.Exceptions;
using MISA.CRM.CORE.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.CORE.Services
{
    public class BaseServices<T> where T : class
    {
        protected readonly IBaseRepository<T> _repo;

        protected BaseServices(IBaseRepository<T> repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Kiểm tra trùng theo cột
        /// </summary>
        protected async Task CheckDuplicateAsync(string columnName, object value, Guid? ignoreId = null)
        {
            var exists = await _repo.IsValueExistAsync(columnName, value, ignoreId);
            if (exists)
                throw new ValidateException($"{columnName} đã tồn tại", $"Giá trị '{value}' ở cột {columnName} đã tồn tại");
        }

        /// <summary>
        /// Kiểm tra tồn tại theo id
        /// </summary>
        protected async Task<T> EnsureExistsAsync(Guid id)
        {
            var data = await _repo.GetById(id);
            if (data == null)
                throw new NotFoundException("Không tìm thấy dữ liệu", $"{typeof(T).Name} không tồn tại");
            return data;
        }

        /// <summary>
        /// Validate custom (cho lớp con override)
        /// </summary>
        protected virtual Task ValidateAsync(T entity, Guid? id = null)
        {
            return Task.CompletedTask;
        }

        public virtual Task<List<T>> GetAllAsync() => _repo.GetAllAsync();

        public virtual Task<T> GetByIdAsync(Guid id) => EnsureExistsAsync(id);

        public virtual async Task<Guid> CreateAsync(T entity)
        {
            await ValidateAsync(entity, null);
            return await _repo.InsertAsync(entity);
        }

        public virtual async Task<Guid> UpdateAsync(Guid id, T entity)
        {
            await EnsureExistsAsync(id);
            await ValidateAsync(entity, id);
            return await _repo.UpdateAsync(id, entity);
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            await EnsureExistsAsync(id);
            return await _repo.DeleteAsync(id);
        }
    }
}