<template>
    <!-- Tool bar -->
    <div class="toolbar d-flex justify-content-between">
        <div class="left-bar d-flex align-content-center">
            <div class="show-menu d-flex ">
                <div class="icon-folder"></div>
                <div class="title-show-menu">Tất cả khách hàng</div>
                <div class="icon-down"></div>
            </div>
            <div class="btn-edit">Sửa</div>
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
            <div class="btn-add-customer d-flex">
                <div class="icon-plus"></div>
                <div class="title-add">Thêm</div>
            </div>
            <div class="btn-excel-import d-flex">
                <div class="icon-import"></div>
                <div class="title-import">Nhập từ Excel</div>
            </div>
            <div class="btn-show-more ">
                <div class="icon-show-more"></div>
            </div>
            <div class="btn-show-list-action d-flex">
                <div class="icon-list"></div>
                <div class="icon-down"></div>
            </div>
        </div>
    </div>
    <ms-table :columns="cols" :rows="data"></ms-table>
</template>

<script setup>
import MsTable from '../components/ms-table/MsTable.vue';
const cols = [
    { field: "name", header: "Tên", width: 200 },
    { field: "age", header: "Tuổi", width: 100 },
    { field: "email", header: "Email", width: 250 }
];

const data = [
    { name: "A", age: 20, email: "a@gmail.com" },
    { name: "B", age: 21, email: "b@gmail.com" }
];
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
    padding: 7px 12px;
    background-color: #ffffff;
    cursor: pointer;
}

.title-show-menu {
    margin: 0 8px;
    font-size: 13px;
    font-weight: 600;
}

.btn-edit {
    color: var(--primary-color);
    cursor: pointer;
    margin-left: 16px;
    font-size: 13px;
}

.btn-edit:hover {
    text-decoration: underline;
}

.btn-reload {
    margin-left: 16px;
    padding: 5px;
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
    padding: 6px;
    border-image: linear-gradient(251deg, #9F73F1 71.93%, #4262F0 24.05%) 1;
    background:
        linear-gradient(90deg, #E7EBFD 0%, #ECE7FD 32.21%, #E5F7FF 66.11%, #FDEFE7 100%) padding-box,
        linear-gradient(90deg, #9F73F1 71.93%, #4262F0 24.05%) border-box;
    border: 1px solid transparent;
    border-right-width: 2px;
}

.btn-add-customer {
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
}

/* Icon spite */
.icon-folder {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -208px -177px;
}

.icon-down {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -48px -32px;
}

.icon-reload {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -352px -32px;
}

.icon-search-ai {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat 0px -1425px;
    position: absolute;
    left: 10px;
}

.icon-ai {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/ai-icon-16.svg') no-repeat;
    position: absolute;
    right: 10px;
}

.icon-headquarter-ai {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -16px -1425px;
}

.icon-plus {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -240px -128px;
    margin-right: 8px;
}

.icon-import {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -176px -192px;
    margin-right: 8px;
}

.icon-show-more {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -48px 0px;
}

.icon-list {
    width: 16px;
    height: 16px;
    background: url('../assets/icons/icon.svg') no-repeat -128px -304px;
    margin-right: 4px;
}
</style>