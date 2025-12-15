<template>
    <div class="header d-flex flex-row justify-content-between">
        <div class="d-flex align-content-center">
            <div class="icon-logo"></div>

            <div class="m-l-12 title title-name">CRM</div>

            <div class="tabs-list-item d-flex">
                <div
                    v-for="(item, index) in tabsListItem"
                    class="tabs-item d-flex"
                    :key="index"
                    :class="item.active ? 'active' : ''"
                >
                    <div class="tabs-icon" :class="[item.icon ? 'icon-' + item.icon : '']"></div>

                    <div class="tabs-title" :class="item.active ? 'tabs-title-active' : ''">
                        {{ item.title }}
                    </div>

                    <div v-if="item.active" class="under-line-tabs-active"></div>
                </div>
            </div>
        </div>
        <div class="img-avt m-r-24"></div>
    </div>
</template>

<script setup>
import { ref } from 'vue'

//#region Data
/**
 * Danh sách các mục (tabs) trên Header.
 * @type {Array<Object>}
 * @property {string} icon - Tên class icon.
 * @property {string} title - Tên hiển thị.
 * @property {boolean} [active] - Trạng thái active (mặc định là false).
 */
const tabsListItem = ref([
    { icon: 'home', title: 'Bàn làm việc' },
    { icon: 'headquarters', title: 'Khách hàng', active: true },
    { icon: 'wallet', title: 'Cơ hội' },
    { icon: 'quote', title: 'Báo giá' },
    { icon: 'cart', title: 'Đơn hàng' },
    { icon: 'box', title: 'Hàng hóa' },
    { icon: 'chart', title: 'Báo cáo' },
    { icon: 'hamburger', title: 'Tất cả' },
])
//#endregion

// Nếu cần thêm logic xử lý chuyển tab, bạn có thể thêm hàm ở đây
// Ví dụ: function handleTabClick(item) { ... }
</script>

<style scoped>
/* Scoped Styles */

.header {
    padding: 8px 0;
    box-shadow: 0 2px 4px #1f222929;
    z-index: 1;
}

.title {
    font-size: 18px;
    font-weight: 700;
}

/* --- Tabs Styling --- */
.tabs-list-item {
    margin-left: 16px;
}

.tabs-icon {
    margin-right: 8px;
}

.tabs-title {
    font-size: 13px;
    line-height: 16px; /* Giúp căn giữa tốt hơn */
}

.tabs-title-active {
    color: var(--primary-color);
}

.tabs-item {
    padding: 8px 8px;
    border-radius: 4px;
    position: relative;
    cursor: pointer;
    align-items: center; /* Đảm bảo icon và text được căn giữa */
}

.tabs-item:hover {
    background-color: #f0f2f4;
}

/* Tab đang active không thay đổi background khi hover */
.tabs-item.active:hover,
.tabs-item.active {
    background-color: transparent;
}

.under-line-tabs-active {
    height: 2px;
    position: absolute;
    bottom: -8px; /* Đặt dưới header */
    left: 8px;
    right: 8px;
    background-color: var(--primary-color);
}

/* Giả lập Avatar user */
.img-avt {
    width: 32px;
    height: 32px;
    background-color: #a0a4a8; /* Màu nền giả định */
    border-radius: 50%;
    /* Thêm image/avatar thật nếu cần */
}
</style>
