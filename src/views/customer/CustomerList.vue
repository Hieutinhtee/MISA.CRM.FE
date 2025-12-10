<template>
    <!-- Tool bar -->
    <div class="toolbar d-flex justify-content-between flex-col">
        <div class="left-bar d-flex align-content-center">
            <a-select lineHeight="32px" style="width: 200px;" class="my-select"
                v-model:value="payload.selectedTypeCustomer">
                <template #suffixIcon>
                    <div class="icon-folder icon-left-select"></div>
                    <div class="icon-down "></div>
                </template>

                <a-select-option :value="null"><span
                        class="select-type-customer-font">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tất
                        cả khách
                        hàng</span></a-select-option>
                <a-select-option :value="'VIP'"><span
                        class="select-type-customer-font">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;VIP</span></a-select-option>
                <a-select-option :value="'NBH01'"><span
                        class="select-type-customer-font">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;NBH01</span></a-select-option>
                <a-select-option :value="'LKHA'"><span
                        class="select-type-customer-font">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;LKHA</span></a-select-option>

            </a-select>

            <ms-text-color type="primary">Sửa</ms-text-color>
            <div class="btn-grey">
                <div class="icon-reload" @click="reloadData"></div>
            </div>
            <ms-button v-if="selectedFromChild.length > 0" class="m-l-12" type="danger" @click="showModalDelete">
                Xóa
            </ms-button>

            <ms-button v-if="selectedFromChild.length > 0" class="m-l-12" type="outline-primary"
                @click="exportSelected">
                Xuất Excel các bản ghi
            </ms-button>
            <ms-button v-if="selectedFromChild.length > 0" type="outline-primary" class="m-l-12"
                @click="showModalChangeType">Gán
                loại
                khách
                hàng</ms-button>
        </div>
        <div class="right-bar d-flex align-content-center gap8">
            <div class="search-ai d-flex align-content-center">
                <div class="icon-search-ai"></div>
                <input class="input-search-ai" type="text" placeholder="Tìm kiếm thông minh" v-model="payload.search">
                <div class="icon-ai"></div>
            </div>
            <div class="btn-headquarter-ai">
                <div class="icon-headquarter-ai"></div>
            </div>
            <ms-button iconLeft="plus" type="primary" @click="goToAdd">Thêm</ms-button>
            <ms-button iconLeft="import" type="outline-primary" @click="showModalExcelImport">Nhập từ Excel</ms-button>
            <ms-button iconLeft="show-more" type="outline"></ms-button>
            <ms-button iconLeft="list" iconRight="down" type="outline"></ms-button>
        </div>
    </div>
    <div class="content d-flex flex1 flex-row">
        <ms-table :columns="cols" :rows="data" :pagination-data="payload" @row-click="onRowClick"
            @update:sort-change="handleSortChange" :loading="isLoadingTable"
            v-model:selectedProducts="selectedFromChild" @update:pagination="onPaginationUpdate"></ms-table>
    </div>
    <a-modal v-model:open="isModalOpen" title="Xác nhận xóa" @ok="handleOk" @cancel="handleCancel" ok-text="Đồng ý"
        cancel-text="Hủy">
        <p>Bạn có chắc chắn xóa {{ ids.length }} bản ghi?</p>
    </a-modal>

    <a-modal v-model:open="isModalOpenChangeType" title="Xác nhận sửa loại khách hàng" @ok="handleOkChangeType"
        @cancel="handleCancelChangeType" ok-text="Sửa" cancel-text="Hủy">
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
    <a-modal v-model:open="isModalOpenExcelImport" title="Nhập từ Excel" @ok="handleOkExcelImport"
        @cancel="handleCancelExcelImport" ok-text="Nhập" cancel-text="Hủy">
        <a-upload-dragger v-model:fileList="fileList" name="file" :multiple="false" accept=".xlsx"
            :beforeUpload="beforeUpload" @change="handleChange">
            <p class="ant-upload-text">Click hoặc kéo file vào đây</p>
            <p class="ant-upload-hint">Hỗ trợ nhập file Excel (xlsx)</p>
        </a-upload-dragger>
    </a-modal>
</template>

<script setup>
import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsTable from '@/components/ms-table/MsTable.vue';
import { useRouter } from 'vue-router';
import { reactive, ref, watch, onMounted, computed } from 'vue';
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js';
import ExcelJS from 'exceljs';
import { saveAs } from 'file-saver';
import { formatNumber, formatDate, formatText } from '@/utils/formatter.js';
import { useToastMessage } from '@/composables/useToastMessage';

const { showToastSuccess, showToastError, showToastInfo } = useToastMessage();

