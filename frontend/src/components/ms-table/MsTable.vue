<template>
    <div class="table-content d-flex flex1 flex-column">
        <DataTable
            :value="props.rows"
            scrollable
            scrollHeight="100%"
            resizableColumns
            columnResizeMode="expand"
            lazy
            :loading="props.loading"
            removableSort
            @row-click="handleRowClick"
            sortMode="single"
            class="prime-table flex1"
            dataKey="crmCustomerId"
            :customSort="true"
        >
            <Column :frozen="true">
                <template #header>
                    <div class="custom-header-checkbox">
                        <input
                            class="check-box-icon"
                            type="checkbox"
                            :checked="isCheck && isShow"
                            :indeterminate="isCheck && !isShow"
                            @change="onHeaderCheck"
                        />
                    </div>
                </template>
                <template #body="slotProps">
                    <input
                        class="check-box-icon"
                        type="checkbox"
                        :checked="isRowChecked(slotProps.data)"
                        @change="toggleRow(slotProps.data)"
                    />
                </template>
            </Column>
            <Column
                v-for="col in columns"
                :key="col.field"
                :field="col.field"
                :sortable="true"
                @click="onSortClick(col.field)"
                :style="
                    col.width
                        ? {
                              minWidth: col.width + 'px',
                              maxWidth: col.width + 'px',
                          }
                        : null
                "
            >
                <template #header>
                    <div
                        class="sortable-header"
                        @click="onSortClick(col.field)"
                        style="cursor: pointer"
                    >
                        {{ col.header }}
                        <span v-if="sortState.field === col.field" style="margin-left: 4px"> </span>
                    </div>
                </template>
                <template #body="slotProps">
                    <div class="ellipsis" :title="slotProps.data[col.field]">
                        <div v-if="!slotProps.data[col.field]">-</div>

                        <div v-else-if="col.field === 'crmCustomerPhoneNumber'" class="d-flex">
                            <i class="icon-phone"></i>
                            <MsTextColor
                                >{{ handleFormat(slotProps.data[col.field], col.type) }}
                            </MsTextColor>
                        </div>

                        <div
                            v-else-if="
                                col.field === 'crmCustomerCode' || col.field === 'crmCustomerName'
                            "
                        >
                            <MsTextColor
                                >{{ handleFormat(slotProps.data[col.field], col.type) }}
                            </MsTextColor>
                        </div>

                        <div v-else>
                            {{ handleFormat(slotProps.data[col.field], col.type) }}
                        </div>
                    </div>
                </template>
            </Column>
        </DataTable>

        <div class="footer d-flex justify-content-between align-content-center">
            <div class="footer-start d-flex align-content-center">
                <div class="icon-sort-setting"></div>
                <div class="total-row">
                    <div class="total-row-title">Tổng số:</div>
                    <strong class="total-row-value">{{ localData.totalRows }}</strong>
                </div>
                <div class="total-debt">
                    <div class="total-debt-title">Công nợ</div>
                    <strong class="total-debt-value">0</strong>
                </div>
            </div>

            <div class="footer-end d-flex align-content-center">
                <a-select
                    style="width: 180px"
                    v-model:value="localData.pageSize"
                    @change="onChangePageSize"
                    lineHeight="32px"
                >
                    <template #suffixIcon>
                        <div class="icon-down"></div>
                    </template>
                    <a-select-option :value="10">10 Bản ghi trên trang</a-select-option>
                    <a-select-option :value="20">20 Bản ghi trên trang</a-select-option>
                    <a-select-option :value="50">50 Bản ghi trên trang</a-select-option>
                    <a-select-option :value="100">100 Bản ghi trên trang</a-select-option>
                </a-select>

                <div class="pagination d-flex">
                    <div class="icon-pagination-first pointer" @click="changePage('first')"></div>
                    <div class="icon-left pointer" @click="changePage('prev')"></div>
                    <div>
                        <strong>{{ localData.recordStart }}</strong>
                    </div>
                    <div>đến</div>
                    <div>
                        <strong>{{ localData.recordEnd }}</strong>
                    </div>
                    <div class="icon-right pointer" @click="changePage('next')"></div>
                    <div class="icon-pagination-end pointer" @click="changePage('last')"></div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
