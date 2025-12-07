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


}

export default new CustomersAPI();