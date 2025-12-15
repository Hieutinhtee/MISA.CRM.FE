<template>
    <div :class="['ms-button', `ms-button--${type}`]" class="d-flex">
        <div v-if="iconLeft" :class="[`icon-${iconLeft}`]"></div>

        <div v-if="$slots.default" class="m-l-8 d-flex align-content-center gap12">
            <slot></slot>
            <svg
                v-if="loading"
                width="16"
                height="11"
                fill="#9BACF2FF"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
            >
                <path
                    d="M12,4a8,8,0,0,1,7.89,6.7A1.53,1.53,0,0,0,21.38,12h0a1.5,1.5,0,0,0,1.48-1.75,11,11,0,0,0-21.72,0A1.5,1.5,0,0,0,2.62,12h0a1.53,1.53,0,0,0,1.49-1.3A8,8,0,0,1,12,4Z"
                >
                    <animateTransform
                        attributeName="transform"
                        type="rotate"
                        dur="0.75s"
                        values="0 12 12;360 12 12"
                        repeatCount="indefinite"
                    />
                </path>
            </svg>
        </div>

        <div v-if="iconRight" :class="[`icon-${iconRight}`]" class="m-l-8"></div>
    </div>
</template>

<script setup>
// Imports
import { defineProps } from 'vue'

//#region Props
const props = defineProps({
    /** Class icon bên trái */
    iconLeft: {
        type: String,
        default: null,
    },
    /** Class icon bên phải */
    iconRight: {
        type: String,
        default: null,
    },
    /** Loại button: 'primary', 'danger', 'outline', 'outline-primary' */
    type: {
        type: String,
        default: null,
    },
    /** Trạng thái loading, hiển thị icon loading */
    loading: {
        type: Boolean,
        default: false,
    },
})
//#endregion

// Component này chỉ là wrapper/styling, không có logic phức tạp trong script.
</script>

<style scoped>
/* Scoped Styles */

.ms-button {
    padding: 7px 16px 8px 8px;
    border-radius: 4px;
    cursor: pointer;
    font-size: 13px;
    transition: all 0.3s ease;
    /* Dùng d-flex ở đây để căn chỉnh nội dung */
}

/* Các loại Button */

/* Primary */
.ms-button--primary {
    background-color: var(--primary-color);
    color: #fff;
}

.ms-button--primary:hover {
    background-color: #2b4eee;
}

/* Danger */
.ms-button--danger {
    background-color: #f9373a;
    color: #fff;
}

.ms-button--danger:hover {
    background-color: #fe5356;
}

/* Outline (Secondary) */
.ms-button--outline {
    background-color: #ffffff;
    padding: 7px 9px;
    border: 1px solid #d3d7de;
    color: #1f2229b6; /* Thêm màu chữ mặc định cho outline */
}

.ms-button--outline:hover {
    background-color: #f0f2f4;
}

/* Outline Primary */
.ms-button--outline-primary {
    background-color: #fff;
    padding: 7px 16px 7px 8px;
    border: 1px solid var(--primary-color);
    color: var(--primary-color);
}

.ms-button--outline-primary:hover {
    background-color: #e7ebfd;
}
</style>
