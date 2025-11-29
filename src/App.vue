<template>

  <ConfigProvider :locale="viVN">
    <div class="container d-flex flex-column">
      <header-component></header-component>
      <div class="content d-flex flex1 flex-row">

      </div>
    </div>
  </ConfigProvider>
</template>

<script setup>
import HeaderComponent from '@/layouts/Header.vue';
import viVN from 'ant-design-vue/es/locale/vi_VN';
import { ConfigProvider } from 'ant-design-vue';

</script>

<style scoped>
.container {
  position: absolute;
  width: 100%;
  height: 100vh;
}

.content {
  background-color: #ebecef;
  z-index: 0;
}
</style>
