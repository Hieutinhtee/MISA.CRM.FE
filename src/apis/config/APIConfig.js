import axios from 'axios';
import { useToastMessage } from '@/composables/useToastMessage';
const { showToastSuccess, showToastError, showToastInfo } = useToastMessage();

const baseURL = "https://localhost:7140/api/v1";

let api = axios.create({
    baseURL: baseURL,
    headers: {
        'Content-Type': 'application/json'
    }
})

// Interceptor bắt lỗi chung
api.interceptors.response.use(
    (response) => response,
    (error) => {
        const res = error?.response;

        // Không nhận được response -> lỗi kết nối
        if (!res) {
            showToastError("Không thể kết nối đến máy chủ");
            return Promise.reject(error);
        }

        // ASP.NET Core trả lỗi mặc định không qua middleware
        if (res.status === 400 && res.data?.errors) {
            let messages = [];

            for (let key in res.data.errors) {
                messages.push(...res.data.errors[key]);
            }

            showToastError(messages.join("\n"));
            return Promise.reject(error);
        }

        // Lỗi custom từ middleware BE
        // BE trả về: { error: { userMsg, devMsg, traceId } }
        if (res.data?.error) {
            const userMsg = res.data.error.userMsg
                || "Có lỗi xảy ra, vui lòng thử lại";

            showToastError(userMsg);


            return Promise.reject(error);
        }

        // 404 – NotFound
        if (res.status === 404) {
            showToastError("Không tìm thấy tài nguyên yêu cầu");
            return Promise.reject(error);
        }

        // 500 – Internal server error
        if (res.status >= 500) {
            showToastError("Lỗi hệ thống, vui lòng thử lại sau");
            return Promise.reject(error);
        }

        // Lỗi không xác định
        showToastError("Đã xảy ra lỗi không xác định");
        return Promise.reject(error);
    }
);


export default api;