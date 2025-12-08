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
         <ms-button class="btn-cancel" type="outline">Hủy bỏ</ms-button>
         <ms-button type="outline-primary">Lưu và thêm</ms-button>
         <ms-button type="primary">Lưu</ms-button>
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
                  <ms-input-text label="Tên khách hàng" :column="false" v-model="formData.crmCustomerName"
                     required></ms-input-text>

               </div>
               <div class="flex-item">
                  <ms-input-text label="Loại khách hàng" :column="false" v-model="formData.crmCustomerType"
                     :options="crmCustomerTypeOption" :type="'select'"></ms-input-text>
                  <ms-input-text label="Mã số thuế" :column="false"
                     v-model="formData.crmCustomerTaxCode"></ms-input-text>

               </div>
            </div>


         </div>
      </div>
      <div class="title-form title-info">Thông tin giao hàng</div>
      <div class="d-flex">
         <div class="form-body">
            <div class="d-flex gap80">
               <div class="flex-item">
                  <ms-input-text label="Email" :column="false" v-model="formData.crmCustomerEmail"></ms-input-text>
                  <ms-input-text label="Địa chỉ liên hệ" :column="false"
                     v-model="formData.crmCustomerAddress"></ms-input-text>
               </div>
               <div class="flex-item">
                  <ms-input-text label="Số điện thoại" :column="false"
                     v-model="formData.crmCustomerPhoneNumber"></ms-input-text>
                  <ms-input-text label="Địa chỉ (Giao hàng)" :column="false"
                     v-model="formData.crmCustomerShippingAddress"></ms-input-text>
               </div>
            </div>
         </div>
      </div>
      <div class="title-form title-info">Thông tin hóa đơn</div>
      <div class="d-flex">
         <div class="form-body">
            <div class="d-flex gap80">
               <div class="flex-item">
                  <ms-input-text label="Ngày mua gần nhất" :column="false"
                     v-model="formData.crmCustomerLastPurchaseDate" :type="'date'">
                  </ms-input-text>
                  <ms-input-text label="Mã hàng hóa" :column="false"
                     v-model="formData.crmCustomerPurchasedItemCode"></ms-input-text>
               </div>
               <div class="flex-item">
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
import { ref, onMounted } from 'vue';

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
   crmCustomerPurchasedItemCode: ''
});

const crmCustomerTypeOption = ref([
   { value: 'NBH01', label: 'NBH01' },
   { value: 'LKHA', label: 'LKHA' },
   { value: 'VIP', label: 'VIP' }
]);

// API
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js';


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

onMounted(() => {
   fetchCustomerCode(); // Gọi khi component mount
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