<template>



    <ConfigProvider :locale="viVN">
        <div class="container d-flex flex-column">
            <header-component></header-component>
            <router-view></router-view>

        </div>
    </ConfigProvider>
</template>

<script setup>
import HeaderComponent from '@/layouts/Header.vue';
import viVN from 'ant-design-vue/es/locale/vi_VN';
import { ConfigProvider } from 'ant-design-vue';

</script>

<style>
.container {
    position: absolute;
    width: 100%;
    height: 100vh;
}
</style>
