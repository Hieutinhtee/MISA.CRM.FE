<template>
    <a-space>
        <a-date-picker
            v-model:value="model"
            class="d-block"
            :format="dateFormatList"
            :placeholder="placeholder || label"
        />
    </a-space>
</template>

<script setup>
// Imports & Init
import { defineModel } from 'vue'

//#region Props & Models
const props = defineProps({
    /** Label hiển thị nếu placeholder không được cung cấp */
    label: String,
    /** Placeholder của Date Picker */
    placeholder: String,
})

// Model binding cho v-model
// Lưu ý: v-model của Ant Design DatePicker thường là Dayjs object,
// nhưng nếu muốn tự động chuyển đổi sang String theo format, cần xử lý thêm ở component cha
const model = defineModel('value')
//#endregion

//#region State Data
/**
 * Danh sách các định dạng ngày tháng được chấp nhận.
 */
const dateFormatList = ['DD/MM/YYYY', 'DD/MM/YY']
//#endregion

// Không có logic phức tạp trong component này, chỉ đơn thuần là wrapper.
</script>

<style scoped>
/* Scoped styles cho Ant Design DatePicker (Picker) */

.ant-space {
    height: 32px;
    flex: 1 1 550px;
    min-width: 0px;
    transition: all 0.2s;
}

.ant-space-item {
    width: 100%;
}

.ant-picker {
    border-radius: 4px !important;
}
</style>
