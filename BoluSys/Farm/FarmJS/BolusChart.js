$(function () {
    var now = new Date();

    $("#dateFrom").dxDateBox({
        type: "datetime",
        value: now,
        width: 200,
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert(newValue);
            var dateBox = $("#dateTo").dxDateBox("instance");
            var dt = dateBox.option('value');
            ChartCreate(ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val());
        }
    });
});
$(function () {
    var now = new Date();

    $("#dateTo").dxDateBox({
        type: "datetime",
        value: now,
        width: 200,
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert(newValue);
            var dateBox = $("#dateFrom").dxDateBox("instance");
            var df = dateBox.option('value');
            ChartCreate(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
        }
    });
});
function ChartCreate(df, dt, bid) {
    if (df == "" || dt == "") {
        return;
    }
    //------------------------------------------------------------------------------------
    var ds = "BolusChart.aspx?DateFrom=" + df + "&DateTO=" + dt + "&Bolus_id=" + bid +"&SP=GetDataForChart";
    $("#chart").dxChart({
        dataSource: ds,
        series: {
            color: "#79cac4",
            type: "Spline",
            argumentField: "t",
            valueField: "Temperature"
        },
        legend: {
            visible: false
        },
        commonPaneSettings: {
            border: {
                visible: true,
                width: 2,
                top: false,
                right: false
            }
        },
        title: "Temperature " + "&#176C",
        tooltip: {
            enabled: true,
            customizeTooltip: function (arg) {
                return {
                    text: arg.valueText + "&#176C" + "<br />" + arg.argumentText
                };
            }
        },
        valueAxis: {
            valueType: "numeric",
            grid: {
                opacity: 0.2
            },
            constantLines: [
                {
                    value: 38.5,
                    color: "#0066ff",
                    dashStyle: "dash",
                    width: 2,
                    label: { visible: false }
                },
                {
                    value: 39.5,
                    color: "#ff3333",
                    dashStyle: "dash",
                    width: 2,
                    label: { visible: false }
                }]
        },
        argumentAxis: {
            type: "date",
            grid: {
                visible: true,
                opacity: 0.5
            }
        },
        "export": {
            enabled: true
        },
        legend: {
            visible: false
        }
        ,
        loadingIndicator: {
            backgroundColor: "#ffffff",
            enabled: true,
            font: {
                color: "#767676",
                family: "Segoe UI",
                opacity: 1,
                size: 24,
                weight: 400
            },
            show: false,
            text: "Loading..."
        }
    });
}
//-----------------------------------------------------------
var AnimalIDlist = [];
var BolusIDlist = [];
//$(function () {
    //$("#BolusIDList").dxSelectBox({
    //    items: AnimalIDlist,
    //    placeholder: "Choose Animal_id",
    //    showClearButton: true,
    //    onValueChanged: function (e) {

    //        $("#Animal_id").val(e.value);

    //        //----------------------------------------------------------
    //        var bl_c = BolusIDlist.filter(a => a.animal_id == e.value);

    //        $("#Bolus_id").val(bl_c[0].bolus_id);

    //        var dto = $("#dateTo").dxDateBox("instance").option('value');
    //        var dfr = $("#dateFrom").dxDateBox("instance").option('value');
    //        ChartCreate(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bl_c[0].bolus_id);            //------------------------------------
    //        //--------------------------------------

    //    }
    //});
//});

//------Page Load Section--------------------------------------------------------------------------

$(document).ready(function () {
    var d = $("#DateSearch").val();
    //Request data for initial chart
    ChartCreate(d + ' 12:00 AM', d + '  12:00 PM', $("#Bolus_id").val());

    //Set dates in dateboxes
    var d_from = new Date(d + ' 12:00 AM');
    var d_to = new Date(d + '  12:00 PM')
    $("#dateFrom").dxDateBox("instance").option("value", d_from);
    $("#dateTo").dxDateBox("instance").option("value", d_to);

    //-------------------------------------------
    var url = 'BolusChart.aspx/GetBolusIDList?SP=GetBolusIDList';
    var Param = {};
    Param.SP = "GetBolusIDList";
    myAjaxRequestJson(url, Param, Success_BolusIDList);
});

function Success_BolusIDList(result) {
    //var AnimalIDlist = [];
    for (var item in result.d) {
        AnimalIDlist.push(result.d[item].animal_id);
        BolusIDlist.push(result.d[item]);
    }
    var aid = BolusIDlist.findIndex(a => a.animal_id == $("#Animal_id").val());

    $("#BolusIDList").dxSelectBox({
        items: AnimalIDlist,
        placeholder: "Choose Animal_id",
        showClearButton: true,
        value: AnimalIDlist[aid],
        onValueChanged: function (e) {

            $("#Animal_id").val(e.value);

            //----------------------------------------------------------
            var bl_c = BolusIDlist.filter(a => a.animal_id == e.value);

            $("#Bolus_id").val(bl_c[0].bolus_id);

            var dto = $("#dateTo").dxDateBox("instance").option('value');
            var dfr = $("#dateFrom").dxDateBox("instance").option('value');
            ChartCreate(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bl_c[0].bolus_id);            //------------------------------------
            //--------------------------------------

        }
    });
    //var aid = $("#Animal_id").val();
   // $("#BolusIDList").dxSelectBox("instance").option("value", aid);
    return;
}