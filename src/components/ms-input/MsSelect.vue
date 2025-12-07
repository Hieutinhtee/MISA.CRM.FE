<template>
   <a-select show-search :options="props.options" :filter-option="filterOption" :placeholder="placeholder || label"
      @change="onChange" />
</template>

<script setup>
import { defineEmits } from 'vue';

const props = defineProps({
   label: String,
   placeholder: String,
   options: { type: Array, default: () => [] }
});
// filter mặc định
const filterOption = (input, option) => {
   return option.value.toLowerCase().includes(input.toLowerCase())
}

const emit = defineEmits(["change"]);
function onChange(value) {
   emit("change", value);
}
</script>

<style>
.ant-select {
   height: 32px;
   flex: 1 1 550px;
   min-width: 0px;
   transition: all 0.2s;
   border-radius: 4px;
}

.ant-select-selector {
   border-radius: 4px !important;
}
</style>