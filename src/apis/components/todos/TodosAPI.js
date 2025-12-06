import api from '@/apis/config/APIConfig.js';
import BaseAPI from '@/apis/base/BaseAPI.js';

class TodosAPI extends BaseAPI {
    constructor(){
        super();
        this.controler = "todos";
    }

    /**
     * Hàm kiểm tra trùng lặp theo nghiệp vụ riêng
     * @param {*} payload 
     * @returns 
     */
    checkDuplicate(payload) {
        return api.post(`${this.controller}/check-duplicate`, payload);
    }
}

export default new TodosAPI();