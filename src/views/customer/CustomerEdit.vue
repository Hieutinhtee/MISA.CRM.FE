<template>
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
            <ms-button type="outline-primary" @click="updateAndAdd" :loading="isSavingAndAdd"
                >Lưu và thêm</ms-button
            >
            <ms-button type="primary" @click="editCustomer" :loading="isSaving">Lưu</ms-button>
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
            <img
                v-show="previewUrl || formDataEdit.crmCustomerImage"
                :src="previewUrl || formDataEdit.crmCustomerImage"
                alt="preview"
                class="preview-img"
            />
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
                            v-model="formDataEdit.crmCustomerCode"
                        ></ms-input-text>
                        <ms-input-text
                            label="Tên khách hàng"
                            :column="false"
                            v-model="formDataEdit.crmCustomerName"
                            ref="nameRef"
                            required
                        ></ms-input-text>
                        <ms-input-text
                            label="Email"
                            :field="'crmCustomerEmail'"
                            :column="false"
                            ref="emailRef"
                            v-model="formDataEdit.crmCustomerEmail"
                            @blur-check="handleBlurEmail"
                            required
                        ></ms-input-text>
                        <ms-input-text
                            label="Ngày mua gần nhất"
                            :column="false"
                            v-model="formDataEdit.crmCustomerLastPurchaseDate"
                            :type="'date'"
                        >
                        </ms-input-text>
                        <ms-input-text
                            label="Mã hàng hóa"
                            :column="false"
                            v-model="formDataEdit.crmCustomerPurchasedItemCode"
                        ></ms-input-text>
                        <ms-input-text
                            label="Mã số thuế"
                            :column="false"
                            v-model="formDataEdit.crmCustomerTaxCode"
                        ></ms-input-text>
                    </div>
                    <div class="flex-item">
                        <ms-input-text
                            label="Loại khách hàng"
                            :column="false"
                            v-model="formDataEdit.crmCustomerType"
                            :options="crmCustomerTypeOption"
                            :type="'select'"
                        ></ms-input-text>
                        <ms-input-text
                            label="Số điện thoại"
                            :field="'crmCustomerPhoneNumber'"
                            :column="false"
                            ref="phoneRef"
                            v-model="formDataEdit.crmCustomerPhoneNumber"
                            @blur-check="handleBlurPhone"
                            required
                        ></ms-input-text>
                        <ms-input-text
                            label="Địa chỉ liên hệ"
                            :column="false"
                            v-model="formDataEdit.crmCustomerAddress"
                        ></ms-input-text>

                        <ms-input-text
                            label="Địa chỉ (Giao hàng)"
                            :column="false"
                            v-model="formDataEdit.crmCustomerShippingAddress"
                        ></ms-input-text>
                        <ms-input-text
                            label="Tên hàng hóa đã mua"
                            :column="false"
                            v-model="formDataEdit.crmCustomerPurchasedItemName"
                        ></ms-input-text>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
// Imports & Init
import MsTextColor from '@/components/ms-button/MsTextColor.vue'
import MsButton from '@/components/ms-button/MsButton.vue'
import MsInputText from '../../components/ms-input/MsInputText.vue'
import dayjs from 'dayjs'
import { useRoute, useRouter } from 'vue-router'
import { ref, onMounted, watch } from 'vue'
import CustomersAPI from '@/apis/components/customers/CustomersAPI.js'
import { useToastMessage } from '@/composables/useToastMessage'

const { showToastSuccess, showToastError } = useToastMessage()
const router = useRouter()
const route = useRoute()
const id = route.params.id // Lấy ID khách hàng từ URL

//#region State Data
// Dữ liệu form chỉnh sửa
const formDataEdit = ref({
    crmCustomerCode: '',
    crmCustomerType: '',
    crmCustomerName: '',
    crmCustomerTaxCode: '',
    crmCustomerEmail: '',
    crmCustomerPhoneNumber: '',
    crmCustomerAddress: '',
    crmCustomerShippingAddress: '',
    crmCustomerLastPurchaseDate: '', // Dùng dayjs object
    crmCustomerPurchasedItemName: '',
    crmCustomerPurchasedItemCode: '',
    crmCustomerImage: '',
})

