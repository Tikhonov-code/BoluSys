ShowReport(new Date());
$(function () {
    var now = new Date();

    $("#date").dxDateBox({
        type: "datetime",
        value: now,
        width: 200,
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            ShowReport(newValue);
        }
    });
});
function ShowReport(date_time) {

    var FarmReportStore = new DevExpress.data.CustomStore({
        loadMode: "raw",
        cacheRawData: true,
        //key: 'ANIMAL_ID',
        load: function (loadOptions) {
            var JSfile4Store = 'json_data.aspx/GetDataFarmStat?SP=001&PARAMS=' + convertDatePickerTimeToMySQLTime(date_time);

            return $.getJSON(JSfile4Store);
        }
    });

    $("#farmReport").dxDataGrid({
        dataSource: FarmReportStore,
        allowColumnReordering: false,
        allowColumnResizing: false,
        showBorders: true,
        rowAlternationEnabled: true,
        showColumnLines: true,
        showRowLines: true,
        hoverStateEnabled: true,
        columnAutoWidth: true,
        columns: [
            {
                dataField: "ANIMAL_ID",
                caption: "Animal ID",
                dataType: "number"
            },
            {
                dataField: "BOLUS_ID",
                caption: "Bolus ID",
                dataType: "number"
            },
            {
                dataField: "OUT_OF_RANGE",
                caption: "Out of range (> 2 hrs.)",
                dataType: "number"
            },
            {
                dataField: "NUMBER_OF_WATER_INTAKES",
                caption: "Number of water intakes",
                dataType: "number"
            },
            {
                dataField: "AVG_NUMBER_OF_WATER_INTAKES",
                caption: "Average Number of water intakes",
                dataType: "number"
            },
            {
                dataField: "TEMPERATURE",
                caption: "Temperature, °C",
                dataType: "number"
            },
            {
                dataField: "OVERLIMIT",
                visible: false
            },
        ],
        onRowPrepared: function (e) {
             console.log(e);
            if (e.rowType == "header") {
                e.rowElement.css('color', 'black');
                e.rowElement.css('font-weight', 'bold');
                e.rowElement.css('background', '#e6ff99');
            }
            if (e.rowType != "header" && e.data.OVERLIMIT > 0) {
                e.rowElement.css('background', '#facbb6');
            }
        }
    });
}
function convertDatePickerTimeToMySQLTime(str) {
    var month, day, year, hours, minutes, seconds;
    var date = new Date(str),
        month = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    hours = ("0" + date.getHours()).slice(-2);
    minutes = ("0" + date.getMinutes()).slice(-2);
    seconds = ("0" + date.getSeconds()).slice(-2);

    var mySQLDate = [date.getFullYear(), month, day].join("-");
    var mySQLTime = [hours, minutes, seconds].join(":");
    return [mySQLDate, mySQLTime].join(" ");
};