const isLoadingTable = ref(true);
const data = ref([]);
const selectedFromChild = ref([]);
const selectedChangeTypeCustomer = ref('VIP');



// Columns-----------------------------------------------------------
const cols = [
    { field: "crmCustomerType", header: "Loại khách hàng", width: 175 },
    { field: "crmCustomerCode", header: "Mã khách hàng", width: 240 },
    { field: "crmCustomerName", header: "Tên khách hàng", width: 300 },
    { field: "crmCustomerTaxCode", header: "Mã số thuế", width: 160 },
    { field: "crmCustomerShippingAddress", header: "Địa chỉ (Giao hàng)", width: 280 },
    { field: "crmCustomerPhoneNumber", header: "Điện thoại", width: 160 },
    { field: "crmCustomerLastPurchaseDate", header: "Ngày mua hàng gần nhất", width: 240, type: "date" },
    { field: "crmCustomerPurchasedItemCode", header: "Hàng hóa đã mua", width: 200 },
    { field: "crmCustomerPurchasedItemName", header: "Tên hàng hóa đã mua", width: 300 },
    { field: "crmCustomerEmail", header: "Email", width: 200 },
    { field: "crmCustomerAddress", header: "Địa chỉ liên hệ", width: 280 }

];

// ref payload lưu thông tin phân trang
const payload = reactive({
    page: 1,
    pageSize: 100,
    search: "",
    sortBy: "",
    sortOrder: "",
    totalRows: 0,
    selectedTypeCustomer: null
});

// -----------------------------------
// Debounce search
// -----------------------------------
let debounceTimer = null;

watch(
    () => payload.search,
    (newVal) => {
        if (debounceTimer) clearTimeout(debounceTimer);
        debounceTimer = setTimeout(() => {
            payload.page = 1;
            loadDataForAPI();
        }, 350);
    }
);



// Watch các trường khác (page, pageSize, sortBy, sortOrder)
watch(
    () => [payload.page, payload.pageSize, payload.sortBy, payload.sortOrder, payload.selectedTypeCustomer],
    () => {
        loadDataForAPI();
    }
);


function reloadData() {
    payload.page = 1;
    payload.pageSize = 100;
    payload.search = "";
    payload.sortBy = "";
    payload.sortOrder = "";
    payload.selectedTypeCustomer = null;
    loadDataForAPI();
}

// -----------------------------------
// Hàm gọi API
// -----------------------------------
async function loadDataForAPI() {
    isLoadingTable.value = true;
    setTimeout(async () => {
        try {
            const { page, pageSize, search, sortBy, sortOrder, selectedTypeCustomer } = payload;
            const result = await CustomersAPI.paging({
                page,
                pageSize,
                search,
                sortBy,
                sortOrder,
                type: selectedTypeCustomer
            });
            data.value = result.data.data;
            payload.totalRows = result.data.meta.total;
            isChangeSearch.value = false;
        } catch (err) {

        } finally {
            isLoadingTable.value = false;
        }
    });
}


// -----------------------------------
// Con emit -> cha nhận (ví dụ nếu Footer component emit)
// -----------------------------------
function onPaginationUpdate(newPayload) {
    Object.assign(payload, newPayload);
    loadDataForAPI();
}

function handleSortChange({ field, sortOrder }) {
    payload.sortBy = field || "";
    payload.sortOrder = sortOrder || "";

    // Reset về trang 1 khi sort
    payload.page = 1;

    // gọi API lại
    loadDataForAPI();
}


// -----------------------------------
// Mounted: load data 1 lần khi component khởi tạo
// -----------------------------------
onMounted(() => {
    loadDataForAPI();
});


// Route--------------------------------------------------------------
const router = useRouter()

function goToAdd() {
    router.push('/customer/add')
}

//-------------Xử lý edit------------------
function onRowClick(row) {
    router.push(`/customer/edit/${row.crmCustomerId}`);
}

//---------------Xóa----------------------

const ids = ref([]);

// watch mỗi khi selectedFromChild thay đổi
watch(selectedFromChild, (newVal) => {
    ids.value = newVal.map(item => item.crmCustomerId);
    console.log("ids cập nhật:", ids.value);
}, { deep: true });

console.log(ids.value);

// Gọi API


function deleteSelected() {
    const payloadDelete = ids.value; // ["3fa85f64-5717-4562-b3fc-2c963f66afa6", "44b71a89-795b-29c6-b892-08c864661f9c"]

    CustomersAPI.deleteCustomer(payloadDelete)
        .then(res => {
            if (res.status === 200) { // API update trả về 200 OK
                showToastSuccess("Xóa thành công");
                reloadData();
                loadDataForAPI();
                selectedFromChild.value = [];
            } else {
                alert(`Có lỗi, status: ${res.status}`);
            }
        })
        .catch();
}

