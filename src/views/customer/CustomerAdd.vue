<template>
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
            <ms-button
                type="outline-primary"
                @click="createAndResetCustomer"
                :loading="isSavingAndAdd"
                >Lưu và thêm</ms-button
            >
            <ms-button type="primary" @click="createCustomer" :loading="isSaving">Lưu</ms-button>
        </div>
    </div>
    <div class="content flex1">
        <div class="title-form title-img">Ảnh</div>

        <input
            ref="fileInput"
            type="file"
            accept="image/*"
            style="display: none"
            @change="handleFileChange"
        />
        <div class="input-img icon-input-img" @click="openFilePicker">
            <img v-if="previewUrl" :src="previewUrl" alt="preview" class="preview-img" />
        </div>

        <div class="title-form title-info">Thông tin chung</div>
        <div class="d-flex">
            <div class="form-body">
                <div class="d-flex gap80">
                    <div class="flex-item">
                        <ms-input-text
                            label="Mã khách hàng"
                            :column="false"
                            readonly
                            v-model="formData.crmCustomerCode"
                        ></ms-input-text>
                        <ms-input-text
                            label="Tên khách hàng"
                            :column="false"
                            v-model="formData.crmCustomerName"
                            ref="nameRef"
                            required
                        ></ms-input-text>
                        <ms-input-text
                            label="Email"
                            :field="'crmCustomerEmail'"
                            :column="false"
                            ref="emailRef"
                            v-model="formData.crmCustomerEmail"
                            @blur-check="handleBlurEmail"
                            required
                        ></ms-input-text>
                        <ms-input-text
                            label="Ngày mua gần nhất"
                            :column="false"
                            v-model="formData.crmCustomerLastPurchaseDate"
                            :type="'date'"
                        >
                        </ms-input-text>
                        <ms-input-text
                            label="Mã hàng hóa"
                            :column="false"
                            v-model="formData.crmCustomerPurchasedItemCode"
                        ></ms-input-text>
                        <ms-input-text
                            label="Mã số thuế"
                            :column="false"
                            v-model="formData.crmCustomerTaxCode"
                        ></ms-input-text>
                    </div>
                    <div class="flex-item">
                        <ms-input-text
                            label="Loại khách hàng"
                            :column="false"
                            v-model="formData.crmCustomerType"
                            :options="crmCustomerTypeOption"
                            :type="'select'"
                        ></ms-input-text>
                        <ms-input-text
                            label="Số điện thoại"
                            :field="'crmCustomerPhoneNumber'"
                            :column="false"
                            ref="phoneRef"
                            v-model="formData.crmCustomerPhoneNumber"
                            @blur-check="handleBlurPhone"
                            required
                        ></ms-input-text>
                        <ms-input-text
                            label="Địa chỉ liên hệ"
                            :column="false"
                            v-model="formData.crmCustomerAddress"
                        ></ms-input-text>

                        <ms-input-text
                            label="Địa chỉ (Giao hàng)"
                            :column="false"
                            v-model="formData.crmCustomerShippingAddress"
                        ></ms-input-text>
                        <ms-input-text
                            label="Tên hàng hóa đã mua"
                            :column="false"
                            v-model="formData.crmCustomerPurchasedItemName"
                        ></ms-input-text>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
// #region Imports & Init
import MsTextColor from '@/components/ms-button/MsTextColor.vue'
import MsButton from '@/components/ms-button/MsButton.vue'
import MsInputText from '../../components/ms-input/MsInputText.vue'
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js' // Đặt import API gần nơi sử dụng
import { ref, onMounted, nextTick } from 'vue'
import { useToastMessage } from '@/composables/useToastMessage'
import { useRouter } from 'vue-router' // Chỉ cần useRouter
// #endregion

// #region Dependencies
const { showToastSuccess, showToastError, showToastInfo } = useToastMessage()
const router = useRouter()
// #endregion

// #region State Data & Refs
/** Ref cho input Tên Khách hàng (để focus) */
const nameRef = ref(null)
/** Ref cho input Email (để gọi setError/clearError) */
const emailRef = ref(null)
/** Ref cho input Số điện thoại (để gọi setError/clearError) */
const phoneRef = ref(null)

/** Ref cho input file ẩn */
const fileInput = ref(null)
/** URL tạm thời để hiển thị ảnh preview */
const previewUrl = ref(null)

/** Trạng thái loading cho nút Lưu */
const isSaving = ref(false)
/** Trạng thái loading cho nút Lưu và Thêm */
const isSavingAndAdd = ref(false)

/**
 * Dữ liệu form thêm mới/chỉnh sửa khách hàng.
 * @type {import('vue').Ref<Object>}
 */
const formData = ref({
    crmCustomerCode: '', // Giá trị mặc định, có thể fetch từ API
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
    crmCustomerImage: '',
})

/**
 * Danh sách options cho loại khách hàng.
 * @type {import('vue').Ref<Array<Object>>}
 */
