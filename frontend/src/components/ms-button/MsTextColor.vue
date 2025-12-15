<template>
    <div :class="['text-color', `text-color--${type}`]">
        <slot></slot>
    </div>
</template>

<script setup>
// Imports
import { defineProps } from 'vue'

//#region Props
const props = defineProps({
    /** Loại màu text: 'primary' hoặc 'second' */
    type: {
        type: String,
        default: 'second', // Mặc định là 'second' (xanh đậm hơn)
    },
})
//#endregion

// Component này chỉ là wrapper/styling, không có logic phức tạp trong script.
</script>

<style scoped>
/* Scoped Styles */

.text-color {
    cursor: pointer;
    font-size: 13px;
}

/* Màu Primary (thường là màu chủ đạo của hệ thống) */
.text-color--primary {
    color: var(--primary-color);
}

/* Màu Second (màu xanh đậm/blue, thường dùng cho link) */
.text-color--second {
    color: #0f2fbd;
}

/* Hiệu ứng khi hover */
.text-color:hover {
    text-decoration: underline;
}
</style>
