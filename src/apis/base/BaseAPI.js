import api from '@/apis/config/APIConfig.js';

export default class BaseAPI {
    constructor() {
        this.controller = null;
    }
    /**
     * Phương thức lấy tất cả dữ liệu
     * createdby: TMHieu - 6.12.2025
     */
    getAll() {
        return api.get(`${this.controller}`);
    }
    /**
     * Hàm lấy dữ liệu phân trang
     * @param {*} payload 
     * createdby: TMHieu - 6.12.2025
     */
    paging(payload) {
        return api.get(`${this.controller}/paging`, { params: payload });
    }
    /**
     * Hàm cập nhật dữ liệu
     * @param {*} id 
     * @param {*} body 
     * createdby: TMHieu - 6.12.2025
     */
    update(id, body) {
        return api.put(`${this.controller}/update/${id}`, body);
    }
    /**
     * Hàm xóa bản ghi
     * @param {*} id 
     * createdby: TMHieu - 6.12.2025
     */
    delete(id) {
        return api.delete(`${this.controller}/delete/${id}`);
    }
}