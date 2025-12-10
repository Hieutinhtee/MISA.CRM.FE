<template>
   <!-- Tool bar -->
   <div class="toolbar d-flex justify-content-between flex-col">
      <div class="left-bar d-flex align-content-center">
         <div class="title-header">Sửa Khách hàng</div>
         <div class="d-flex align-content-center m-r-8">
            <div class="dropdown-select-layout">Mẫu tiêu chuẩn</div>
            <div class="icon-down"></div>
         </div>
         <ms-text-color type="primary">Sửa bố cục</ms-text-color>
      </div>
      <div class="right-bar d-flex align-content-center">
         <ms-button class="btn-cancel" type="outline" @click="backToList">Hủy bỏ</ms-button>
         <ms-button type="outline-primary" @click="updateAndReset">Lưu và thêm</ms-button>
         <ms-button type="primary" @click="editCustomer">Lưu</ms-button>
      </div>
   </div>
   <div class="content flex1">
      <div class="title-form title-img">Ảnh</div>
      <div class="input-img icon-input-img"></div>
      <div class="title-form title-info">Thông tin chung</div>
      <div class="d-flex">
         <div class="form-body">
            <div class="d-flex gap80">
               <div class="flex-item">
                  <ms-input-text label="Mã khách hàng" :column="false" readonly
                     v-model="formDataEdit.crmCustomerCode"></ms-input-text>
                  <ms-input-text label="Tên khách hàng" :column="false" v-model="formDataEdit.crmCustomerName"
                     ref="nameRef" required></ms-input-text>
                  <ms-input-text label="Email" :field="'crmCustomerEmail'" :column="false" ref="emailRef"
                     v-model="formDataEdit.crmCustomerEmail" @blur-check="handleBlurEmail" required></ms-input-text>
                  <ms-input-text label="Ngày mua gần nhất" :column="false"
                     v-model="formDataEdit.crmCustomerLastPurchaseDate" :type="'date'">
                  </ms-input-text>
                  <ms-input-text label="Mã hàng hóa" :column="false"
                     v-model="formDataEdit.crmCustomerPurchasedItemCode"></ms-input-text>
                  <ms-input-text label="Mã số thuế" :column="false"
                     v-model="formDataEdit.crmCustomerTaxCode"></ms-input-text>

               </div>
               <div class="flex-item">
                  <ms-input-text label="Loại khách hàng" :column="false" v-model="formDataEdit.crmCustomerType"
                     :options="crmCustomerTypeOption" :type="'select'"></ms-input-text>
                  <ms-input-text label="Số điện thoại" :field="'crmCustomerPhoneNumber'" :column="false" ref="phoneRef"
                     v-model="formDataEdit.crmCustomerPhoneNumber" @blur-check="handleBlurPhone"
                     required></ms-input-text>
                  <ms-input-text label="Địa chỉ liên hệ" :column="false"
                     v-model="formDataEdit.crmCustomerAddress"></ms-input-text>

                  <ms-input-text label="Địa chỉ (Giao hàng)" :column="false"
                     v-model="formDataEdit.crmCustomerShippingAddress"></ms-input-text>
                  <ms-input-text label="Tên hàng hóa đã mua" :column="false"
                     v-model="formDataEdit.crmCustomerPurchasedItemName"></ms-input-text>
               </div>
            </div>


         </div>


      </div>
   </div>
</template>


<script setup>
import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsInputText from '../../components/ms-input/MsInputText.vue';
import dayjs from 'dayjs'
import { useRoute, useRouter } from 'vue-router';
import { ref, onMounted } from 'vue';
import { useToastMessage } from '@/composables/useToastMessage';
const { showToastSuccess, showToastError, showToastInfo } = useToastMessage();


const router = useRouter();
const route = useRoute()
const id = route.params.id

const formDataEdit = ref({
   crmCustomerCode: '',  // Giá trị mặc định, có thể fetch từ API
   crmCustomerType: '',
   crmCustomerName: '',
   crmCustomerTaxCode: '',
   crmCustomerEmail: '',
   crmCustomerPhoneNumber: '',
   crmCustomerAddress: '',
   crmCustomerShippingAddress: '',
   crmCustomerLastPurchaseDate: '',
   crmCustomerPurchasedItemName: '',
   crmCustomerPurchasedItemCode: '',
   crmCustomerImage: 'img'
});

const crmCustomerTypeOption = ref([
   { value: 'NBH01', label: 'NBH01' },
   { value: 'LKHA', label: 'LKHA' },
   { value: 'VIP', label: 'VIP' }
]);

