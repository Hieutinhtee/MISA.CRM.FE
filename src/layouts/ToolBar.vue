<template>
    <!-- Tool bar -->
    <div class="toolbar d-flex justify-content-between flex-col">
        <div class="left-bar d-flex align-content-center">
            <ms-button class="show-menu" iconLeft="folder" iconRight="down" type="outline">Tất cả khách hàng</ms-button>
            <ms-text-color type="primary">Sửa</ms-text-color>
            <div class="btn-reload">
                <div class="icon-reload"></div>
            </div>
        </div>
        <div class="right-bar d-flex align-content-center gap8">
            <div class="search-ai d-flex align-content-center">
                <div class="icon-search-ai"></div>
                <input class="input-search-ai" type="text" placeholder="Tìm kiếm thông minh">
                <div class="icon-ai"></div>
            </div>
            <div class="btn-headquarter-ai">
                <div class="icon-headquarter-ai"></div>
            </div>
            <ms-button iconLeft="plus" type="primary">Thêm</ms-button>
            <ms-button iconLeft="import" type="outline-primary">Nhập từ Excel</ms-button>
            <ms-button iconLeft="show-more" type="outline"></ms-button>
            <ms-button iconLeft="list" iconRight="down" type="outline"></ms-button>

        </div>
    </div>

</template>

<script setup>
import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
</script>

<style>
.toolbar {
    padding: 12px 16px;
    background-color: #e2e4e9;
}


/* Left toolbar */
.show-menu {
    border-radius: 4px;
    border: 1px solid #d3d7de;
    padding: 7px 12px !important;
    background-color: #ffffff;
    cursor: pointer;
    margin-right: 16px;
    font-size: 13px;
    font-weight: 600;
}




.btn-reload {
    margin-left: 16px;
    padding: 5px;
    cursor: pointer;
}

.btn-reload:hover {
    background-color: #c5c9d3;
    border-radius: 50%;

}

/* Right toolbar */
.right-bar {
    gap: 8px;
}

.search-ai {
    position: relative;
}

.input-search-ai {
    min-width: 282px;
    border-radius: 4px;
    background-color: #f2f0fd;
    border: 1px solid var(--primary-color);
    outline: none;
    font-size: 13px;
    padding: 7px 16px 7px 32px;
}

.input-search-ai:focus-visible {
    border: 1px solid var(--primary-color);
    background-color: #fff;
}

.input-search-ai::placeholder {
    color: #8886f3;
    opacity: 1;
}

.btn-headquarter-ai {
    border-radius: 4px;
    padding: 6px 6px 7px 6px;
    border-image: linear-gradient(251deg, #9F73F1 71.93%, #4262F0 24.05%) 1;
    background:
        linear-gradient(90deg, #E7EBFD 0%, #ECE7FD 32.21%, #E5F7FF 66.11%, #FDEFE7 100%) padding-box,
        linear-gradient(90deg, #9F73F1 71.93%, #4262F0 24.05%) border-box;
    border: 1px solid transparent;
    border-right-width: 2px;
    cursor: pointer;
}

/* .btn-add-customer {
    background-color: var(--primary-color);
    padding: 6px 16px 7px 8px;
    border-radius: 4px;
    color: #fff;
}

.btn-excel-import {
    border-radius: 4px;
    background-color: #fff;
    padding: 5px 16px 6px 8px;
    border: 1px solid var(--primary-color);
    font-size: 13px;
    color: var(--primary-color);
}

.btn-show-more,
.btn-show-list-action {
    background-color: #ffffff;
    padding: 7px;
    border-radius: 4px;
    border: 1px solid #d3d7de;
} */
</style>