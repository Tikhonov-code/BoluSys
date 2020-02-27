$(function () {
    var ds = new DevExpress.data.CustomStore({
        loadMode: "raw",
        load: function () {
            return $.getJSON('Dashboard.aspx?SP=GetAlerts');
        }
    });

    $("#AlertsLine").dxDataGrid({
        dataSource: ds,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10],
            showInfo: true
        },
        //columns: ["line"]
    });

});
function Myalert() {
    alert("Hello"+this.value);
}
//Context Menu Section -----------------------------------------
$(function () {
    $("#context-menu").dxContextMenu({
        dataSource: contextMenuItems,
        width: 100,
        target: "#image",
        onItemClick: function (e) {
            if (!e.itemData.items) {
                //var x = $(this).attr("id");
                DevExpress.ui.notify("The \"" + e.itemData.text + "\" item was clicked", "success", 1500);
            }
        }
    });
});
var contextMenuItems = [
    { text: 'Info'},
    { text: 'Chart' }
];
