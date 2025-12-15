<template>
    <div class="wrapper-input">
        <div
            class="ms-input-wrapper w-100"
            :class="props.column ? 'flex-column' : 'justify-content-between'"
        >
            <label v-if="props.label" :for="props.label" :class="{ required: props.required }">{{
                props.label
            }}</label>

            <a-select
                v-if="props.type === 'select'"
                show-search
                :options="props.options"
                :filter-option="filterOption"
                @blur="validate"
                :placeholder="placeholder || label"
                :value="model"
                @change="handleSelectChange"
                :class="{ 'input-error': !!errorMessage }"
            />

            <a-space v-else-if="props.type === 'date'">
                <a-date-picker
                    :value="model ? dayjs(model, dateFormatList[0]) : null"
                    @change="handleDateChange"
                    @blur="validate"
                    class="d-block"
                    :format="dateFormatList"
                    :placeholder="placeholder || label"
                    :class="{ 'input-error': !!errorMessage }"
                />
            </a-space>

            <input
                v-else
                :id="props.label"
                type="text"
                :placeholder="props.placeholder || ''"
                v-model="model"
                :class="{ 'input-error': !!errorMessage }"
                @blur="validate"
                :readonly="props.readonly"
            />
        </div>

        <div v-if="errorMessage" class="error-text d-flex">
            <div>{{ errorMessage }}</div>
        </div>
    </div>
</template>

<script setup>
// Imports
import { ref, watch, defineModel } from 'vue'
import dayjs from 'dayjs'
import customParseFormat from 'dayjs/plugin/customParseFormat'
dayjs.extend(customParseFormat)

//#region Props, Emits, Models

const props = defineProps({
    /** Label hiển thị cho input/select */
    label: String,
    /** Placeholder hiển thị */
    placeholder: String,
    /** Input có bắt buộc nhập không */
    required: { type: Boolean, default: false },
    /** Input có ở chế độ chỉ đọc không */
    readonly: { type: Boolean, default: false },
    /** Layout: true (label trên input) | false (label trái input) */
    column: { type: Boolean, default: true },
    /** Loại input: 'text', 'select', 'date' */
    type: String,
    /** Danh sách options cho type='select' */
    options: Array,
    /** Rules validation bổ sung (chưa dùng) */
    rules: { type: Array, default: () => [] },
    /** Lỗi từ bên ngoài truyền vào (ví dụ: lỗi trùng lặp từ API) */
    externalError: { type: String, default: '' },
    /** Tên field trong payload, dùng để truyền cho API validate trùng lặp */
    field: String,
})

const emit = defineEmits(['blur-check'])

// Model binding cho v-model
const model = defineModel({ required: true })

//#endregion

//#region State Data
const errorMessage = ref('')
const dateFormatList = ['DD/MM/YYYY', 'DD/MM/YY']

//#endregion

//#region Lifecycle
/**
 * Theo dõi `props.externalError` và cập nhật `errorMessage` khi có lỗi từ bên ngoài.
 * (Dùng cho check trùng lặp API)
 */
watch(
    () => props.externalError,
    (val) => {
        errorMessage.value = val
    },
)

/**
 * Theo dõi `model` (giá trị input)
 * Khi giá trị thay đổi, nếu đang có lỗi thì xóa lỗi đi (bắt đầu gõ lại).
 */
watch(model, () => {
    if (errorMessage.value) {
        errorMessage.value = '' // xóa lỗi khi người dùng bắt đầu gõ
    }
})

//#endregion

//#region Methods - Handling Input

/**
 * Hàm lọc mặc định cho Ant Design Select.
 * So sánh input của người dùng với label của option.
 * @param {string} input - Giá trị nhập vào của người dùng.
 * @param {Object} option - Đối tượng option đang được kiểm tra ({ value: string, label: string }).
 * @returns {boolean} True nếu option match với input.
 * createdby: TMHieu - 09.12.2025
 */
