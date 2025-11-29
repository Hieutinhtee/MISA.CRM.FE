<template>
   <div class="ms-input-wrapper" :class="props.column ? 'flex-column' : 'justify-content-between'">
      <label v-if="props.label" :for="props.label">{{ props.label }}</label>

      <input :id="props.label" type="text" :placeholder="props.placeholder || props.label" v-model="model"
         :class="{ 'input-error': !!errorMessage }" @blur="validate" />

      <!-- Hiển thị lỗi đỏ bên dưới -->
      <div v-if="errorMessage" class="error-text">
         {{ errorMessage }}
      </div>
   </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'

const props = defineProps({
   label: String,
   placeholder: String,
   column: { type: Boolean, default: true },
   rules: { type: Array, default: () => [] }  // Nhận rules từ cha
})

const model = defineModel({ required: true })
const errorMessage = ref('')

// Hàm validate riêng (có thể gọi từ bên ngoài)
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

// Quan trọng: expose hàm validate ra ngoài để cha gọi được
defineExpose({
   validate,
   hasError: computed(() => !!errorMessage.value)
})
</script>

<style scoped>
.ms-input-wrapper {
   display: flex;
   margin-bottom: 16px;
   position: relative;
}

.column {
   flex-direction: column;
}

input {
   height: 32px;
   padding: 0 12px;
   border: 1px solid #ddd;
   border-radius: 4px;
   transition: all 0.2s;

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

.error-text {
   color: #ff4d4f;
   font-size: 12px;
   margin-top: 4px;
   position: absolute;
   bottom: -16px;
}
</style>