// Imports
import { ref, watch, reactive, defineModel } from 'vue'
import 'primevue/resources/themes/saga-blue/theme.css'
import 'primevue/resources/primevue.min.css'
import 'primeicons/primeicons.css'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import MsTextColor from '@/components/ms-button/MsTextColor.vue'
import { formatNumber, formatDate, formatText } from '@/utils/formatter.js'

// Props, Emits, Models
const emit = defineEmits(['update:pagination', 'row-click', 'update:sort-change'])

const props = defineProps({
    /** danh sách cột */
    columns: {
        type: Array,
        required: true,
        validator: (value) => {
            return value.every((col) => {
                const validTypes = ['text', 'number', 'date', 'custom']
                return col.field && col.header && validTypes.includes(col.type || 'text')
            })
        },
    },

    /** danh sách dữ liệu */
    rows: {
        type: Array,
        required: true,
    },

    /** prop cho paging, sort */
    paginationData: {
        type: Object,
        required: true,
    },

    /** trạng thái loading của bảng */
    loading: {
        type: Boolean,
    },
})

// Model cho dữ liệu được chọn (checkbox)
const selectedProducts = defineModel('selection', { default: [] })

// State Data
/**
 * Trạng thái sắp xếp hiện tại của bảng
 * @property {string|null} field - Tên trường đang được sắp xếp.
 * @property {('ASC'|'DESC'|null)} order - Thứ tự sắp xếp.
 */
const sortState = ref({
    field: null, // cột đang sort
    order: null, // 'ASC' | 'DESC' | null
})

/**
 * Copy local của dữ liệu phân trang để component con tự quản lý
 * @property {number} page - Trang hiện tại.
 * @property {number} pageSize - Số bản ghi trên mỗi trang.
 * @property {number} totalRows - Tổng số bản ghi.
 * @property {number} recordStart - Số thứ tự bản ghi đầu trang.
 * @property {number} recordEnd - Số thứ tự bản ghi cuối trang.
 */
const localData = reactive({
    ...props.paginationData,
    recordStart: 1,
    recordEnd: 1,
})

/**
 * Trạng thái của checkAll
 */
const isCheck = ref(false) // người dùng đã bấm chọn tất cả
const isShow = ref(false) // hiển thị của checkbox header
const selected = ref([]) // chứa id được chọn (khi isCheck = false)
const excluded = ref([]) // chứa id bị loại ra (khi isCheck = true)

// Watcher
/**
 * Theo dõi sự thay đổi của props.paginationData từ component cha
 * và cập nhật localData, đồng thời tính lại range hiển thị.
 */
watch(
    () => props.paginationData,
    (newVal) => {
        Object.assign(localData, newVal)
        computeRecordRange(localData.page)
    },
    { deep: true },
)

//#region Methods - Pagination

/**
 * Hàm tính lại recordStart (bản ghi đầu) và recordEnd (bản ghi cuối)
 * dựa trên pageSize và page (số trang).
 * @param {number} pageNumber - Số trang hiện tại.
 * createdby: TMHieu - 09.12.2025
 */
function computeRecordRange(pageNumber) {
    if (localData.totalRows === 0) {
        localData.recordStart = 0
        localData.recordEnd = 0
        return
    }
    localData.recordStart = (pageNumber - 1) * localData.pageSize + 1
    localData.recordEnd = Math.min(
        localData.recordStart + localData.pageSize - 1,
        localData.totalRows,
    )
}

/**
 * Xử lý khi đổi kích thước trang (pageSize) → reset về trang 1
 * @param {number} value - PageSize mới.
 * createdby: TMHieu - 09.12.2025
 */
function onChangePageSize(value) {
    localData.pageSize = value
    localData.page = 1 // reset về trang 1
    computeRecordRange(localData.page)
    // Emit sự kiện lên cha để cha gọi API lấy dữ liệu mới
    emit('update:pagination', { ...localData })
}

