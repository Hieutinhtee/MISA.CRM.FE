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
     * Phương thức lấy dữ liệu theo guid
     * createdby: TMHieu - 6.12.2025
     */
    getById(id) {
        return api.get(`${this.controller}/${id}`);
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
        return api.put(`${this.controller}/${id}`, body);
    }

    /**
     * Hàm thêm mới bản ghi dữ liệu
     * @param {*} body 
     * createdby: TMHieu - 6.12.2025
     */
    create(body) {
        return api.post(`${this.controller}/`, body);
    }

    /**
     * Hàm xóa 1 hoặc nhiều bản ghi dữ liệu
     * @param {*} body 
     * createdby: TMHieu - 7.12.2025
     */
    delete(body) {
        return api.post(`${this.controller}/bulk-update`, body);
    }

}