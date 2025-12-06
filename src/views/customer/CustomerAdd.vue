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
            <div class="d-flex w-100 gap80">
               <ms-input-text label="Mã khách hàng" :column="false" readonly
                  v-model="formData.customerCode"></ms-input-text>
               <ms-input-text label="Loại khách hàng" :column="false" v-model="formData.customerType"></ms-input-text>
            </div>
            <div class="d-flex w-100 gap80">
               <ms-input-text label="Tên khách hàng" :column="false" v-model="formData.fullName"
                  required></ms-input-text>
               <ms-input-text label="Mã số thuế" :column="false" v-model="formData.taxCode"></ms-input-text>
            </div>
            <div class="d-flex w-100 gap80">
               <ms-input-text label="Email" :column="false" v-model="formData.email"></ms-input-text>
               <ms-input-text label="Số điện thoại" :column="false" v-model="formData.phone"></ms-input-text>
            </div>
            <div class="d-flex w-100 gap80">
               <ms-input-text label="Địa chỉ liên hệ" :column="false" v-model="formData.address"></ms-input-text>
               <ms-input-text label="Địa chỉ (Giao hàng)" :column="false"
                  v-model="formData.shippingAddress"></ms-input-text>
            </div>
            <div class="d-flex w-100 gap80">
               <ms-input-text label="Ngày mua gần nhất" :column="false"
                  v-model="formData.lastPurchaseDate"></ms-input-text>
               <ms-input-text label="Tên hàng hóa đã mua" :column="false"
                  v-model="formData.lastPurchasedItemName"></ms-input-text>
            </div>
            <div>{{ formData }}</div>
         </div>
      </div>
   </div>
</template>


<script setup>
import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsInputText from '../../components/ms-input/MsInputText.vue';
import { ref } from 'vue';

const formData = ref({
   customerCode: '',  // Giá trị mặc định, có thể fetch từ API
   customerType: '',
   customerName: '',
   taxCode: '',
   email: '',
   phone: '',
   contactAddress: '',
   deliveryAddress: '',
   lastPurchaseDate: '',
   purchasedItems: ''
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
}
</style>