/**
 * Thay đổi trang hiện tại khi nhấn các nút điều hướng (first, prev, next, last).
 * @param {('first'|'prev'|'next'|'last')} action - Hành động chuyển trang.
 * createdby: TMHieu - 09.12.2025
 */
function changePage(action) {
    const totalPages = Math.ceil(localData.totalRows / localData.pageSize)
    let currentPage = localData.page || 1

    switch (action) {
        case 'first':
            currentPage = 1
            break
        case 'prev':
            currentPage = Math.max(currentPage - 1, 1)
            break
        case 'next':
            currentPage = Math.min(currentPage + 1, totalPages)
            break
        case 'last':
            currentPage = totalPages
            break
    }

    // Ngăn chặn gọi API nếu không có thay đổi trang (hoặc trang cuối/đầu)
    if (localData.page === currentPage) return

    localData.page = currentPage
    computeRecordRange(currentPage)
    // Emit sự kiện lên cha để cha gọi API lấy dữ liệu mới
    emit('update:pagination', { ...localData })
}

// Khởi tạo range khi component mount lần đầu
computeRecordRange(localData.page || 1)

//#endregion

//#region Methods - Sorting

/**
 * Xử lý sự kiện click vào header cột để sắp xếp
 * @param {string} clickedField - Tên field (cột) được click.
 * createdby: TMHieu - 09.12.2025
 */
function onSortClick(clickedField) {
    if (sortState.value.field === clickedField) {
        // Cột hiện tại, toggle thứ tự: null → ASC → DESC → null
        if (sortState.value.order === null) {
            sortState.value.order = 'ASC'
        } else if (sortState.value.order === 'ASC') {
            sortState.value.order = 'DESC'
        } else {
            sortState.value.order = null
        }
    } else {
        // Cột khác, reset cột khác, mặc định tăng dần
        sortState.value.field = clickedField
        sortState.value.order = 'ASC'
    }

    // Emit lên cha
    emit('update:sort-change', {
        field: sortState.value.field,
        sortOrder: sortState.value.order,
    })
}

//#endregion

//#region Methods - Row & Selection, All checkbox

/**
 * Xử lý sự kiện click vào một hàng (row) trên bảng, emit data của dòng đó lên cha xử lý
 * @param {Object} event - Sự kiện click từ PrimeVue DataTable.
 * createdby: TMHieu - 09.12.2025
 */
function handleRowClick(event) {
    emit('row-click', event.data) // event.data là row được click
}

/**
 * Xử lý sự kiện click vào allCheckbox trên bảng đổi state đánh dấu, trạng thái hiển thị
 * createdby: TMHieu - 12.12.2025
 */
const onHeaderCheck = () => {
    if (!isCheck.value) {
        // Bật chọn tất cả
        isCheck.value = true
        isShow.value = true
        selected.value = []
        excluded.value = []
    } else {
        // Tắt chọn tất cả
        isCheck.value = false
        isShow.value = false
        selected.value = []
        excluded.value = []
    }
}

/**
 * Dòng được chọn hay không
 * @param {Object} row - data của dòng được chọn
 * createdby: TMHieu - 12.12.2025
 */
const isRowChecked = (row) => {
    const id = row.crmCustomerId

    if (isCheck.value) {
        return !excluded.value.includes(id)
    }
    return selected.value.includes(id)
}

// Tick từng dòng
const toggleRow = (row) => {
    const id = row.crmCustomerId

    // Nếu đang ở chế độ "chọn tất cả"
    if (isCheck.value) {
        const idx = excluded.value.indexOf(id)

        if (idx >= 0) {
            excluded.value.splice(idx, 1)
        } else {
            excluded.value.push(id)
        }

        // Nếu không còn dòng bị loại → header hiện tick đầy đủ
        isShow.value = excluded.value.length === 0

        return
    }

    // Trường hợp KHÔNG chọn tất cả
    const i = selected.value.indexOf(id)
    if (i >= 0) {
        selected.value.splice(i, 1)
    } else {
        selected.value.push(id)
    }

    // Kiểm tra xem tất cả dòng trên trang có được chọn hết không
    const allIdsThisPage = props.rows.map((r) => r.crmCustomerId)
    isShow.value = allIdsThisPage.every((id) => selected.value.includes(id))
}
//#endregion

