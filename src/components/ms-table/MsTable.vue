<template>
   <div class="table-content d-flex flex1 flex-column">
      <DataTable :value="rows" scrollable scrollHeight="100%" resizableColumns columnResizeMode="expand" lazy
         :loading="props.loading" removableSort @row-click="handleRowClick" sortMode="single" class="prime-table flex1"
         dataKey="crmCustomerId" selectionMode="checkbox" v-model:selection="selectedProducts" :customSort="true"
         @sort="onSort">
         <Column selectionMode="multiple" :frozen="true"></Column>
         <Column v-for="col in columns" :key="col.field" :field="col.field" :header="col.header" :sortable="true"
            :style="col.width ? {
               minWidth: col.width + 'px',
               maxWidth: col.width + 'px'
            } : null">
            <template #body="slotProps">
               <div class="ellipsis" :title="slotProps.data[col.field]">
                  <div v-if="!slotProps.data[col.field]">-</div>

                  <div v-else-if="col.field === 'crmCustomerPhoneNumber'" class="d-flex">
                     <i class="icon-phone"></i>
                     <MsTextColor>{{ handleFormat(slotProps.data[col.field], col.type) }}
                     </MsTextColor>
                  </div>

                  <div v-else-if="col.field === 'crmCustomerCode' || col.field === 'crmCustomerName'">
                     <MsTextColor>{{ handleFormat(slotProps.data[col.field], col.type) }}
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
            <a-select style="width: 180px" v-model:value="localData.pageSize" @change="onChangePageSize"
               lineHeight="32px">
               <template #suffixIcon>
                  <div class="icon-down"></div>
               </template>
               <a-select-option :value="10">10 Bản ghi trên trang</a-select-option>
               <a-select-option :value="20">20 Bản ghi trên trang</a-select-option>
               <a-select-option :value="50">50 Bản ghi trên trang</a-select-option>
               <a-select-option :value="100">100 Bản ghi trên trang</a-select-option>
            </a-select>

            <div class="pagination d-flex">
               <div class="icon-pagination-first" @click="changePage('first')"></div>
               <div class="icon-left" @click="changePage('prev')"></div>
               <div><strong>{{ localData.recordStart }}</strong></div>
               <div>đến</div>
               <div><strong>{{ localData.recordEnd }}</strong></div>
               <div class="icon-right" @click="changePage('next')"></div>
               <div class="icon-pagination-end" @click="changePage('last')"></div>
            </div>
         </div>
      </div>
   </div>

</template>

<script setup>
import { defineProps, ref, watch, reactive } from "vue";
import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';

import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import { formatNumber, formatDate, formatText } from '@/utils/formatter.js';

const pageSize = ref(100);
const emit = defineEmits(["update:pagination", "row-click", "update:selectedProducts"]);


//Khai báo props
const props = defineProps({
   /** danh sách cột */
   columns: {
      type: Array,
      required: true,
      validator: (value) => {
         return value.every(col => {
            const validTypes = ['text', 'number', 'date', 'custom'];
            return col.field &&
               col.header &&
               validTypes.includes(col.type || 'text');
         });
      }
   },

   /** danh sách dữ liệu */
   rows: {
      type: Array,
      required: true
   },

   //prop cho paging, sort
   paginationData: {
      type: Object,
      required: true
   },
   loading: {
      type: Boolean
   }
});


//----------------------------Xử lý pagination --------------------------------------------------------------------------------------


// tạo copy local để con tự quản lý
const localData = reactive({
   ...props.paginationData,
   recordStart: 1, // số thứ tự bản ghi đầu trang
   recordEnd: 1    // số thứ tự bản ghi cuối trang
});

// watch khi cha update object → cập nhật localData
watch(
   () => props.paginationData,
   (newVal) => {
      Object.assign(localData, newVal);
      computeRecordRange(localData.page);
   },
   { deep: true }
);

