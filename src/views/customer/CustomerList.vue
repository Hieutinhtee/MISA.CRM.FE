<template>
    <!-- Tool bar -->
    <div class="toolbar d-flex justify-content-between flex-col">
        <div class="left-bar d-flex align-content-center">
            <ms-button class="show-menu" iconLeft="folder" iconRight="down" type="outline">Tất cả khách hàng</ms-button>
            <ms-text-color type="primary">Sửa</ms-text-color>
            <div class="btn-grey">
                <div class="icon-reload" @click="reloadData"></div>
            </div>
            <ms-button v-if="selectedFromChild.length > 0" class="m-l-12" type="danger" @click="deleteSelected">
                Xóa
            </ms-button>

            <ms-button v-if="selectedFromChild.length > 0" class="m-l-12" type="outline-primary"
                @click="exportSelected">
                Xuất Excel các bản ghi
            </ms-button>
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
            <ms-button iconLeft="import" type="outline-primary">Nhập từ Excel</ms-button>
            <ms-button iconLeft="show-more" type="outline"></ms-button>
            <ms-button iconLeft="list" iconRight="down" type="outline"></ms-button>
        </div>
    </div>
    <div class="content d-flex flex1 flex-row">
        <ms-table :columns="cols" :rows="data" :pagination-data="payload" @row-click="onRowClick"
            v-model:selectedProducts="selectedFromChild" @update:pagination="onPaginationUpdate"></ms-table>
    </div>

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

const data = ref([]);
const selectedFromChild = ref([]);



// Columns-----------------------------------------------------------
const cols = [
    { field: "crmCustomerType", header: "Loại khách hàng", width: 175 },
    { field: "crmCustomerCode", header: "Mã khách hàng", width: 280 },
    { field: "crmCustomerName", header: "Tên khách hàng", width: 300 },
    { field: "crmCustomerTaxCode", header: "Mã số thuế", width: 160 },
    { field: "crmCustomerShippingAddress", header: "Địa chỉ (Giao hàng)", width: 210 },
    { field: "crmCustomerPhoneNumber", header: "Điện thoại", width: 160 },
    { field: "crmCustomerLastPurchaseDate", header: "Ngày mua hàng gần nhất", width: 240, type: "date" },
    { field: "crmCustomerPurchasedItemCode", header: "Hàng hóa đã mua", width: 200 },
    { field: "crmCustomerPurchasedItemName", header: "Tên hàng hóa đã mua", width: 300 }
];

// ref payload lưu thông tin phân trang
const payload = reactive({
    page: 1,
    pageSize: 100,
    search: "",
    sortBy: "",
    sortOrder: "",
    totalRows: 1000
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
    () => [payload.page, payload.pageSize, payload.sortBy, payload.sortOrder],
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
}

// -----------------------------------
// Hàm gọi API
// -----------------------------------
function loadDataForAPI() {
    // Chỉ lấy 5 tham số đầu để gửi API
    const { page, pageSize, search, sortBy, sortOrder } = payload;

    CustomersAPI.paging({ page, pageSize, search, sortBy, sortOrder })
        .then(result => {
            data.value = result.data.data;
            payload.totalRows = result.data.meta.total // dữ liệu table
        });
}

// -----------------------------------
// Con emit -> cha nhận (ví dụ nếu Footer component emit)
// -----------------------------------
function onPaginationUpdate(newPayload) {
    Object.assign(payload, newPayload);
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
    const payloadDelete = {
        ids: ids.value, // ["3fa85f64-5717-4562-b3fc-2c963f66afa6", "44b71a89-795b-29c6-b892-08c864661f9c"]
        columnName: "crm_customer_is_deleted",
        value: 1 // hoặc "1" tùy kiểu cột
    };
    CustomersAPI.delete(payloadDelete)
        .then(res => {
            if (res.status === 200) { // API update trả về 200 OK
                alert("Xóa thành công", res.data.updatedCount, " bản ghi!");
                reloadData();
                loadDataForAPI();
                selectedFromChild.value = [];
            } else {
                alert(`Có lỗi, status: ${res.status}`);
            }
        })
        .catch(err => {
            console.log("API Error:", err);

            const errors = err?.response?.data?.errors;

            if (errors) {
                // Lấy tất cả lỗi validation nếu có
                const allErrors = Object.values(errors).flat();
                alert(allErrors.join("\n"));
                return;
            }

            alert("Có lỗi xảy ra");
        });
}

function exportSelected(data) {
    console.log("Xuất Excel:", selectedFromChild.value);

    const workbook = new ExcelJS.Workbook();
    const sheet = workbook.addWorksheet('DanhSachKhachHang');

    // Tạo header
    sheet.columns = cols.map(c => ({ header: c.header, key: c.field, width: c.width * 0.1 }));

    // In đậm header
    sheet.getRow(1).font = { bold: true };

    // Thêm dữ liệu
    selectedFromChild.value.forEach(item => {
        const rowData = {};
        cols.forEach(c => {
            if (c.field === 'crmCustomerLastPurchaseDate') {
                rowData[c.field] = formatDate(item[c.field]);
            } else {
                rowData[c.field] = item[c.field];
            }
        });
        sheet.addRow(rowData);
    });
    const fileName = "DanhSachKhachHang" + Date.now() + ".xlsx";
    // Xuất file
    workbook.xlsx.writeBuffer().then(buffer => {
        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), fileName);
    });
}
</script>

<style scoped>
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