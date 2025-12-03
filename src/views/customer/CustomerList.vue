<template>
    <!-- Tool bar -->
    <div class="toolbar d-flex justify-content-between flex-col">
        <div class="left-bar d-flex align-content-center">
            <ms-button class="show-menu" iconLeft="folder" iconRight="down" type="outline">Tất cả khách hàng</ms-button>
            <ms-text-color type="primary">Sửa</ms-text-color>
            <div class="btn-grey">
                <div class="icon-reload"></div>
            </div>
        </div>
        <div class="right-bar d-flex align-content-center gap8">
            <div class="search-ai d-flex align-content-center">
                <div class="icon-search-ai"></div>
                <input class="input-search-ai" type="text" placeholder="Tìm kiếm thông minh">
                <div class="icon-ai"></div>
            </div>
            <div class="btn-headquarter-ai">
                <div class="icon-headquarter-ai"></div>
            </div>
            <ms-button iconLeft="plus" type="primary" @click="goToAdd">Thêm</ms-button>
            <ms-button iconLeft="import" type="outline-primary">Nhập từ Excel</ms-button>
            <ms-button iconLeft="show-more" type="outline"></ms-button>
            <ms-button iconLeft="list" iconRight="down" type="outline"></ms-button>

        </div>
    </div>
    <div class="content d-flex flex1 flex-row">
        <ms-table :columns="cols" :rows="data"></ms-table>
    </div>

</template>

<script setup>
import MsTextColor from '@/components/ms-button/MsTextColor.vue';
import MsButton from '@/components/ms-button/MsButton.vue';
import MsTable from '@/components/ms-table/MsTable.vue';
import { useRouter } from 'vue-router'

const router = useRouter()

function goToAdd() {
    router.push('/customer/add')
}

// Columns
const cols = [
    { field: "customerType", header: "Loại khách hàng", width: 175 },
    { field: "customerCode", header: "Mã khách hàng", width: 280 },
    { field: "customerName", header: "Tên khách hàng", width: 300 },
    { field: "taxCode", header: "Mã số thuế", width: 160 },
    { field: "address", header: "Địa chỉ (Giao hàng)", width: 210 },
    { field: "phone", header: "Điện thoại", width: 160 },
    { field: "lastPurchaseDate", header: "Ngày mua hàng gần nhất", width: 240 },
    { field: "purchasedGoods", header: "Hàng hóa đã mua", width: 200 },
    { field: "purchasedGoodsName", header: "Tên hàng hóa đã mua", width: 300 }
];