const crmCustomerTypeOption = ref([
    { value: 'NBH01', label: 'NBH01' },
    { value: 'LKHA', label: 'LKHA' },
    { value: 'VIP', label: 'VIP' },
])
// #endregion

// #region Lifecycle Hooks
/**
 * Hook được gọi sau khi component được mount vào DOM.
 * Thực hiện lấy mã khách hàng mới và focus vào trường Tên.
 */
onMounted(() => {
    fetchCustomerCode() // Gọi khi component mount
    // Sử dụng nextTick để chắc chắn DOM đã render xong trước khi focus
    nextTick(() => {
        if (nameRef.value?.$el) {
            // $el là DOM element của component
            nameRef.value.$el.querySelector('input')?.focus()
        }
    })
})
// #endregion

// #region Methods - Navigation & Utility

/**
 * Chuyển hướng người dùng về trang danh sách khách hàng.
 * createdby: TMHieu - 09.12.2025
 */
function backList() {
    router.push('/customer/list')
}

/**
 * Mở hộp thoại chọn file ảnh.
 * createdby: TMHieu - 09.12.2025
 */
const openFilePicker = () => {
    fileInput.value.click()
}

/**
 * Chuyển đổi chuỗi ngày tháng (DD/MM/YYYY) sang định dạng ISO 8601 UTC.
 * @param {string} dateStr - Chuỗi ngày tháng ở định dạng DD/MM/YYYY.
 * @returns {string} Chuỗi ngày tháng ở định dạng ISO 8601 UTC.
 * createdby: TMHieu - 09.12.2025
 */
function convertToISO(dateStr) {
    const [day, month, year] = dateStr.split('/').map(Number)
    const now = new Date()

    const date = new Date(
        Date.UTC(
            year,
            month - 1,
            day,
            now.getHours(),
            now.getMinutes(),
            now.getSeconds(),
            now.getMilliseconds(),
        ),
    )

    return date.toISOString()
}

/**
 * Reset form data về trạng thái ban đầu và lấy mã khách hàng mới.
 * createdby: TMHieu - 09.12.2025
 */
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
        crmCustomerImage: '',
    }

    // Xóa lỗi hiển thị (nếu có)
    emailRef.value?.clearError()
    phoneRef.value?.clearError()

    // Xóa preview ảnh
    previewUrl.value = null
    if (fileInput.value) {
        fileInput.value.value = '' // Clear file input
    }

    // Lấy lại mã khách hàng mới
    fetchCustomerCode()
}
// #endregion

// #region Methods - File Upload
/**
 * Xử lý sự kiện khi người dùng chọn file ảnh.
 * Kiểm tra định dạng và dung lượng file, sau đó tạo URL preview.
 * @param {Event} e - Sự kiện change từ input file.
 * createdby: TMHieu - 09.12.2025
 */
const handleFileChange = (e) => {
    const file = e.target.files[0]
    if (!file) return

    // 1) Kiểm tra loại file
    if (!file.type.startsWith('image/')) {
        showToastError('Tệp không phải ảnh!')
        e.target.value = ''
        return
    }

    // 2) Kiểm tra dung lượng < 32MB
    const maxSize = 32 * 1024 * 1024
    if (file.size > maxSize) {
        showToastError('Ảnh vượt quá 32MB!')
        e.target.value = ''
        return
    }

    // 3) Tạo preview
    previewUrl.value = URL.createObjectURL(file)

    return
}
// #endregion

// #region Methods - Validation & Check Duplicate

/**
 * Gọi API kiểm tra trùng lặp cho các trường.
 * @param {Object} payload - Dữ liệu check blur từ MsInputText ({field, value, error}).
 * @param {import('vue').Ref<any>} inRef - Ref của MsInputText tương ứng để gọi setError.
 * createdby: TMHieu - 09.12.2025
 */
function validateInput(payload, inRef) {
    // Nếu đã có lỗi required từ component con, không gọi API check duplicate nữa
    if (payload.error) return

    const apiPayload = {
        propertyName: payload.field,
        value: payload.value,
        ignoreId: null, // hoặc lấy từ formData nếu edit
    }

    CustomersAPI.checkDuplicate(apiPayload).catch((err) => {
        // Bắt lỗi status 400 (Custom Error) từ BE
        const userMsg = err?.response?.data?.error?.userMsg
        if (userMsg) {
            // Hiển thị lỗi trùng lặp lên input
            inRef.value?.setError(userMsg)
        }
        return
    })
}

/**
 * Xử lý validation và check trùng lặp cho Email khi blur.
 * @param {Object} payload - Dữ liệu check blur từ MsInputText.
 * createdby: TMHieu - 09.12.2025
 */
