$(function () {
    var now = new Date();

    $("#DateFrom").dxDateBox({
        type: "datetime",
        name: "Date From:",
        value: now
    });
});
$(function () {
    var now = new Date();

    $("#DateTo").dxDateBox({
        type: "datetime",
        value: now
    });
});
function FillData() {
    $("#GridGaps").dxDataGrid({
        dataSource: data_db,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },   
        columns: [
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Bolus_id",
                dataField: "bolus_id"
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Animal_id",
                dataField: "animal_id" 
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Date From",
                dataField: "dt_from"
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Date To",
                dataField: "dt_to"
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Interval, min",
                dataField: "interval"
            }]
    });

};
var data_db;

//------------Button Section------------------------------------
$(function () {
    
    $("#SearchGaps").dxButton({
        //stylingMode: "outlined",
        text: "Find Gaps In Data",
        type: "success",
        elementAttr: {
            title: "Click To Find"
        },
        width: 150,
        onClick: function () {
            data_db = new DevExpress.data.CustomStore({
                loadMode: "raw",
                cacheRawData: true,
                key: "bolus_id",
                load: function (loadOptions) {
                    var dt0 = ConvertDateToMyF($("#DateFrom").dxDateBox("instance").option("value"));
                    var dt1 = ConvertDateToMyF($("#DateTo").dxDateBox("instance").option("value"));
                    return $.getJSON('DataGaps.aspx?SP=GetGapsData&dt0=' + dt0+'&dt1='+dt1 );
                }
            });

            FillData();
        }
    });
});