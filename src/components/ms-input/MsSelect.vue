<template>
    <a-select
        show-search
        :options="props.options"
        :filter-option="filterOption"
        :placeholder="placeholder || label"
        @change="onChange"
    />
</template>

<script setup>
// Imports & Init
import { defineEmits } from 'vue'

//#region Props & Emits
const props = defineProps({
    /** Label hiển thị nếu placeholder không được cung cấp */
    label: String,
    /** Placeholder của input */
    placeholder: String,
    /** Danh sách options, format: [{ value: '...', label: '...' }] */
    options: {
        type: Array,
        default: () => [],
    },
})

const emit = defineEmits(['change'])
//#endregion

//#region Methods - Handling & Logic

/**
 * Hàm lọc mặc định cho Ant Design Select, dùng cho tìm kiếm
 * So sánh input của người dùng với giá trị (value) của option (không phân biệt chữ hoa/thường).
 * @param {string} input - Giá trị nhập vào của người dùng.
 * @param {Object} option - Đối tượng option đang được kiểm tra ({ value: string, label: string }).
 * @returns {boolean} True nếu option match với input.
 * createdby: TMHieu - 09.12.2025
 */
const filterOption = (input, option) => {
    // Đảm bảo option.value tồn tại và chuyển sang chữ thường để so sánh
    return option.value?.toLowerCase().includes(input.toLowerCase())
}

/**
 * Xử lý sự kiện khi giá trị Select thay đổi.
 * @param {*} value - Giá trị mới được chọn.
 * createdby: TMHieu - 09.12.2025
 */
function onChange(value) {
    emit('change', value)
}
//#endregion
</script>

<style scoped>
.ant-select {
    height: 32px;
    flex: 1 1 550px;
    min-width: 0px;
    transition: all 0.2s;
    border-radius: 4px;
}

.ant-select-selector {
    border-radius: 4px !important;
}
</style>
