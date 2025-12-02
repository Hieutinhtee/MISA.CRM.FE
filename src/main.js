import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import '@/assets/css/commons.css'
import '@/assets/css/icon.css'
import '@/assets/css/style.css'

import Antd from 'ant-design-vue';
import dayjs from 'dayjs';
import 'dayjs/locale/vi';

// set toàn hệ thống sang tiếng Việt
dayjs.locale('vi');
import PrimeVue from 'primevue/config';


const app = createApp(App)

app.use(Antd, {
   token: {
      borderRadius: 4
   }
});
app.use(PrimeVue);
app.use(router)

app.mount('#app')
