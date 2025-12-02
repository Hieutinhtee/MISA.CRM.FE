<template>
    <div class="header d-flex flex-row justify-content-between">
        <!-- header -->
        <div class="d-flex align-content-center">

            <div class="icon-logo"></div>
            <div class="m-l-12 title title-name">CRM</div>

            <div class="tabs-list-item d-flex">
                <div v-for="(item, index) in tabsListItem" class="tabs-item d-flex" :key="index"
                    :class="item.active ? 'active' : ''">
                    <div class="tabs-icon" :class="[item.icon ? 'icon-' + item.icon : '']"></div>

                    <div class="tabs-title" :class="item.active ? 'tabs-title-active' : ''">{{ item.title }}</div>
                    <div v-if="item.active" class="under-line-tabs-active"></div>
                </div>
            </div>
        </div>
        <!-- Avatar user -->
        <div class="img-avt m-r-24"></div>

    </div>
</template>

<script setup>
const tabsListItem = [
    { icon: 'home', title: 'Bàn làm việc' },
    { icon: 'headquarters', title: 'Khách hàng', active: true },
    { icon: 'wallet', title: 'Cơ hội' },
    { icon: 'quote', title: 'Báo giá' },
    { icon: 'cart', title: 'Đơn hàng' },
    { icon: 'box', title: 'Hàng hóa' },
    { icon: 'chart', title: 'Báo cáo' },
    { icon: 'hamburger', title: 'Tất cả' },
]
</script>

<style>
.header {
    padding: 8px 0;
    box-shadow: 0 2px 4px #1f222929;
    z-index: 1;
}

.title {
    font-size: 18px;
    font-weight: 700;
}


/* icon */


/* tabs */
.tabs-list-item {
    margin-left: 16px;
}

.tabs-icon {
    margin-right: 8px;
}

.tabs-title {
    font-size: 13px;
}

.tabs-title-active {
    color: var(--primary-color);
}

.tabs-item {
    padding: 8px 8px;
    border-radius: 4px;
    position: relative;
    cursor: pointer;
}

.tabs-item:hover {
    background-color: #f0f2f4;
}

.tabs-item.active:hover,
.tabs-item.active {
    background-color: transparent;
}

.under-line-tabs-active {

    /* width: 100%; */
    height: 2px;
    position: absolute;
    bottom: -8px;
    left: 8px;
    right: 8px;
    background-color: var(--primary-color);
}
</style>