const filterOption = (input, option) => {
    return option.label.toLowerCase().includes(input.toLowerCase())
}

/**
 * Xử lý khi giá trị Select thay đổi.
 * @param {*} value - Giá trị mới được chọn.
 * createdby: TMHieu - 09.12.2025
 */
function handleSelectChange(value) {
    model.value = value
}

/**
 * Xử lý khi giá trị DatePicker thay đổi.
 * @param {dayjs.Dayjs|null} date - Đối tượng Dayjs (không dùng ở đây).
 * @param {string} dateString - Chuỗi ngày đã được format ("DD/MM/YYYY").
 * createdby: TMHieu - 09.12.2025
 */
function handleDateChange(date, dateString) {
    model.value = dateString // Lưu dạng string "DD/MM/YYYY"
}

//#endregion

//#region Methods - Validation

/**
 * Hàm kiểm tra validation cơ bản (required) và emit sự kiện ra bên ngoài
 * để component cha thực hiện validation phức tạp hơn (ví dụ: check trùng lặp).
 * Chạy khi blur khỏi input.
 * createdby: TMHieu - 09.12.2025
 */
async function validate() {
    // 1. Kiểm tra Required
    if (props.required && (!model.value || model.value.toString().trim() === '')) {
        errorMessage.value = `${props.label} không được để trống`
    } else {
        errorMessage.value = ''
    }

    // 2. Emit ra ngoài để cha có thể validate tổng thể / check trùng lặp
    emit('blur-check', {
        label: props.label,
        field: props.field,
        value: model.value,
        error: errorMessage.value,
    })
}

//#endregion

//#region Expose (API công khai của component)

/**
 * Expose các hàm để component cha có thể gọi trực tiếp.
 * @method setError(msg) - Đặt thông báo lỗi.
 * @method clearError() - Xóa thông báo lỗi.
 */
defineExpose({
    /**
     * @param {string} msg - Thông báo lỗi cần hiển thị.
     */
    setError(msg) {
        errorMessage.value = msg
    },
    /**
     * Xóa thông báo lỗi hiện tại.
     */
    clearError() {
        errorMessage.value = ''
    },
})

//#endregion
</script>

<style scoped>
/* Scoped styles */

.wrapper-input {
    margin-bottom: 16px;
}

.ms-input-wrapper {
    display: flex;
    margin-bottom: 6px;
    position: relative;
    align-items: center;
}

/* Label Styling */
label {
    width: 174px;
    font-weight: 500;
    color: #1f2229b6;
}

.required::after {
    content: ' *';
    color: red;
    margin-left: 2px;
}

/* Input Text Styling */
input {
    height: 32px;
    flex: 1 1 550px;
    min-width: 0px;
    padding: 6px 32px 6px 16px !important;
    border: 1px solid #ddd;
    border-radius: 4px;
    transition: all 0.2s;
}

input::placeholder {
    color: #99a1b2;
}

input:focus {
    outline: none;
    border-color: #2970f6;
}

/* Error State */
.input-error {
    border-color: #ff4d4f !important;
}

.error-text {
    color: #ff4d4f;
    font-size: 12px;
    margin-top: 4px;
    justify-content: flex-end;
}

/* Ant Select Styles (Deep Selectors) */
:deep(.ant-select) {
    height: 32px;
    flex: 1 1 550px;
    min-width: 0px;
    transition: all 0.2s;
    border-radius: 4px;
}

:deep(.ant-select-selector) {
    border-radius: 4px !important;
}

/* Ant Space/DatePicker Styles (Deep Selectors) */
:deep(.ant-space) {
    height: 32px;
    flex: 1 1 550px;
    min-width: 0px;
    transition: all 0.2s;
}

:deep(.ant-space-item) {
    width: 100%;
}

:deep(.ant-picker) {
    border-radius: 4px !important;
}

/* Ant Picker Error State (for date picker) */
:deep(.ant-picker.input-error) {
    border-color: #ff4d4f !important;
}
</style>
