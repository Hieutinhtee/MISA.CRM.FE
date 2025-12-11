<template>
    <div class="toolbar d-flex justify-content-between flex-col">
        <div class="left-bar d-flex align-content-center">
            <a-select
                lineHeight="32px"
                style="width: 200px"
                class="my-select"
                v-model:value="payload.selectedTypeCustomer"
            >
                <template #suffixIcon>
                    <div class="icon-folder icon-left-select"></div>
                    <div class="icon-down"></div>
                </template>

                <a-select-option :value="null"
                    ><span class="select-type-customer-font"
                        >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tất cả khách hàng</span
                    ></a-select-option
                >
                <a-select-option :value="'VIP'"
                    ><span class="select-type-customer-font"
                        >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;VIP</span
                    ></a-select-option
                >
                <a-select-option :value="'NBH01'"
                    ><span class="select-type-customer-font"
                        >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;NBH01</span
                    ></a-select-option
                >
                <a-select-option :value="'LKHA'"
                    ><span class="select-type-customer-font"
                        >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LKHA</span
                    ></a-select-option
                >
            </a-select>

            <ms-text-color type="primary">Sửa</ms-text-color>

            <div class="btn-grey">
                <div class="icon-reload" @click="reloadData"></div>
            </div>

            <ms-button
                v-if="selectedFromChild.length > 0"
                class="m-l-12"
                type="danger"
                @click="showModalDelete"
            >
                Xóa
            </ms-button>

            <ms-button
                v-if="selectedFromChild.length > 0"
                class="m-l-12"
                type="outline-primary"
                @click="exportSelected"
            >
                Xuất Excel các bản ghi
            </ms-button>

            <ms-button
                v-if="selectedFromChild.length > 0"
                type="outline-primary"
                class="m-l-12"
                @click="showModalChangeType"
                >Gán loại khách hàng</ms-button
            >
        </div>
        <div class="right-bar d-flex align-content-center gap8">
            <div class="search-ai d-flex align-content-center">
                <div class="icon-search-ai"></div>
                <input
                    class="input-search-ai"
                    type="text"
                    placeholder="Tìm kiếm thông minh"
                    v-model="payload.search"
                />
                <div class="icon-ai"></div>
            </div>

            <div class="btn-headquarter-ai">
                <div class="icon-headquarter-ai"></div>
            </div>

            <ms-button iconLeft="plus" type="primary" @click="goToAdd">Thêm</ms-button>

            <ms-button iconLeft="import" type="outline-primary" @click="showModalExcelImport"
                >Nhập từ Excel</ms-button
            >

            <ms-button iconLeft="show-more" type="outline"></ms-button>

            <ms-button iconLeft="list" iconRight="down" type="outline"></ms-button>
        </div>
    </div>

    <div class="content d-flex flex1 flex-row">
        <ms-table
            :columns="cols"
            :rows="data"
            :pagination-data="payload"
            @row-click="onRowClick"
            @update:sort-change="handleSortChange"
            :loading="isLoadingTable"
            v-model:selection="selectedFromChild"
            @update:pagination="onPaginationUpdate"
        ></ms-table>
    </div>
    <a-modal
        v-model:open="isModalOpen"
        title="Xác nhận xóa"
        @ok="handleOk"
        @cancel="handleCancel"
        ok-text="Đồng ý"
        cancel-text="Hủy"
    >
        <p>Bạn có chắc chắn xóa {{ ids.length }} bản ghi?</p>
    </a-modal>

    <a-modal
        v-model:open="isModalOpenChangeType"
        title="Xác nhận sửa loại khách hàng"
        @ok="handleOkChangeType"
        @cancel="handleCancelChangeType"
        ok-text="Sửa"
        cancel-text="Hủy"
    >
        <p class="m-b-12">Chọn loại khách hàng cho {{ ids.length }} bản ghi</p>
        <a-select style="width: 180px" v-model:value="selectedChangeTypeCustomer" lineHeight="32px">
            <template #suffixIcon>
                <div class="icon-down"></div>
            </template>
            <a-select-option :value="'VIP'">VIP</a-select-option>
            <a-select-option :value="'LKHA'">LKHA</a-select-option>
            <a-select-option :value="'NBH01'">NBH01</a-select-option>
        </a-select>
    </a-modal>

    <a-modal
        v-model:open="isModalOpenExcelImport"
        title="Nhập từ Excel"
        @ok="handleOkExcelImport"
        @cancel="handleCancelExcelImport"
        ok-text="Nhập"
        cancel-text="Hủy"
    >
        <a-upload-dragger
            v-model:fileList="fileList"
            name="file"
            :multiple="false"
            accept=".xlsx"
            :beforeUpload="beforeUpload"
            @change="handleChange"
        >
            <p class="ant-upload-text">Click hoặc kéo file vào đây</p>
            <p class="ant-upload-hint">Hỗ trợ nhập file Excel (xlsx)</p>
        </a-upload-dragger>
    </a-modal>
