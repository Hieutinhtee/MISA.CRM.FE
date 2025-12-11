import { useToast } from 'vue-toastification'

/**
 * @fileoverview Composable để quản lý việc hiển thị thông báo toast (popup).
 * Sử dụng thư viện 'vue-toastification'.
 * createdby: TMHieu - 09.12.2025
 */

/**
 * Composable cung cấp các hàm tiện ích để hiển thị các loại thông báo toast khác nhau.
 * @returns {{showToastSuccess: Function, showToastError: Function, showToastInfo: Function}}
 */
export function useToastMessage() {
    // #region Initialization
    const toast = useToast()

    /**
     * Cấu hình mặc định cho tất cả các loại toast.
     * @type {Object}
     */
    const defaultOptions = {
        position: 'top-center',
        timeout: 3000, // Hiển thị trong 3 giây
    }
    // #endregion

    // #region Toast Functions

    /**
     * Hiển thị thông báo thành công.
     * @param {string} message - Nội dung thông báo.
     * createdby: TMHieu - 09.12.2025
     */
    function showToastSuccess(message) {
        toast.success(message, defaultOptions)
    }

    /**
     * Hiển thị thông báo lỗi.
     * @param {string} message - Nội dung thông báo.
     * createdby: TMHieu - 09.12.2025
     */
    function showToastError(message) {
        toast.error(message, defaultOptions)
    }

    /**
     * Hiển thị thông báo thông tin/cảnh báo nhẹ.
     * LƯU Ý: Hàm gốc của bạn đang gọi toast.error, đã được sửa thành toast.info.
     * @param {string} message - Nội dung thông báo.
     * createdby: TMHieu - 09.12.2025
     */
    function showToastInfo(message) {
        toast.info(message, defaultOptions)
    }

    // #endregion

    return {
        showToastSuccess,
        showToastError,
        showToastInfo,
    }
}
