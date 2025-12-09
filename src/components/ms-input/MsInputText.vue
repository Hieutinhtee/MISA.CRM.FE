<template>
   <div class="wrapper-input">
      <div class="ms-input-wrapper w-100" :class="props.column ? 'flex-column' : 'justify-content-between'">
         <label v-if="props.label" :for="props.label" :class="{ required: props.required }">{{ props.label }}</label>

         <!-- SELECT -->
         <a-select v-if="props.type === 'select'" show-search :options="props.options" :filter-option="filterOption"
            @blur="validate" :placeholder="placeholder || label" :value="model" @change="handleSelectChange" />

         <!-- DATE PICKER -->
         <a-space v-else-if="props.type === 'date'">
            <a-date-picker :value="model ? dayjs(model, dateFormatList[0]) : null" @change="handleDateChange"
               @blur="validate" class="d-block" :format="dateFormatList" :placeholder="placeholder || label" />
         </a-space>

         <!-- INPUT TEXT -->
         <input v-else :id="props.label" type="text" :placeholder="props.placeholder || ''" v-model="model"
            :class="{ 'input-error': !!errorMessage }" @blur="validate" :readonly="props.readonly" />


         <!-- Hiển thị lỗi đỏ bên dưới -->

      </div>
      <div v-if="errorMessage" class="error-text d-flex">
         <div>{{ errorMessage }}</div>
      </div>
   </div>
</template>

<script setup>
import { ref, computed, watch, defineExpose } from 'vue'
import dayjs from 'dayjs'
import customParseFormat from 'dayjs/plugin/customParseFormat'
dayjs.extend(customParseFormat);

const props = defineProps({
   label: String,
   placeholder: String,
   required: { type: Boolean, default: false },
   readonly: { type: Boolean, default: false },
   column: { type: Boolean, default: true },
   type: String,
   options: Array,
   rules: { type: Array, default: () => [] },
   externalError: { type: String, default: "" },
   field: String
});

const filterOption = (input, option) => {
   return option.label.toLowerCase().includes(input.toLowerCase())
}

const dateFormatList = ['DD/MM/YYYY', 'DD/MM/YY'];

const model = defineModel({ required: true });
const emit = defineEmits(["blur-check"]);

const errorMessage = ref('');

//  Xử lý thay đổi Select
function handleSelectChange(value) {
   model.value = value
}

//  Xử lý thay đổi DatePicker
function handleDateChange(date, dateString) {
   model.value = dateString // Lưu dạng string "DD/MM/YYYY"
}

// Hàm emit lên để cha validate
async function validate() {
   emit("blur-check", {
      label: props.label,
      field: props.field,
      value: model.value
   });
}

watch(() => props.externalError, (val) => {
   errorMessage.value = val;
});


watch(model, () => {
   if (errorMessage.value) {
      errorMessage.value = ""; // xóa lỗi
   }
})


defineExpose({
   setError(msg) {
      errorMessage.value = msg;
   },
   clearError() {
      errorMessage.value = "";
   }
});

</script>

<style scoped>
label {
   width: 174px;
   font-weight: 500;
   color: #1f2229b6;
}

.wrapper-input {
   margin-bottom: 16px;
}

.ms-input-wrapper {
   display: flex;
   margin-bottom: 6px;
   position: relative;
   align-items: center;
}

.column {
   flex-direction: column;
}

input {
   height: 32px;
   flex: 1 1 550px;
   min-width: 0px;
   padding: 6px 32px 6px 16px !important;
   border: 1px solid #ddd;
   border-radius: 4px;
   transition: all 0.2s;
}

input::placeholder {
   color: #99a1b2;
}

input:focus {
   outline: none;
   border-color: #2970f6;
   /* box-shadow: 0 0 0 3px rgba(0, 115, 230, 0.1); */
}

.input-error {
   border-color: #ff4d4f !important;
   /* box-shadow: 0 0 0 3px rgba(255, 77, 79, 0.1); */
}

.required::after {
   content: " *";
   color: red;
   margin-left: 2px;
}

.error-text {
   color: #ff4d4f;
   font-size: 12px;
   margin-top: 4px;
   justify-content: flex-end;
   /* position: absolute;
   bottom: -16px;
   right: 0; */
}

:deep(.ant-select) {
   height: 32px;
   flex: 1 1 550px;
   min-width: 0px;
   transition: all 0.2s;
   border-radius: 4px;
}

:deep(.ant-select-selector) {
   border-radius: 4px !important;
}

:deep(.ant-space) {
   height: 32px;
   flex: 1 1 550px;
   min-width: 0px;
   transition: all 0.2s;
}

:deep(.ant-space-item) {
   width: 100%;
}

:deep(.ant-picker) {
   border-radius: 4px !important;
}
</style>