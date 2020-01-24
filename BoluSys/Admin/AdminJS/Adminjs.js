var menuData = [{
    id: "1",
    name: "Dashboard"
}, {
    id: "2",
    name: "Cows",
    items: [{
        id: "2_1",
        name: "Data",
        icon: "images/products/7.png",
        price: 1200
    }, {
        id: "2_2",
        name: "Logs",
        icon: "images/products/5.png",
        price: 1450
    },]
}, {
    id: "3",
    name: "Data Services",
    items: [{
        id: "3_1",
        name: "TTN Raw Converter",
        icon: "images/products/14.png",
        price: 550
    }, {
        id: "3_2",
        name: "Download to Excel",
        icon: "images/products/15.png",
        price: 750
    }]
    }];
$(function () {
    var dxMenu = $("#menu").dxMenu({
        dataSource: menuData,
        hideSubmenuOnMouseLeave: false,
        displayExpr: "name",
        onItemClick: function (data) {
            var item = data.itemData;
           // if (item.price)
            {
                window.open("../Farm/dashboard1.aspx", '_blank');
                $("#product-details > img").attr("src", item.icon);
                $("#product-details > .price").text("$" + item.price);
                $("#product-details > .name").text(item.name);
            }
        }
    }).dxMenu("instance");

    var showSubmenuModes = [{
        name: "onHover",
        delay: { show: 0, hide: 500 }
    }, {
        name: "onClick",
        delay: { show: 0, hide: 300 }
    }];

    $("#show-submenu-mode").dxSelectBox({
        items: showSubmenuModes,
        value: showSubmenuModes[1],
        displayExpr: "name",
        onValueChanged: function (data) {
            dxMenu.option("showFirstSubmenuMode", data.value);
        }
    });

    
});