// Trạng thái nút Loading
const isSaving = ref(false)
const isSavingAndAdd = ref(false)

// Ref cho input file và preview ảnh
const fileInput = ref(null)
const previewUrl = ref(null)

// Ref cho các component input để gọi hàm validate
const emailRef = ref(null)
const phoneRef = ref(null)

// Dữ liệu tĩnh cho Select
const crmCustomerTypeOption = ref([
    { value: 'NBH01', label: 'NBH01' },
    { value: 'LKHA', label: 'LKHA' },
    { value: 'VIP', label: 'VIP' },
])
//#endregion

//#region Lifecycle
/**
 * Lifecycle Hook: Được gọi khi component được mount.
 * Thực hiện gọi API để lấy dữ liệu khách hàng theo ID và gán vào form.
 * createdby: TMHieu - 09.12.2025
 */
onMounted(async () => {
    try {
        const res = await CustomersAPI.getById(id)
        const data = res.data.data

        // Chuyển đổi ngày tháng từ API (nếu có) sang đối tượng dayjs để gán vào input date
        const dateConverted = data.crmCustomerLastPurchaseDate
            ? dayjs(data.crmCustomerLastPurchaseDate)
            : ''

        // Map data từ API vào form
        formDataEdit.value = {
            ...formDataEdit.value, // Giữ các trường mặc định
            ...data,
            crmCustomerLastPurchaseDate: dateConverted,
        }
    } catch (err) {
        console.error('Lỗi khi load dữ liệu khách hàng:', err)
        showToastError('Không tìm thấy dữ liệu khách hàng')
    }
})
//#endregion

//#region Methods - Navigation

/**
 * Hàm quay lại trang danh sách khách hàng
 * createdby: TMHieu - 09.12.2025
 */
function backToList() {
    router.push('/customer/list')
}
//#endregion

//#region Methods - Data Manipulation (Update & Save)

/**
 * Hàm chuyển đối tượng ngày tháng sang định dạng ISO string chuẩn cho API.
 * @param {dayjs|string} dateObj - Đối tượng ngày tháng hoặc chuỗi ngày tháng.
 * @returns {string|null} Chuỗi ISO 8601 hoặc null.
 * createdby: TMHieu - 09.12.2025
 */
function convertToISO(dateObj) {
    if (!dateObj) return null
    return dayjs(dateObj).toISOString()
}

/**
 * Xử lý logic chung cho việc upload ảnh và cập nhật dữ liệu khách hàng.
 * @param {Function} successCallback - Hàm callback sau khi cập nhật thành công (ví dụ: chuyển trang).
 * @param {Ref} loadingState - Biến ref quản lý trạng thái loading.
 * createdby: TMHieu - 09.12.2025
 */
async function processUpdate(successCallback, loadingState) {
    loadingState.value = true

    // 1. Xử lý Upload ảnh (nếu có file mới)
    if (fileInput.value.files[0]) {
        try {
            const result = await CustomersAPI.uploadToImgBB(fileInput.value.files[0])
            if (!result.success) {
                showToastError('Tải ảnh lên thất bại')
                loadingState.value = false
                return
            }
            formDataEdit.value.crmCustomerImage = result.data.url
        } catch (error) {
            showToastError('Lỗi kết nối khi tải ảnh')
            loadingState.value = false
            return
        }
    }

    // 2. Chuẩn bị Payload
    const payload = {
        ...formDataEdit.value,
        crmCustomerLastPurchaseDate: formDataEdit.value.crmCustomerLastPurchaseDate
            ? convertToISO(formDataEdit.value.crmCustomerLastPurchaseDate)
            : null,
    }

    // 3. Gọi API Update
    CustomersAPI.update(id, payload)
        .then((res) => {
            showToastSuccess('Sửa thành công!')
            successCallback()
        })
        .catch((err) => {
            showToastError('Sửa thất bại. Vui lòng kiểm tra lại dữ liệu.')
            console.error(err)
        })
        .finally(() => {
            // Tắt loading sau 0.5s để có cảm giác hoàn thành
            setTimeout(() => {
                loadingState.value = false
            }, 500)
        })
}