</template>

<script setup>
// #region Imports & Init
import { reactive, ref, watch, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import ExcelJS from 'exceljs'
import { saveAs } from 'file-saver'
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js'
import { useToastMessage } from '@/composables/useToastMessage'
// Lưu ý: Import Upload.LIST_IGNORE nếu dùng Ant Design Vue
// import { Upload } from 'ant-design-vue';

import MsTextColor from '@/components/ms-button/MsTextColor.vue'
import MsButton from '@/components/ms-button/MsButton.vue'
import MsTable from '@/components/ms-table/MsTable.vue'

// Init Utils
const { showToastSuccess, showToastError } = useToastMessage()
const router = useRouter()
// #endregion

//#region Static Data
/**
 * Cấu hình cột cho bảng
 * @type {Array<Object>}
 */
const cols = [
    { field: 'crmCustomerType', header: 'Loại khách hàng', width: 175 },
    { field: 'crmCustomerCode', header: 'Mã khách hàng', width: 240 },
    { field: 'crmCustomerName', header: 'Tên khách hàng', width: 300 },
    { field: 'crmCustomerTaxCode', header: 'Mã số thuế', width: 160 },
    { field: 'crmCustomerShippingAddress', header: 'Địa chỉ (Giao hàng)', width: 280 },
    { field: 'crmCustomerPhoneNumber', header: 'Điện thoại', width: 160 },
    {
        field: 'crmCustomerLastPurchaseDate',
        header: 'Ngày mua hàng gần nhất',
        width: 240,
        type: 'date',
    },
    { field: 'crmCustomerPurchasedItemCode', header: 'Hàng hóa đã mua', width: 200 },
    { field: 'crmCustomerPurchasedItemName', header: 'Tên hàng hóa đã mua', width: 300 },
    { field: 'crmCustomerEmail', header: 'Email', width: 200 },
    { field: 'crmCustomerAddress', header: 'Địa chỉ liên hệ', width: 280 },
]
//#endregion

//#region State Data
// Trạng thái bảng và dữ liệu
const isLoadingTable = ref(true)
const data = ref([])
const selectedFromChild = ref([])
/** Danh sách ID của các khách hàng đang được chọn */
const ids = ref([])
/** Loại khách hàng được chọn trong Modal Gán loại */
const selectedChangeTypeCustomer = ref('VIP')

// Payload phân trang và filter
/**
 * Dữ liệu payload để gọi API phân trang và filter
 * @type {Object}
 */
const payload = reactive({
    page: 1,
    pageSize: 100,
    search: '',
    sortBy: '',
    sortOrder: '',
    totalRows: 0,
    selectedTypeCustomer: null,
})

// Trạng thái Modal
const isModalOpen = ref(false) // Modal Xóa
const isModalOpenChangeType = ref(false) // Modal Sửa loại khách hàng
const isModalOpenExcelImport = ref(false) // Modal Import Excel

// Upload Excel state
const fileList = ref([])
let debounceTimer = null
//#endregion

//#region Watchers & Hooks
/**
 * Theo dõi sự thay đổi của text search (Debounce)
 * createdby: TMHieu - 09.12.2025
 */
watch(
    () => payload.search,
    (newVal) => {
        if (debounceTimer) clearTimeout(debounceTimer)
        debounceTimer = setTimeout(() => {
            payload.page = 1 // Reset về trang 1 khi search
            loadDataForAPI()
        }, 350)
    },
)

/**
 * Theo dõi các thay đổi về phân trang, sort, filter loại KH để reload data
 * createdby: TMHieu - 09.12.2025
 */
watch(
    () => [
        payload.page,
        payload.pageSize,
        payload.sortBy,
        payload.sortOrder,
        payload.selectedTypeCustomer,
    ],
    () => {
        loadDataForAPI()
    },
)

/**
 * Theo dõi danh sách bản ghi được chọn từ bảng để lấy ra danh sách ID
 * createdby: TMHieu - 09.12.2025
 */
watch(selectedFromChild, (newVal) => {
    ids.value = newVal.map((item) => item.crmCustomerId)
    console.log('selectedFromChild', selectedFromChild.value)
    console.log('ids cập nhật:', ids.value)
})

/**
 * Lifecycle Mounted: Load dữ liệu lần đầu
 * createdby: TMHieu - 09.12.2025
 */
onMounted(() => {
    loadDataForAPI()
})
//#endregion

//#region Methods - Data Loading & Table Interactions

/**
 * Hàm gọi API lấy danh sách khách hàng theo payload hiện tại
 * createdby: TMHieu - 09.12.2025
 */
async function loadDataForAPI() {
    isLoadingTable.value = true
    setTimeout(async () => {
        try {
            const { page, pageSize, search, sortBy, sortOrder, selectedTypeCustomer } = payload
            const result = await CustomersAPI.paging({
                page,
                pageSize,
                search,
                sortBy,
                sortOrder,
                type: selectedTypeCustomer,
            })
            data.value = result.data.data
            payload.totalRows = result.data.meta.total
        } catch (err) {
            console.error(err)
        } finally {
            isLoadingTable.value = false
        }
    }, 750) // Dùng setTimeout 0ms để tách khỏi watch/ui thread
}

/**
 * Hàm reload lại dữ liệu về trạng thái mặc định (trang 1, clear filter)
 * createdby: TMHieu - 09.12.2025
 */
function reloadData() {
    payload.page = 1
    payload.pageSize = 100
    payload.search = ''
    payload.sortBy = ''
    payload.sortOrder = ''
    payload.selectedTypeCustomer = null
    loadDataForAPI()
}

/**
 * Hàm xử lý khi thay đổi phân trang (nhận từ component con)
 * @param {Object} newPayload - { page, pageSize }
 * createdby: TMHieu - 09.12.2025
 */
function onPaginationUpdate(newPayload) {
    Object.assign(payload, newPayload)
    loadDataForAPI()
}

/**
 * Hàm xử lý khi thay đổi sắp xếp cột
 * @param {Object} param0 { field, sortOrder }
 * createdby: TMHieu - 09.12.2025
 */
function handleSortChange({ field, sortOrder }) {
    payload.sortBy = field || ''
    payload.sortOrder = sortOrder || ''
    payload.page = 1 // Quay về trang 1 khi sort
    loadDataForAPI()
}
// #endregion

//#region Methods - Navigation & Actions

/**
 * Chuyển hướng đến trang thêm mới khách hàng
 * createdby: TMHieu - 09.12.2025
 */
function goToAdd() {
    router.push('/customer/add')
}

/**
 * Xử lý khi click vào 1 dòng trong bảng -> Chuyển đến trang sửa
 * @param {Object} row - Dữ liệu của dòng được click
 * createdby: TMHieu - 09.12.2025
 */
function onRowClick(row) {
    router.push(`/customer/edit/${row.crmCustomerId}`)
}

/**
 * Gọi API xóa các bản ghi đã chọn
 * createdby: TMHieu - 09.12.2025
 */
function deleteSelected() {
    const payloadDelete = ids.value

    CustomersAPI.deleteCustomer(payloadDelete)
        .then((res) => {
            if (res.status === 200) {
                showToastSuccess('Xóa thành công')
                reloadData() // Reload data sau khi xóa
                selectedFromChild.value = [] // Clear selection
            } else {
                alert(`Có lỗi, status: ${res.status}`)
            }
        })
        .catch(() => {
            // Lỗi đã được xử lý chung trong Axios Interceptor
        })
}

/**
 * Gọi API cập nhật loại khách hàng cho hàng loạt bản ghi
 * createdby: TMHieu - 09.12.2025
 */
function changeTypeCustomer() {
    const payloadBulkType = {
        ids: ids.value,
        value: selectedChangeTypeCustomer.value,
    }
    CustomersAPI.bulkType(payloadBulkType)
        .then((res) => {
            if (res.status === 200) {
                showToastSuccess('Sửa thành công')
                reloadData()
                selectedChangeTypeCustomer.value = 'VIP'
            }
        })
        .catch(() => {
            // Lỗi đã được xử lý chung trong Axios Interceptor
        })
}

/**
 * Xuất khẩu dữ liệu đã chọn ra file Excel
 * createdby: TMHieu - 09.12.2025
 */
function exportSelected() {
    console.log('Xuất Excel:', selectedFromChild.value)

    const workbook = new ExcelJS.Workbook()
    const sheet = workbook.addWorksheet('DanhSachKhachHang')

    // Tạo header
    sheet.columns = cols.map((c) => ({
        header: c.header,
        key: c.field,
        width: Math.max(10, (c.width || 15) * 0.1), // Căn chỉnh width cho Excel
    }))
    sheet.getRow(1).font = { bold: true }

    // Thêm dữ liệu
    selectedFromChild.value.forEach((item) => {
        const rowData = {}
        cols.forEach((c) => {
            let value = item[c.field]
            // Xử lý trường hợp giá trị là object (ví dụ: format trong bảng)
            if (value && typeof value === 'object') {
                value = value.text ?? ''
            }
            rowData[c.field] = (value ?? '').toString()
        })
        sheet.addRow(rowData)
    })

    // Thiết lập Auto Filter
    sheet.autoFilter = { from: { row: 1, column: 1 }, to: { row: 1, column: cols.length } }

    // Export và save file
    const fileName = 'DanhSachKhachHang_' + Date.now() + '.xlsx'
    workbook.xlsx.writeBuffer().then((buffer) => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), fileName)
        showToastSuccess(`Xuất file ${fileName} thành công!`)
    })
}
// #endregion

