<template>
   <div class="table-content d-flex flex1 flex-column">
      <DataTable :value="rows" scrollable scrollHeight="100%" resizableColumns columnResizeMode="expand"
         class="prime-table flex1" dataKey="customerId" selectionMode="checkbox" v-model:selection="selectedProducts">
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
               <strong class="total-row-value">22</strong>
            </div>
            <div class="total-debt">
               <div class="total-debt-title">Công nợ</div>
               <strong class="total-debt-value">22.000.00</strong>
            </div>
         </div>
         <div class="footer-end d-flex align-content-center">
            <a-select v-model:value="pageSize" style="width: 180px" lineHeight="32px">
               <template #suffixIcon>
                  <div class="icon-down"></div>
               </template>
               <a-select-option :value="10">10 Bản ghi trên trang</a-select-option>
               <a-select-option :value="20">20 Bản ghi trên trang</a-select-option>
               <a-select-option :value="50">50 Bản ghi trên trang</a-select-option>
               <a-select-option :value="100">100 Bản ghi trên trang</a-select-option>
            </a-select>
            <div class="pagination d-flex">
               <div class="icon-pagination-first"></div>
               <div class="icon-left"></div>
               <div><strong>4</strong></div>
               <div>đến</div>
               <div><strong>26</strong></div>

               <div class="icon-right"></div>
               <div class="icon-pagination-end"></div>
            </div>
         </div>
      </div>
   </div>

</template>

<script setup>
import { defineProps, ref, watch } from "vue";
import 'primevue/resources/themes/saga-blue/theme.css';
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';

import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import { formatNumber, formatDate, formatText } from '@/utils/formatter.js';

const pageSize = ref(100);

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


});

const selectedProducts = ref();

watch(selectedProducts, () => console.log(selectedProducts));

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

.p-datatable {
   min-width: 100%;
}

.p-datatable-wrapper {
   min-width: 100%;
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


.pagination {
   margin-left: 20px;
   gap: 10px;
}
</style>
