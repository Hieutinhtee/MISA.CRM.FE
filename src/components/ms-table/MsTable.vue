<template>
   <div class="table-wrapper position-relative flex1">
      <table class="datagrid" id="wrapper-table">
         <thead>
            <tr>
               <th>
                  <div class="icon-20 icon-checkbox"></div>
               </th>
               <th v-for="field in fields" :key="field.key">{{ field.label }}</th>
            </tr>
         </thead>
         <tbody id="table-body">
            <tr v-for="(row, rowIndex) in rows" :key="rowIndex">
               <td>
                  <div class="icon-20 icon-checkbox" :class="{ checked: selected.includes(rowIndex) }"
                     @click="toggleRow(rowIndex)"></div>
               </td>
               <td v-for="field in fields" :key="field.key">
                  {{ safeGet(row, field, '--') }}
               </td>
            </tr>
         </tbody>
      </table>
   </div>
</template>

<script setup>
import { toValue, defineModel } from 'vue';

const props = defineProps({
   fields: {
      type: Array,
      required: true,
   },

   rows: {
      type: Array,
      required: true
   }
});

const selected = defineModel('modelCheckbox', { type: Array, default: [] });

function toggleRow(index) {
   const i = selected.value.indexOf(index);
   if (i >= 0) {
      selected.value.splice(i, 1);
   } else {
      selected.value.push(index);
   }
}


// const selectedRows = ref([]);

function safeGet(row, field, defaultValue = '') {
   // field.key hợp lệ không?
   if (!field?.key) return defaultValue;

   // lấy value, fallback nếu undefined hoặc null
   const value = row?.[field.key];

   if (value === undefined || value === null) {
      return defaultValue;
   }

   return value;
}
</script>

<style scoped>
.table-wrapper {
   overflow: auto;
}

.datagrid th,
.datagrid td {
   font-size: 14px;
   white-space: nowrap;
   padding: 12px 30px 12px 6px;
   border-bottom: 1px solid #dddddd;
}

.datagrid {
   border-collapse: separate;
}

.datagrid thead th {
   position: sticky;
   top: 0;
   background: #fff;
   z-index: 10;
   border-bottom: 1px solid #00000016;
   border-top: 1px solid #00000016;
}

.datagrid th:first-child,
.datagrid td:first-child {
   position: sticky;
   left: 0;
   background: #fff;
   z-index: 9;
}

.datagrid thead th:first-child {
   z-index: 11;
}

.datagrid thead {
   background-color: #f9fafb;
}

.datagrid tbody tr:hover {
   background-color: #e1eeff;
}

.icon-checkbox {
   margin-left: 15px;
   background: url("/src/assets/imgs/ICON.svg") 0px -696px no-repeat;
   cursor: pointer;
}

.icon-checkbox.checked {
   background: url("/src/assets/imgs/ICON.svg") -20px -696px no-repeat;
   cursor: pointer;
}
</style>