<template>
   <div :class="[
      'text-color',
      `text-color--${type}`
   ]">
      <slot></slot>
   </div>
</template>

<script setup>
//#region Props
const props = defineProps({
   type: {
      type: String,
      default: 'second'
   }
});
//#endregion
</script>

<style scoped>
.text-color {
   cursor: pointer;
   /* margin-left: 16px; */
   font-size: 13px;
}

.text-color--primary {
   color: var(--primary-color);
}

.text-color--second {
   color: #0f2fbd;
}

.text-color:hover {
   text-decoration: underline;
}
</style>