// API
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js';

function editCustomer() {
   const payload = { ...formDataEdit.value, crmCustomerLastPurchaseDate: convertToISO(formDataEdit.value.crmCustomerLastPurchaseDate), crmCustomerImage: 'img' };
   CustomersAPI.update(id, payload).then(res => {
      showToastSuccess("Sửa thành công!");

   })
      .catch();
}

function validateInput(payload, inRef) {
   const apiPayload = {
      propertyName: payload.field,
      value: payload.value,
      ignoreId: id // hoặc lấy từ formData nếu edit
   };

   CustomersAPI.checkDuplicate(apiPayload)
      .catch(err => {
         const userMsg = err?.response?.data?.error?.userMsg;
         if (userMsg) {
            inRef.value?.setError(userMsg);
         }
         return
      });
}

//Chuyển kiểu date
function convertToISO(dateObj) {
   if (!dateObj) return null;
   return dayjs(dateObj).toISOString();
}

//Quay lại trang list
function backToList() {
   router.push('/customer/list');
}

//Validate khi blur
const emailRef = ref(null);
const phoneRef = ref(null);

function handleBlurEmail(payload) {
   const value = payload.value?.trim() || "";

   // 1. Check rỗng
   if (!value) {
      emailRef.value?.setError("Không được để trống email");
      return;
   }

   // 2. Check định dạng email chuẩn
   const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
   if (!emailRegex.test(value)) {
      emailRef.value?.setError("Email không đúng định dạng");
      return;
   }

   validateInput(payload, emailRef);

   // 3. Nếu ok thì clear lỗi
   emailRef.value?.clearError();
}

function handleBlurPhone(payload) {
   const value = payload.value?.trim() || "";

   // 1. Check rỗng
   if (!value) {
      phoneRef.value?.setError("Không được để trống số điện thoại");
      return;
   }

   // 2. Check chỉ chứa số
   const numberRegex = /^[0-9]+$/;
   if (!numberRegex.test(value)) {
      phoneRef.value?.setError("Số điện thoại chỉ được chứa chữ số");
      return;
   }

   // 3. Check độ dài 10-11
   if (value.length < 10 || value.length > 11) {
      phoneRef.value?.setError("Số điện thoại phải có 10 - 11 số");
      return;
   }


   validateInput(payload, phoneRef); // truyền thêm ref tương ứng

   // 5. Nếu ok thì clear lỗi
   phoneRef.value?.clearError();
}

function updateAndReset() {
   const payload = { ...formDataEdit.value, crmCustomerLastPurchaseDate: convertToISO(formDataEdit.value.crmCustomerLastPurchaseDate), crmCustomerImage: 'img' };
   CustomersAPI.update(id, payload).then(res => {
      showToastSuccess("Sửa thành công!");
      router.push('/customer/add');
   })
      .catch();
}

onMounted(async () => {
   const res = await CustomersAPI.getById(id)
   console.log(res);
   // Kiểm tra có ngày không để khỏi lỗi nhé (._.)
   const dateConverted = res.data.data.crmCustomerLastPurchaseDate
      ? dayjs(res.data.data.crmCustomerLastPurchaseDate)
      : '';

   // Map thẳng data từ API vào form
   formDataEdit.value = {
      ...formDataEdit.value,
      ...res.data.data,
      crmCustomerLastPurchaseDate: dateConverted,
      crmCustomerImage: 'img'
   }
})
</script>

<style scoped>
/* Toolbar */
.toolbar {
   padding: 12px 16px;
   background-color: #e2e4e9;
   border-bottom: 1px solid #d3d7de;
}

.title-header {
   font-size: 20px;
   margin-right: 16px;
   font-weight: 500;
   color: #1f2229e7;
}

.dropdown-select-layout {
   font-size: 16px;
   margin-top: 4px;
   margin-right: 8px;
}

.right-bar {
   gap: 10px;
}

.btn-cancel {
   padding-right: 16px;
}

/* Content */
.content {
   padding: 32px 56px 0px 56px;
   overflow: auto;
}

.title-form {
   font-size: 18px;
   font-weight: 500;
   margin-bottom: 16px;
   color: #1f2229e7;
}

.input-img {
   margin-bottom: 40px;
}

.title-info {
   margin-bottom: 18px;
}


.form-body {
   flex-basis: 1420px;
   margin-bottom: 24px;
}

.flex-item {
   flex: 1 1 200px;
   /* flex-grow | flex-shrink | flex-basis */
   min-width: 100px;
}

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