//#region Methods - Formatting

/**
 * Định dạng dữ liệu dựa trên loại (type) cột.
 * @param {*} value - Giá trị cần định dạng.
 * @param {('text'|'number'|'date'|'custom')} type - Loại định dạng.
 * @returns {string} Giá trị đã được định dạng.
 * createdby: TMHieu - 09.12.2025
 */
const handleFormat = (value, type) => {
    switch (type) {
        case 'number':
            return formatNumber(value)
        case 'date':
            return formatDate(value)
        case 'text':
        default:
            return formatText(value)
    }
}

//#endregion
</script>

<style>
/* Toolbar */
.prime-table .p-column-resizer {
    width: 16px;
    /* dễ kéo hơn mặc định */
}

/* Header */
.p-datatable .p-column-header-content {
    box-sizing: border-box;
    padding: 0px 10px 0px 14px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.sortable-header {
    width: 100%;
    padding: 12px 0 12px 0;
}

/* Body */
.prime-table {
    flex: 1 1 auto !important;
    display: flex;
}

.p-datatable-wrapper {
    flex: 1;
}

.p-datatable tbody td {
    box-sizing: border-box;
    padding: 10px 10px 10px 14px;
    text-overflow: ellipsis !important;
    overflow: hidden !important;
    white-space: nowrap !important;
    min-width: 0;
}

.ellipsis {
    display: block;
    width: 100%;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}

.p-datatable .p-datatable-tbody > tr:hover {
    background: #f0f2f4;
    cursor: pointer;
}

/* Sort Icon Styling (Custom behavior) */
.p-datatable .p-sortable-column .p-sortable-column-icon {
    opacity: 0;
    transition: opacity 0.2s;
}

/* Hiển thị khi hover header */
.p-datatable .p-sortable-column:hover .p-sortable-column-icon {
    opacity: 1;
}

/* Chặn PrimeVue sort data (Chỉ dùng click event) */
.p-datatable .p-sortable-column {
    pointer-events: auto !important;
}

.p-datatable .p-sortable-column-icon {
    pointer-events: none !important;
}

/* Checkbox */
.p-checkbox .p-checkbox-box {
    width: 16px !important;
    height: 16px !important;
    margin: 3px 0 3px 0;
    border: 1px solid #7c869c;
    outline: none;
    border-radius: 4px;
}

/* Footer */
.footer {
    border-top: 1px solid #d3d7de;
    padding: 12px 16px;
}

.total-row {
    margin-left: 20px;
}

.total-debt {
    margin-left: 32px;
}

.total-row-title,
.total-debt-title {
    font-weight: 400;
    color: #586074;
    margin-bottom: 3px;
    font-size: 13px;
}

.pagination {
    margin-left: 20px;
    gap: 10px;
}

/* Loading Overlay Custom */
.p-datatable .p-datatable-loading-overlay {
    background: rgba(255, 255, 255, 0.571) !important;
}

.p-datatable .p-datatable-loading-icon {
    width: 50px !important;
    height: 50px !important;
    /* tăng kích thước */
    color: var(--primary-color) !important;
    /* đổi màu */
}

/* Scrollbar Custom */
::-webkit-scrollbar-button {
    display: none;
    /* ẩn các mũi tên ở đầu/cuối scrollbar */
}

::-webkit-scrollbar {
    margin-top: 100px;
    width: 8px;
    /* chiều rộng scrollbar */
    height: 8px;
    /* chiều cao scrollbar (nếu scroll ngang) */
}

::-webkit-scrollbar-thumb {
    background-color: #c5c9d3;
    /* màu thanh scroll */
    border-radius: 4px;
}

::-webkit-scrollbar-track {
    background-color: #f1f1f1;
    border-radius: 4px;
    /* màu nền track */
}
</style>