//#region Methods - Modal Handlers

// 1. Modal Xóa
const showModalDelete = () => {
    isModalOpen.value = true
}

const handleOk = () => {
    deleteSelected()
    isModalOpen.value = false
}

const handleCancel = () => {
    isModalOpen.value = false
}

// 2. Modal Sửa loại khách hàng
const showModalChangeType = () => {
    isModalOpenChangeType.value = true
}

const handleOkChangeType = () => {
    changeTypeCustomer()
    isModalOpenChangeType.value = false
}

const handleCancelChangeType = () => {
    isModalOpenChangeType.value = false
}

// 3. Modal & Logic Import Excel
const showModalExcelImport = () => {
    isModalOpenExcelImport.value = true
}

const handleCancelExcelImport = () => {
    isModalOpenExcelImport.value = false
    fileList.value = [] // Xóa file đang chọn
}

/**
 * Xử lý sự kiện Import file Excel (gọi API)
 * createdby: TMHieu - 09.12.2025
 */
const handleOkExcelImport = async () => {
    if (!fileList.value.length) {
        showToastError('Vui lòng chọn file .xlsx!')
        return
    }

    const file = fileList.value[0].originFileObj
    const formData = new FormData()
    formData.append('file', file)

    try {
        await CustomersAPI.excelImport(formData)
        showToastSuccess('Nhập thành công')
        isModalOpenExcelImport.value = false
        fileList.value = []
        reloadData() // Reload data sau khi import thành công
    } catch (err) {
        // Lỗi đã được xử lý chung trong Axios Interceptor
    } finally {
        isModalOpenExcelImport.value = false
    }
}

