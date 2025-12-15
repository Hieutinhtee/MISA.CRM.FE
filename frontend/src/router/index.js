import { createRouter, createWebHistory } from 'vue-router'

//#region Import Views
// Import các Views
import CustomerList from '../views/customer/CustomerList.vue'
import CustomerAdd from '../views/customer/CustomerAdd.vue'
import CustomerEdit from '../views/customer/CustomerEdit.vue'
//#endregion

//#region Route Definitions
/**
 * Mảng định nghĩa các route của ứng dụng.
 * @type {import('vue-router').RouteRecordRaw[]}
 */
const routes = [
    /**
     * Route gốc: Chuyển hướng về trang danh sách khách hàng.
     * @path /
     */
    {
        path: '/',
        redirect: '/customer/list',
    },

    /**
     * Trang Danh sách Khách hàng.
     * @path /customer/list
     */
    {
        path: '/customer/list',
        name: 'customerList', // Thêm name để dễ dàng điều hướng
        component: CustomerList,
    },

    /**
     * Trang Thêm mới Khách hàng.
     * @path /customer/add
     */
    {
        path: '/customer/add',
        name: 'customerAdd',
        component: CustomerAdd,
    },

    /**
     * Trang Chỉnh sửa Khách hàng (có tham số ID).
     * @path /customer/edit/:id
     */
    {
        path: '/customer/edit/:id',
        name: 'customerEdit',
        component: CustomerEdit,
        // props: true, // Có thể thêm props: true để biến param thành props
    },
]
//#endregion

//#region Router Instance
/**
 * Tạo instance Router.
 * Sử dụng createWebHistory cho chế độ history (URL sạch).
 * @type {import('vue-router').Router}
 */
const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes,
})
//#endregion

export default router