// Sample Data
const data = [
    {
        customerType: "NBH01",
        customerCode: "KH001-testvnquan3",
        customerName: "Công ty TNHH Hoa Mai",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH00000100100216265210",
        customerName: "Vương Long",
        taxCode: "",
        address: "",
        phone: "055555553...",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH00000100100216265209",
        customerName: "Silkweb",
        taxCode: "",
        address: "N/A",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH00000100100216265208",
        customerName: "CÔNG TY CỔ PHẦN SẢN XUẤT KINH DOANH",
        taxCode: "0100101234",
        address: "Số 5 ngõ 31 phố Hoàng ...",
        phone: "0904165344",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "NBH01",
        customerCode: "KH001-testnhanh-111",
        customerName: "Công ty TNHH Hoa Mai",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH00000100100216265211",
        customerName: "Ngọc Anh",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "22/10/2025",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH00000100100216265207",
        customerName: "Trung tâm Internet điện lực",
        taxCode: "0100101033-010",
        address: "132 Phố Đức Chính P.Trú...lkjglkdf gdfhldkf gdjf;gijsp'dfj kjsdfoigjspod sijdp9oif gsdikfogidsjp fksdjfoigj s;dfngoisdpfojsd fikgjsdoip; iujsdoifjgo isdhjofi gsldkjfngo;sdifj  ",
        phone: "",
        lastPurchaseDate: "22/10/2025",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH00000100100216265206",
        customerName: "Lưu Thị Lan Hương",
        taxCode: "",
        address: "Việt Nam, Thành phố Hà ...",
        phone: "",
        lastPurchaseDate: "21/10/2025",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "LKHA",
        customerCode: "Vnquan2110_1",
        customerName: "Công ty TNHH Hoa Mai",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    // --- 10 dòng dữ liệu giả mới ---
    {
        customerType: "NBH02",
        customerCode: "KH001-test001",
        customerName: "Công ty ABC",
        taxCode: "0100203030",
        address: "Hà Nội",
        phone: "0912345678",
        lastPurchaseDate: "01/11/2025",
        purchasedGoods: "Máy tính",
        purchasedGoodsName: "Laptop Dell"
    },
    {
        customerType: "",
        customerCode: "KH001-test002",
        customerName: "Nguyễn Văn B",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "NBH03",
        customerCode: "KH001-test003",
        customerName: "Công ty XYZ",
        taxCode: "0203030404",
        address: "HCM",
        phone: "0987654321",
        lastPurchaseDate: "15/10/2025",
        purchasedGoods: "Điện thoại",
        purchasedGoodsName: "iPhone 15"
    },
    {
        customerType: "",
        customerCode: "KH001-test004",
        customerName: "Trần Thị C",
        taxCode: "",
        address: "Đà Nẵng",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH001-test005",
        customerName: "Công ty DEF",
        taxCode: "0304040505",
        address: "Hà Nội",
        phone: "0933221100",
        lastPurchaseDate: "20/09/2025",
        purchasedGoods: "Máy in",
        purchasedGoodsName: "HP LaserJet"
    },
    {
        customerType: "NBH04",
        customerCode: "KH001-test006",
        customerName: "Phạm Văn D",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "05/10/2025",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "",
        customerCode: "KH001-test007",
        customerName: "Công ty GHI",
        taxCode: "",
        address: "TP HCM",
        phone: "0909988776",
        lastPurchaseDate: "",
        purchasedGoods: "Bàn ghế",
        purchasedGoodsName: "Ghế văn phòng"
    },
    {
        customerType: "",
        customerCode: "KH001-test008",
        customerName: "Lê Thị E",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    },
    {
        customerType: "NBH05",
        customerCode: "KH001-test009",
        customerName: "Công ty JKL",
        taxCode: "0405050606",
        address: "Hải Phòng",
        phone: "0911223344",
        lastPurchaseDate: "10/11/2025",
        purchasedGoods: "Máy lạnh",
        purchasedGoodsName: "Daikin"
    },
    {
        customerType: "",
        customerCode: "KH001-test010",
        customerName: "Nguyễn Văn F",
        taxCode: "",
        address: "",
        phone: "",
        lastPurchaseDate: "",
        purchasedGoods: "",
        purchasedGoodsName: ""
    }
];

</script>

<style scoped>
.toolbar {
    padding: 12px 16px;
    background-color: #e2e4e9;
}


/* Left toolbar */
.show-menu {
    border-radius: 4px;
    border: 1px solid #d3d7de;
    padding: 7px 12px !important;
    background-color: #ffffff;
    cursor: pointer;
    margin-right: 16px;
    font-size: 13px;
    font-weight: 600;
}


/* Right toolbar */
.right-bar {
    gap: 8px;
}

.search-ai {
    position: relative;
}

.input-search-ai {
    min-width: 282px;
    border-radius: 4px;
    background-color: #f2f0fd;
    border: 1px solid var(--primary-color);
    outline: none;
    font-size: 13px;
    padding: 7px 16px 7px 32px;
}

.input-search-ai:focus-visible {
    border: 1px solid var(--primary-color);
    background-color: #fff;
}

.input-search-ai::placeholder {
    color: #8886f3;
    opacity: 1;
}

.btn-headquarter-ai {
    border-radius: 4px;
    padding: 6px 6px 7px 6px;
    border-image: linear-gradient(251deg, #9F73F1 71.93%, #4262F0 24.05%) 1;
    background:
        linear-gradient(90deg, #E7EBFD 0%, #ECE7FD 32.21%, #E5F7FF 66.11%, #FDEFE7 100%) padding-box,
        linear-gradient(90deg, #9F73F1 71.93%, #4262F0 24.05%) border-box;
    border: 1px solid transparent;
    border-right-width: 2px;
    cursor: pointer;
}
</style>