function handleBlurEmail(payload) {
    const value = payload.value?.trim() || ''

    // 1. Check rỗng
    if (!value) {
        emailRef.value?.setError('Không được để trống email')
        return
    }

    // 2. Check định dạng email chuẩn
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    if (!emailRegex.test(value)) {
        emailRef.value?.setError('Email không đúng định dạng')
        return
    }

    // 3. Nếu qua các check cơ bản, gọi API check trùng lặp
    validateInput(payload, emailRef)

    // 4. Nếu ok thì clear lỗi (nếu không bị API bắt lỗi)
    emailRef.value?.clearError()
}

/**
 * Xử lý validation và check trùng lặp cho Số điện thoại khi blur.
 * @param {Object} payload - Dữ liệu check blur từ MsInputText.
 * createdby: TMHieu - 09.12.2025
 */
function handleBlurPhone(payload) {
    const value = payload.value?.trim() || ''

    // 1. Check rỗng
    if (!value) {
        phoneRef.value?.setError('Không được để trống số điện thoại')
        return
    }

    // 2. Check chỉ chứa số
    const numberRegex = /^[0-9]+$/
    if (!numberRegex.test(value)) {
        phoneRef.value?.setError('Số điện thoại chỉ được chứa chữ số')
        return
    }

    // 3. Check độ dài 10-11
    if (value.length < 10 || value.length > 11) {
        phoneRef.value?.setError('Số điện thoại phải có 10 - 11 số')
        return
    }

    // 4. Nếu qua các check cơ bản, gọi API check trùng lặp
    validateInput(payload, phoneRef) // truyền thêm ref tương ứng

    // 5. Nếu ok thì clear lỗi (nếu không bị API bắt lỗi)
    phoneRef.value?.clearError()
}
// #endregion

// #region Methods - API Calls

/**
 * Lấy mã khách hàng mới nhất từ API.
 * createdby: TMHieu - 09.12.2025
 */
function fetchCustomerCode() {
    CustomersAPI.getNextCustomerCode()
        .then((res) => {
            if (res.status === 200) {
                formData.value.crmCustomerCode = res.data.customerCode
            } else {
                console.error('API error:', res.status)
            }
        })
        .catch((err) => {
            console.error('Fetch error:', err)
        })
}

/**
 * Thực hiện thêm mới khách hàng và chuyển hướng về danh sách.
 * createdby: TMHieu - 09.12.2025
 */
async function createCustomer() {
    isSaving.value = true

    // 1. Upload ảnh nếu có
    if (fileInput.value.files[0]) {
        const result = await CustomersAPI.uploadToImgBB(fileInput.value.files[0])
        if (!result.success) {
            showToastError('Tải ảnh lên thất bại')
            isSaving.value = false
            return
        }
        formData.value.crmCustomerImage = result.data.url
    }

    // 2. Chuẩn bị payload
    const payload = {
        ...formData.value,
        crmCustomerLastPurchaseDate: formData.value.crmCustomerLastPurchaseDate
            ? convertToISO(formData.value.crmCustomerLastPurchaseDate)
            : null,
    }

    // 3. Gọi API tạo mới
    CustomersAPI.create(payload)
        .then((res) => {
            if (res.status === 201 || res.status === 200) {
                showToastSuccess('Thêm mới khách hàng thành công')
                router.push('/customer/list')
            }
        })
        .catch((err) => {
            // Lỗi đã được xử lý chung trong Axios Interceptor
        })
        .finally(() => {
            // Tắt loading sau 0.5s để tạo hiệu ứng
            setTimeout(() => {
                isSaving.value = false
            }, 500)
        })
}

/**
 * Thực hiện thêm mới khách hàng, hiển thị toast, và reset form.
 * createdby: TMHieu - 09.12.2025
 */
async function createAndResetCustomer() {
    isSavingAndAdd.value = true

    // 1. Upload ảnh nếu có
    if (fileInput.value.files[0]) {
        const result = await CustomersAPI.uploadToImgBB(fileInput.value.files[0])
        if (!result.success) {
            showToastError('Tải ảnh lên thất bại')
            isSavingAndAdd.value = false
            return
        } else {
            formData.value.crmCustomerImage = result.data.url
        }
    }

    // 2. Chuẩn bị payload
    const payload = {
        ...formData.value,
        crmCustomerLastPurchaseDate: formData.value.crmCustomerLastPurchaseDate
            ? convertToISO(formData.value.crmCustomerLastPurchaseDate)
            : null,
    }

    // 3. Gọi API tạo mới
    CustomersAPI.create(payload)
        .then((res) => {
            if (res.status === 201 || res.status === 200) {
                showToastSuccess('Thêm mới khách hàng thành công')
                resetForm() // Reset form sau khi thành công
            }
        })
        .catch((err) => {
            // Lỗi đã được xử lý chung trong Axios Interceptor
        })
        .finally(() => {
            // Tắt loading sau 0.5s để tạo hiệu ứng
            setTimeout(() => {
                isSavingAndAdd.value = false
            }, 500)
        })
}
// #endregion
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
