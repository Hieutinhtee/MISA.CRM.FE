<template>
   <div class="d-flex flex-column flex1">
      <label :for="label">{{ label }}</label>
      <a-space class="d-block">
         <a-date-picker :format="dateFormatList" class="d-block" :placeholder="placeholder || label" />
      </a-space>
   </div>

</template>


<script setup>
const dateFormatList = ['DD/MM/YYYY', 'DD/MM/YY'];
const props = defineProps({
   label: String,
   placeholder: String
});
</script>
