<template>
    <div :class="[
        'ms-button',
        `ms-button--${type}`
    ]" class="d-flex">
        <div v-if="iconLeft" :class="[`icon-${iconLeft}`]">
        </div>
        <div v-if="$slots.default" class="m-l-8">
            <slot></slot>
        </div>
        <div v-if="iconRight" :class="[`icon-${iconRight}`]" class="m-l-8">
        </div>
    </div>
</template>

<script setup>
//#region Props
const props = defineProps({
    iconLeft: {
        type: String,
        default: null,
    },
    iconRight: {
        type: String,
        default: null,
    },
    type: {
        type: String,
        default: null,
    }
});
//#endregion
</script>

<style scoped>
.ms-button {
    padding: 7px 16px 8px 8px;
    border-radius: 4px;
    cursor: pointer;
    font-size: 13px;
    transition: all 0.3s ease;
}

.ms-button--primary {
    background-color: var(--primary-color);
    color: #fff;
}

.ms-button--primary:hover {
    background-color: #2b4eee;
}

.ms-button--danger {
    background-color: #f9373a;
    color: #fff;
}

.ms-button--danger:hover {
    background-color: #fe5356;
}

.ms-button--outline {
    background-color: #ffffff;
    padding: 7px 9px;
    border: 1px solid #d3d7de;
}

.ms-button--outline:hover {
    background-color: #f0f2f4;

}

.ms-button--outline-primary {
    background-color: #fff;
    padding: 7px 16px 7px 8px;
    border: 1px solid var(--primary-color);
    color: var(--primary-color);
}

.ms-button--outline-primary:hover {
    background-color: #e7ebfd;
}
</style>