/**
 * Hàm xử lý sự kiện click nút "Lưu"
 * Chuyển hướng về trang danh sách sau khi lưu thành công.
 * createdby: TMHieu - 09.12.2025
 */
async function editCustomer() {
    await processUpdate(() => router.push('/customer/list'), isSaving)
}

/**
 * Hàm xử lý sự kiện click nút "Lưu và thêm"
 * Chuyển hướng về trang thêm mới sau khi lưu thành công.
 * createdby: TMHieu - 09.12.2025
 */
async function updateAndAdd() {
    await processUpdate(() => router.push('/customer/add'), isSavingAndAdd)
}
//#endregion

//#region Methods - Image Handling

/**
 * Mở hộp thoại chọn tệp (File Picker) khi click vào vùng ảnh.
 * createdby: TMHieu - 09.12.2025
 */
const openFilePicker = () => {
    fileInput.value.click()
}

/**
 * Xử lý khi người dùng chọn file ảnh mới.
 * Thực hiện kiểm tra định dạng, dung lượng và tạo URL preview.
 * @param {Event} e - Sự kiện change của input file.
 * createdby: TMHieu - 09.12.2025
 */
const handleFileChange = (e) => {
    const file = e.target.files[0]
    if (!file) return

    // 1) Kiểm tra loại file (chỉ cho phép ảnh)
    if (!file.type.startsWith('image/')) {
        showToastError('Tệp không phải ảnh!')
        e.target.value = '' // Reset input file
        return
    }

    // 2) Kiểm tra dung lượng (< 32MB)
    const maxSize = 32 * 1024 * 1024
    if (file.size > maxSize) {
        showToastError('Ảnh vượt quá 32MB!')
        e.target.value = ''
        return
    }

    // 3) Tạo preview
    if (previewUrl.value) {
        URL.revokeObjectURL(previewUrl.value) // Xóa URL cũ để giải phóng bộ nhớ
    }
    previewUrl.value = URL.createObjectURL(file)
}
//#endregion

//#region Methods - Validation & Check Duplicate

/**
 * Hàm gọi API kiểm tra trùng lặp (ví dụ: Email, Phone) và hiển thị lỗi.
 * @param {Object} payload - Chứa propertyName, value, field.
 * @param {Ref} inRef - Ref của component MsInputText tương ứng.
 * createdby: TMHieu - 09.12.2025
 */
function validateInput(payload, inRef) {
    const apiPayload = {
        propertyName: payload.field,
        value: payload.value,
        ignoreId: id,
    }

    CustomersAPI.checkDuplicate(apiPayload).catch((err) => {
        const userMsg = err?.response?.data?.error?.userMsg
        if (userMsg) {
            inRef.value?.setError(userMsg)
        }
        return
    })
}

/**
 * Xử lý validate Email khi blur khỏi input.
 * @param {Object} payload - Chứa field và value của input.
 * createdby: TMHieu - 09.12.2025
 */
function handleBlurEmail(payload) {
    const value = payload.value?.trim() || ''
    emailRef.value?.clearError() // Xóa lỗi cũ

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

    // 3. Check trùng lặp
    validateInput(payload, emailRef)
}

/**
 * Xử lý validate Số điện thoại khi blur khỏi input.
 * @param {Object} payload - Chứa field và value của input.
 * createdby: TMHieu - 09.12.2025
 */
function handleBlurPhone(payload) {
    const value = payload.value?.trim() || ''
    phoneRef.value?.clearError() // Xóa lỗi cũ

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

    // 4. Check trùng lặp
    validateInput(payload, phoneRef)
}
//#endregion
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
    position: relative;
}

.loading-img {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
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
