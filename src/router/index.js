import { createRouter, createWebHistory } from 'vue-router';
import CustomerList from '../views/customer/CustomerList.vue';
import CustomerAdd from '../views/customer/CustomerAdd.vue';

const routes = [
  {
    path: '/',
    redirect: '/customer/list'
  },
  {
    path: '/customer/list',
    component: CustomerList
  },
  {
    path: '/customer/add',
    component: CustomerAdd
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
