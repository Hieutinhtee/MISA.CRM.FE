<template>
   <!-- Tool bar -->
   <div class="toolbar d-flex justify-content-between flex-col">
      <div class="left-bar d-flex align-content-center">
         <div class="title-header">Thêm Khách hàng</div>
         <div class="d-flex align-content-center m-r-8">
            <div class="dropdown-select-layout">Mẫu tiêu chuẩn</div>
            <div class="icon-down"></div>
         </div>
         <ms-text-color type="primary">Sửa bố cục</ms-text-color>
      </div>
      <div class="right-bar d-flex align-content-center">
         <ms-button class="btn-cancel" type="outline" @click="backList">Hủy bỏ</ms-button>
         <ms-button type="outline-primary" @click="createAndResetCustomer">Lưu và thêm</ms-button>
         <ms-button type="primary" @click="createCustomer">Lưu</ms-button>
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
                     v-model="formData.crmCustomerCode"></ms-input-text>
                  <ms-input-text label="Tên khách hàng" :column="false" v-model="formData.crmCustomerName" ref="nameRef"
                     required></ms-input-text>
                  <ms-input-text label="Email" :field="'crmCustomerEmail'" :column="false" ref="emailRef"
                     v-model="formData.crmCustomerEmail" @blur-check="handleBlurEmail"></ms-input-text>
                  <ms-input-text label="Ngày mua gần nhất" :column="false"
                     v-model="formData.crmCustomerLastPurchaseDate" :type="'date'">
                  </ms-input-text>
                  <ms-input-text label="Mã hàng hóa" :column="false"
                     v-model="formData.crmCustomerPurchasedItemCode"></ms-input-text>
                  <ms-input-text label="Mã số thuế" :column="false"
                     v-model="formData.crmCustomerTaxCode"></ms-input-text>


               </div>
               <div class="flex-item">
                  <ms-input-text label="Loại khách hàng" :column="false" v-model="formData.crmCustomerType"
                     :options="crmCustomerTypeOption" :type="'select'"></ms-input-text>
                  <ms-input-text label="Số điện thoại" :field="'crmCustomerPhoneNumber'" :column="false" ref="phoneRef"
                     v-model="formData.crmCustomerPhoneNumber" @blur-check="handleBlurPhone"></ms-input-text>
                  <ms-input-text label="Địa chỉ liên hệ" :column="false"
                     v-model="formData.crmCustomerAddress"></ms-input-text>

                  <ms-input-text label="Địa chỉ (Giao hàng)" :column="false"
                     v-model="formData.crmCustomerShippingAddress"></ms-input-text>
                  <ms-input-text label="Tên hàng hóa đã mua" :column="false"
                     v-model="formData.crmCustomerPurchasedItemName"></ms-input-text>
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
import { ref, onMounted, nextTick } from 'vue';
import { useToastMessage } from '@/composables/useToastMessage';

const { showToastSuccess, showToastError, showToastInfo } = useToastMessage();
import { useRouter } from 'vue-router';
const router = useRouter();

const nameRef = ref(null);

const formData = ref({
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

function backList() {
   router.push('/customer/list');

}
// API
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js';

function validateInput(payload, inRef) {
   const apiPayload = {
      propertyName: payload.field,
      value: payload.value,
      ignoreId: null // hoặc lấy từ formData nếu edit
   };

   CustomersAPI.checkDuplicate(apiPayload)
      .catch(err => {
         // Bắt lỗi status 400 từ BE
         const userMsg = err?.response?.data?.error?.userMsg;
         if (userMsg) {
            inRef.value?.setError(userMsg);
         }
         return
      });
}

function fetchCustomerCode() {
   CustomersAPI.getNextCustomerCode()
      .then(res => {
         if (res.status === 200) {
            formData.value.crmCustomerCode = res.data.customerCode;
         } else {
            console.error("API error:", res.status);
         }
      })
      .catch(err => {
         console.error("Fetch error:", err);
      });
}

function createCustomer() {
   const payload = { ...formData.value, crmCustomerLastPurchaseDate: convertToISO(formData.value.crmCustomerLastPurchaseDate) };
   CustomersAPI.create(payload).then(res => {
      if (res.status === 201 || res.status === 200) {
         showToastSuccess("Thêm mới khách hàng thành công")
         router.push('/customer/list');
      }
   })
      .catch();
}

function convertToISO(dateStr) {
   const [day, month, year] = dateStr.split("/").map(Number);
   const now = new Date();

   const date = new Date(Date.UTC(
      year, month - 1, day,
      now.getHours(),
      now.getMinutes(),
      now.getSeconds(),
      now.getMilliseconds()
   ));

   return date.toISOString();
}

function resetForm() {
   formData.value = {
      crmCustomerCode: '',
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
   };

   // Xóa lỗi
   emailRef.value?.clearError();
   phoneRef.value?.clearError();

   // Lấy lại mã khách hàng mới
   fetchCustomerCode();
}

// Lưu và reset
function createAndResetCustomer() {
   const payload = { ...formData.value, crmCustomerLastPurchaseDate: convertToISO(formData.value.crmCustomerLastPurchaseDate) };
   CustomersAPI.create(payload)
      .then(res => {
         if (res.status === 201 || res.status === 200) {
            showToastSuccess("Thêm mới khách hàng thành công")
            resetForm(); // reset toàn bộ input
         }
      })
      .catch();
}


onMounted(() => {
   fetchCustomerCode(); // Gọi khi component mount
   // Sử dụng nextTick để chắc chắn DOM đã render xong
   nextTick(() => {
      if (nameRef.value?.$el) {
         // $el là DOM element của component
         nameRef.value.$el.querySelector('input')?.focus();
      }
   });
});
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
   margin-bottom: 24px;
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