// Hàm tính lại recordStart/recordEnd dựa trên pageSize và page (số trang)
function computeRecordRange(pageNumber) {
   localData.recordStart = (pageNumber - 1) * localData.pageSize + 1;
   localData.recordEnd = Math.min(localData.recordStart + localData.pageSize - 1, localData.totalRows);
}

// khi đổi pageSize → reset về trang 1
function onChangePageSize(value) {
   localData.pageSize = value;
   localData.page = 1;                   // reset về trang 1
   computeRecordRange(localData.page);
   emit("update:pagination", { ...localData });
}

// khi nhấn các icon
function changePage(action) {
   const totalPages = Math.ceil(localData.totalRows / localData.pageSize);
   let currentPage = localData.page;

   switch (action) {
      case "first": currentPage = 1; break;
      case "prev": currentPage = Math.max(currentPage - 1, 1); break;
      case "next": currentPage = Math.min(currentPage + 1, totalPages); break;
      case "last": currentPage = totalPages; break;
   }

   localData.page = currentPage;
   computeRecordRange(currentPage);
   emit("update:pagination", { ...localData });
}

// khởi tạo range khi load
computeRecordRange(localData.page || 1);


// ---------------Xử lý checkbox-------------------------------------------------------------------------------------------------------------------------------
const selectedProducts = ref([]);

watch(selectedProducts, (newVal) => {
   emit("update:selectedProducts", newVal);
});

//---------------------Xử lý sort cột-----------------------------------------------------------------

function onSort(e) {
   console.log("Vừa sort");
}
//---------------------Row Click để edit-------------------------------------------------------

function handleRowClick(event) {
   emit('row-click', event.data)  // event.data là row được click
}


//---------------Emit các bản ghi được checkbox lên cha------------------------------------------
const onSelectionChange = (e) => {
   selectedProducts.value = e.value;
   emit("update:selectedProducts", selectedProducts.value);
};

//---------------Format dữ liệu như số, date, string -------------------------------------------------------------------------------------------------------------
const handleFormat = (value, type) => {
   switch (type) {
      case 'number':
         return formatNumber(value);
      case 'date':
         return formatDate(value);
      case 'text':
         return formatText(value);
      default:
         return formatText(value);
   }
};
</script>

<style>
.prime-table .p-column-resizer {
   width: 16px;
   /* dễ kéo hơn mặc định */
}

.prime-table {
   flex: 1 1 auto !important;
   display: flex;
}

.p-datatable-wrapper {
   flex: 1;
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

.p-datatable .p-column-header-content {
   box-sizing: border-box;
   padding: 10px 10px 10px 14px;
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

.p-checkbox .p-checkbox-box {
   width: 16px !important;
   height: 16px !important;
   margin: 3px 0 3px 0;
   border: 1px solid #7c869c;
   outline: none;
   border-radius: 4px;
}

.p-datatable .p-datatable-tbody>tr:hover {
   background: #f0f2f4;
   cursor: pointer;
}

.p-datatable .p-column-header-content {
   display: flex;
   justify-content: space-between;
   align-items: center;
}

.p-datatable .p-sortable-column .p-sortable-column-icon {
   opacity: 0;
   transition: opacity 0.2s;
}

/* Hiển thị khi hover header */
.p-datatable .p-sortable-column:hover .p-sortable-column-icon {
   opacity: 1;
}

/* Chặn PrimeVue sort data */
.p-datatable .p-sortable-column {
   pointer-events: auto !important;
}

.p-datatable .p-sortable-column-icon {
   pointer-events: none !important;
}

.pagination {
   margin-left: 20px;
   gap: 10px;
}

/* Ẩn arrow, track mặc định và các phần không cần thiết */
::-webkit-scrollbar-button {
   display: none;
   /* ẩn các mũi tên ở đầu/cuối scrollbar */
}

/* Tùy chỉnh scrollbar */
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
</style>