/**
 * Validate trước khi upload file (Ant Design Vue Hook)
 * @param {File} file
 * @returns {false | 'skip'}
 * createdby: TMHieu - 09.12.2025
 */
const beforeUpload = (file) => {
    // Chỉ chấp nhận file .xlsx
    const isXlsx = file.type === 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    if (!isXlsx) {
        showToastError('Chỉ được phép nhập file .xlsx')
        return false // Trả về false/Upload.LIST_IGNORE để ngăn upload
    }
    return false // Ngăn Ant Design tự động upload, ta sẽ upload bằng handleOkExcelImport
}

/**
 * Xử lý change file list (giới hạn 1 file cho input)
 * @param {Object} info
 * createdby: TMHieu - 09.12.2025
 */
const handleChange = (info) => {
    // Luôn giữ file cuối cùng được chọn
    fileList.value = info.fileList.slice(-1)
}

//#endregion
</script>

<style scoped>
.toolbar {
    padding: 12px 16px;
    background-color: #e2e4e9;
}

/* Ant Design Select Overrides (Nếu cần thiết, nên dùng CSS Global hoặc Deep Selector) */
/* Cần xem xét lại CSS này, có vẻ đang cố gắng thêm icon không chuẩn */
.ant-select .ant-select-selector::before {
    content: 'sdasdasdas';
    position: absolute;
    left: 8px;
    top: 50%;
    transform: translateY(-50%);
    width: 16px;
    height: 16px;
    background: url('/icon-user.svg') no-repeat center/contain;
}

/* Left toolbar */
.my-select {
    margin-right: 16px;
}

.select-type-customer-font {
    font-weight: 600;
}

/* Vị trí Icon Folder trong select */
.icon-left-select {
    position: relative;
    left: -137px;
}

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
    /* Sử dụng border-image cho hiệu ứng gradient border */
    border-image: linear-gradient(251deg, #9f73f1 71.93%, #4262f0 24.05%) 1;
    /* Gradient background cho fill bên trong */
    background:
        linear-gradient(90deg, #e7ebfd 0%, #ece7fd 32.21%, #e5f7ff 66.11%, #fdefe7 100%) padding-box,
        linear-gradient(90deg, #9f73f1 71.93%, #4262f0 24.05%) border-box;
    border: 1px solid transparent;
    border-right-width: 2px;
    cursor: pointer;
}
</style>
