import api from '@/apis/config/APIConfig.js';
import BaseAPI from '@/apis/base/BaseAPI.js';

class CustomersAPI extends BaseAPI {
   constructor() {
      super();
      this.controller = "Customers";
   }

   /**
    * Hàm kiểm tra trùng lặp theo nghiệp vụ riêng
    * @param {*} payload 
    * @returns 
    * createdby: TMHieu - 6.12.2025
    */
   checkDuplicate(payload) {
      return api.post(`${this.controller}/check-duplicate`, payload);
   }

   /**
    * Hàm lấy mã khách hàng tự sinh
    * @returns {string} mã khách hàng
    * createdby: TMHieu - 6.12.2025
    */
   getNextCustomerCode() {
      return api.get(`${this.controller}/next-code`);
   }

   /**
    * Hàm gửi excel để import các bản ghi
    * @returns {string} mã khách hàng
    * createdby: TMHieu - 9.12.2025
    */
   excelImport(formData) {
      api.post(`${this.controller}/import`, formData, {
         headers: {
            "Content-Type": "multipart/form-data"
         }
      });
   }

   /**
    * Hàm xóa hàng loạt khách hàng
    * @returns {int} số bản ghi khách hàng xóa thành công
    * createdby: TMHieu - 9.12.2025
    */
   deleteCustomer(payload) {
      return api.put(`${this.controller}/soft-delete-many`, payload);
   }


}

export default new CustomersAPI();