function changeTypeCustomer() {
    const payloadBulkType = {
        ids: ids.value, // ["3fa85f64-5717-4562-b3fc-2c963f66afa6", "44b71a89-795b-29c6-b892-08c864661f9c"]
        value: selectedChangeTypeCustomer.value
    };
    CustomersAPI.bulkType(payloadBulkType)
        .then(res => {
            if (res.status === 200) { // API update trả về 200 OK
                showToastSuccess("Sửa thành công");
                reloadData();
                loadDataForAPI();
                selectedChangeTypeCustomer.value = 'VIP';
            }
        })
        .catch();
}

function exportSelected() {
    console.log("Xuất Excel:", selectedFromChild.value);

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('DanhSachKhachHang');

    // Tạo header (vẫn giữ header như cậu)
    sheet.columns = cols.map(c => ({ header: c.header, key: c.field, width: Math.max(10, (c.width || 15) * 0.1) }));

    // In đậm header
    sheet.getRow(1).font = { bold: true };

    // Thêm dữ liệu — gán từng cell, ép về string và tiền tố bằng dấu nháy đơn để Excel bắt buộc dạng text
    selectedFromChild.value.forEach(item => {
        const rowData = {};
        cols.forEach(c => {
            let value = item[c.field];

            // Nếu FE đang trả về dạng { text: "xxx" }
            if (value && typeof value === 'object') {
                value = value.text ?? ''; // bóc text
            }

            // Ép tất cả về string
            rowData[c.field] = (value ?? '').toString();
        });

        sheet.addRow(rowData);
    });


    // Tùy chọn: auto filter
    sheet.autoFilter = { from: { row: 1, column: 1 }, to: { row: 1, column: cols.length } };

    const fileName = "DanhSachKhachHang_" + Date.now() + ".xlsx";
    workbook.xlsx.writeBuffer().then(buffer => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), fileName);
    });
}


// Modal xác nhận xóa
const isModalOpen = ref(false);

const showModalDelete = () => {
    isModalOpen.value = true;
};

const handleOk = () => {
    deleteSelected();
    isModalOpen.value = false; // đóng modal
};

const handleCancel = () => {
    isModalOpen.value = false; // đóng modal
};

// Modal xác nhận sửa loại khách hàng
const isModalOpenChangeType = ref(false);

const showModalChangeType = () => {
    isModalOpenChangeType.value = true;
};

const handleOkChangeType = () => {
    changeTypeCustomer();
    isModalOpenChangeType.value = false; // đóng modal
};

const handleCancelChangeType = () => {
    isModalOpenChangeType.value = false; // đóng modal
};

// Modal xác Excel Import khách hàng
const isModalOpenExcelImport = ref(false);

const showModalExcelImport = () => {
    isModalOpenExcelImport.value = true;
};

const handleOkExcelImport = async () => {
    if (!fileList.value.length) {
        alert("Vui lòng chọn file .xlsx!");
        return;
    }

    const file = fileList.value[0].originFileObj;

    const formData = new FormData();
    formData.append("file", file);

    try {

        await CustomersAPI.excelImport(formData);
        showToastSuccess("Nhập thành công!");
        isModalOpen.value = false;
        fileList.value = [];
    } catch (err) {
        console.error(err);
        showToastError("Lỗi nhập file!");
    }
    isModalOpenExcelImport.value = false; // đóng modal
};

const handleCancelExcelImport = () => {
    isModalOpenExcelImport.value = false; // đóng modal
};

const fileList = ref([]);

const beforeUpload = (file) => {
    const isXlsx = file.type ===
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    if (!isXlsx) {
        showToastError("Chỉ được phép nhập file .xlsx");
        return Upload.LIST_IGNORE; // Ant Design Vue 3
    }

    return false; // Chặn auto upload
};

const handleChange = (info) => {
    // Giữ chỉ 1 file — luôn lấy file cuối cùng
    fileList.value = info.fileList.slice(-1);
};

</script>

<style scoped>
.toolbar {
    padding: 12px 16px;
    background-color: #e2e4e9;
}

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
    border-image: linear-gradient(251deg, #9F73F1 71.93%, #4262F0 24.05%) 1;
    background:
        linear-gradient(90deg, #E7EBFD 0%, #ECE7FD 32.21%, #E5F7FF 66.11%, #FDEFE7 100%) padding-box,
        linear-gradient(90deg, #9F73F1 71.93%, #4262F0 24.05%) border-box;
    border: 1px solid transparent;
    border-right-width: 2px;
    cursor: pointer;
}
</style>