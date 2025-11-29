<template>
   <div class="d-flex flex-column flex1">
      <label :for="label">{{ label }}</label>
      <a-select show-search :options="props.options" :filter-option="filterOption" :placeholder="placeholder || label"
         v-model="model" />
   </div>
</template>

<script setup>

const props = defineProps({
   label: String,
   placeholder: String,
   options: { type: Array, default: () => [] }
});
// filter mặc định
const filterOption = (input, option) => {
   return option.value.toLowerCase().includes(input.toLowerCase())
}

const model = defineModel();
</script>
