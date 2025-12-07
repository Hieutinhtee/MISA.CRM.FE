<template>
   <a-space>
      <a-date-picker v-model="model" class="d-block" :format="dateFormatList" :placeholder="placeholder || label" />
   </a-space>
</template>


<script setup>
import { defineModel } from 'vue';

const dateFormatList = ['DD/MM/YYYY', 'DD/MM/YY'];
const props = defineProps({
   label: String,
   placeholder: String
});

const model = defineModel()
</script>

<style>
.ant-space {
   height: 32px;
   flex: 1 1 550px;
   min-width: 0px;
   transition: all 0.2s;

}

.ant-space-item {

   width: 100%;
}

.ant-picker {
   border-radius: 4px !important;
}
</style>