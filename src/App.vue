<template>
    <ConfigProvider :locale="viVN">
        <div class="container d-flex flex-column">
            <header-component></header-component>
            <tool-bar-component></tool-bar-component>
            <div class="content d-flex flex1 flex-row">
                <ms-table :columns="cols" :rows="data"></ms-table>
            </div>
        </div>
    </ConfigProvider>
</template>

<script setup>
import HeaderComponent from '@/layouts/Header.vue';
import ToolBarComponent from '@/layouts/ToolBar.vue';
import viVN from 'ant-design-vue/es/locale/vi_VN';
import { ConfigProvider } from 'ant-design-vue';
import MsTable from './components/ms-table/MsTable.vue';
// Columns
const cols = [
    { field: "customerType", header: "Loại khách hàng", width: 170 },
    { field: "customerCode", header: "Mã khách hàng", width: 260 },
    { field: "customerName", header: "Tên khách hàng", width: 300 },
    { field: "taxCode", header: "Mã số thuế", width: 160 },
    { field: "address", header: "Địa chỉ (Giao hàng)", width: 210 },
    { field: "phone", header: "Điện thoại", width: 170 },
    { field: "lastPurchaseDate", header: "Ngày mua hàng gần nhất", width: 220 },
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

<style>
.container {
    position: absolute;
    width: 100%;
    height: 100vh;
}
</style>
