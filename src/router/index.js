import { createRouter, createWebHistory } from 'vue-router';
import CustomerList from '../views/customer/CustomerList.vue';
import CustomerAdd from '../views/customer/CustomerAdd.vue';
import CustomerEdit from '../views/customer/CustomerEdit.vue';

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
  {
    path: '/customer/edit/:id',
    component: CustomerEdit
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
