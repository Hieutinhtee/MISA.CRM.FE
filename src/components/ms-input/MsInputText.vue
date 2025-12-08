<template>
   <div class="ms-input-wrapper w-100" :class="props.column ? 'flex-column' : 'justify-content-between'">
      <label v-if="props.label" :for="props.label" :class="{ required: props.required }">{{ props.label }}</label>

      <!-- SELECT -->
      <a-select v-if="props.type === 'select'" show-search :options="props.options" :filter-option="filterOption"
         :placeholder="placeholder || label" :value="model" @change="handleSelectChange" />

      <!-- DATE PICKER -->
      <a-space v-else-if="props.type === 'date'">
         <a-date-picker :value="model ? dayjs(model, dateFormatList[0]) : null" @change="handleDateChange"
            class="d-block" :format="dateFormatList" :placeholder="placeholder || label" />
      </a-space>

      <!-- INPUT TEXT -->
      <input v-else :id="props.label" type="text" :placeholder="props.placeholder || ''" v-model="model"
         :class="{ 'input-error': !!errorMessage }" @blur="validate" :readonly="props.readonly" />


      <!-- Hiển thị lỗi đỏ bên dưới -->
      <div v-if="errorMessage" class="error-text">
         {{ errorMessage }}
      </div>
   </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import dayjs from 'dayjs'
import customParseFormat from 'dayjs/plugin/customParseFormat'

dayjs.extend(customParseFormat)

const props = defineProps({
   label: String,
   placeholder: String,
   required: { type: Boolean, default: false },
   readonly: { type: Boolean, default: false },
   column: { type: Boolean, default: true },
   type: String,
   options: Array,
   rules: { type: Array, default: () => [] }
})

const filterOption = (input, option) => {
   return option.label.toLowerCase().includes(input.toLowerCase())
}

const dateFormatList = ['DD/MM/YYYY', 'DD/MM/YY']

const model = defineModel({ required: true })
const errorMessage = ref('')

//  Xử lý thay đổi Select
function handleSelectChange(value) {
   model.value = value
   validate()
}

//  Xử lý thay đổi DatePicker
function handleDateChange(date, dateString) {
   model.value = dateString // Lưu dạng string "DD/MM/YYYY"
   validate()
}

// Hàm validate
function validate() {
   if (!props.rules || props.rules.length === 0) {
      errorMessage.value = ''
      return true
   }
   for (const rule of props.rules) {
      if (typeof rule === 'function') {
         const result = rule(model.value)
         if (result !== true) {
            errorMessage.value = result || 'Lỗi không xác định'
            return false
         }
      }
   }
   errorMessage.value = ''
   return true
}

// Tự động xóa lỗi khi người dùng gõ lại
watch(model, () => {
   if (errorMessage.value) {
      validate()
   }
})

defineExpose({
   validate,
   hasError: computed(() => !!errorMessage.value)
})
</script>

<style scoped>
label {
   width: 174px;
   font-weight: 500;
   color: #1f2229b6;
}


.ms-input-wrapper {
   display: flex;
   margin-bottom: 16px;
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
   position: absolute;
   bottom: -16px;
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