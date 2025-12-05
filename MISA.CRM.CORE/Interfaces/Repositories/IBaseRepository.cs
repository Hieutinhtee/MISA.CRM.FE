using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CRM.CORE.Interfaces.Repositories
{
    /// <summary>
    /// Base repo interface cho CRUD cơ bản
    /// </summary>
    /// Created By: TMHieu (03/12/2025)
    /// <typeparam name="T">Thực thể cần truyền vào VD: customer</typeparam>
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Hàm lấy danh sách tất cả dữ liệu
        /// </summary>
        /// <returns>trả về danh sách tất cả dữ liệu</returns>
        /// Created By: TMHieu (03/12/2025)
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Hàm lấy thông tin chi tiết theo id
        /// </summary>
        /// <param name="id"> id của đối tượng  muốn tìm </param>
        /// <returns>đối tượng có id phù hợp</returns>
        /// Created By: TMHieu (03/12/2025)
        Task<T?> GetById(Guid id);

        /// <summary>
        /// Hàm sắp xếp 1 cột trong bảng
        /// </summary>
        /// <param name="sortField"> tên cột muốn sắp xếp </param>
        /// <param name="asc"> true là tăng dần, false là giảm dần </param>
        /// <returns>đối tượng có id phù hợp</returns>
        /// Created By: TMHieu (03/12/2025)
        Task<List<T>> GetAllSortedAsync(string sortField, bool asc = true);

        /// <summary>
        /// Hàm thêm mới bản ghi trong database
        /// </summary>
        /// <param name="entity">thuộc tính của thực thể muốn thêm</param>
        /// <returns>trả về số dòng bị ảnh hưởng </returns>
        /// Created By: TMHieu (03/12/2025)
        Task<Guid> InsertAsync(T entity);

        /// <summary>
        /// Hàm cập nhật 1 bản ghi theo id
        /// </summary>
        /// <param name="id"> id của bản ghi mình muốn cập nhật</param>
        /// <param name="entity">thuộc tính của thực thể</param>
        /// <returns>Guid của dòng vừa thêm</returns>
        /// Created By: TMHieu (03/12/2025)
        Task<Guid> UpdateAsync(Guid id, T entity);

        /// <summary>
        /// Hàm cập nhật 1 cột thành các giá trị giống nhau của nhiều bản ghi theo id
        /// </summary>
        /// <param name="ids"> danh sách id của bản ghi mình muốn cập nhật</param>
        /// <param name="columnName">Cột cần cập nhật hàng loạt</param>
        /// param name="value">Giá trị mới cần cập nhật</param>
        /// <returns>Guid của dòng vừa thêm</returns>
        /// Created By: TMHieu (03/12/2025)
        Task<int> BulkUpdateSameValueAsync(List<Guid> ids, string columnName, object value);

        /// <summary>
        /// xóa mềm 1 bản ghi trong database
        /// </summary>
        /// <param name="id">id mà mình muốn xóa </param>
        /// <returns>số dòng bị ảnh hưởng </returns>
        /// Created By: TMHieu (03/12/2025)
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Kiểm tra giá trị có tồn tại trong cột (bỏ qua soft delete và ignoreId nếu có).
        /// Created by: TMHieu (05/12/2025)
        /// </summary>
        /// <param name="columnName">Tên cột cần kiểm tra.</param>
        /// <param name="value">Giá trị cần kiểm tra.</param>
        /// <param name="ignoreId">ID cần bỏ qua (tùy chọn).</param>
        /// <returns>True nếu tồn tại, False nếu không.</returns>
        Task<bool> IsValueExistAsync(string columnName, object value, Guid? ignoreId = null);
    }
}