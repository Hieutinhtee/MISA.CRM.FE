<template>
   <DataTable :value="rows" resizableColumns columnResizeMode="expand" showGridlines class="prime-table"
      v-model:selection="selectedProducts">
      <Column selectionMode="multiple" headerStyle="width: 3em"></Column>
      <Column v-for="col in columns" :key="col.field" :field="col.field" :header="col.header" />
   </DataTable>
</template>

<script setup>
import { defineProps, ref } from "vue";
import 'primevue/resources/themes/saga-blue/theme.css'; // Chỉ cho component này
import 'primevue/resources/primevue.min.css';
import 'primeicons/primeicons.css';

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';

const props = defineProps({
   /** danh sách cột */
   columns: {
      type: Array,
      required: true
   },

   /** danh sách dữ liệu */
   rows: {
      type: Array,
      required: true
   },


});

const selectedProducts = ref();
</script>

<style scoped>
.prime-table .p-column-resizer {
   width: 16px;
   /* dễ kéo hơn mặc định */
}

.prime-table .p-datatable-thead>tr>th {
   font-weight: 600;
}
</style>
