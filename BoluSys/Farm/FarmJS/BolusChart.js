
//-----------------------------------------------------------
var AnimalIDlist = [];
var BolusIDlist = [];


//------Page Load Section--------------------------------------------------------------------------

$(document).ready(function () {
    var d = $("#DateSearch").val();
    //Request data for initial chart
    var bidc = $("#Bolus_id").val();
    if (bidc != 0 && d != null) {
        // Gate 1 --------datesearch= today-------------------
        dateFrom_Show(d + ' 12:00 AM');
        dateTo_Show(d + '  11:59 PM');
        //ChartCreate("#chart", d + ' 12:00 AM', d + ' 11:59 PM', bidc, "full");
        ChartCreate("#chart_temp", d + ' 12:00 AM', d + ' 11:59 PM', bidc, "temp");

        //GetIntakesData(d + ' 12:00 AM', d + ' 11:59 PM', bidc);
        IntakesChart_Show0(d + ' 12:00 AM', d + ' 11:59 PM', bidc);
        GetTotalIntakes(d + ' 12:00 AM', d + ' 11:59 PM', bidc);
        GetAverTemperature(d + ' 12:00 AM', d + ' 11:59 PM', bidc);

        GetCowInfo(bidc);
        GetCowsLogs($("#Animal_id").val());

        if (BolusIDlist.length == 0) {
            var url = 'BolusChart.aspx/GetBolusIDList?SP=GetBolusIDList';
            var Param = {};
            Param.SP = "GetBolusIDList";
            myAjaxRequestJson(url, Param, Success_BolusIDList);
        }
        else {
            //$("#BolusIDList").dxSelectBox("instance").option("value", bidc);
            BolusIDList_Show(ds, bidc);
        }

        return;
    }
});

function Success_BolusIDList(result) {
    //var AnimalIDlist = [];
    for (var item in result.d) {
        AnimalIDlist.push(result.d[item].animal_id);
        BolusIDlist.push(result.d[item]);
    }
    var bid = $("#Bolus_id").val();
    if (bid == undefined) {
        bid = BolusIDlist[0].bolus_id;
    }
    BolusIDList_Show(BolusIDlist, Number(bid));
    return;
}


function dateFrom_Show(dtpar) {

    $("#dateFrom").dxDateBox({
        type: "datetime",
        value: dtpar,
        width: 200,
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = new Date(e.value);
            var dateBox = $("#dateTo").dxDateBox("instance");
            var dt = new Date(dateBox.option('value'));

            //ChartCreate("#chart", ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val(), "full");
            ChartCreate("#chart_temp", ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val(), "temp");
            IntakesChart_Show0(ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val());
            GetTotalIntakes(ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val());
            GetAverTemperature(ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val());
        }
    });
};
function dateTo_Show(dtpar) {

    $("#dateTo").dxDateBox({
        type: "datetime",
        value: dtpar,
        width: 200,
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = new Date(e.value);
            //var dateBox = $("#dateFrom").dxDateBox("instance");
            var df = new Date($("#dateFrom").dxDateBox("instance").option('value'));
            //ChartCreate("#chart", ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val(), "full");
            ChartCreate("#chart_temp", ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val(), "temp");

            IntakesChart_Show0(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
            GetTotalIntakes(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
            GetAverTemperature(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
        }
    });
};

function ChartCreate(chartSelector,df, dt, bid,chart_type) {
    if (df == "" || dt == "") {
        return;
    }
    //------------------------------------------------------------------------------------
    var ds = "BolusChart.aspx?DateFrom=" + df + "&DateTO=" + dt + "&Bolus_id=" + bid + "&SP=GetDataForChart&chart_type=" + chart_type;
    $(chartSelector).dxChart({
   //$("#chart").dxChart({
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
            valueMarginsEnabled: true,
            visualRange: {
                startValue: 35,
                endValue: 43
            },
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
                },
                {
                    value: 37,
                    color: "#40ff00",
                    dashStyle: "LongDash",
                    width: 2,
                    label: { visible: true,
                        text: "Average Temperature",
                        color: "red"
                    }
                }
            ]
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
        },
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

function GetCowInfo(bidpar) {
    //-------------------------------------------
    var url = 'BolusChart.aspx/GetCowInfoSt?SP=GetCowInfoSt';
    var Param = {};
    Param.SP = "GetCowInfoSt";
    Param.bolus_id = bidpar;
    myAjaxRequestJson(url, Param, Success_GetCowInfo);
    //----------------------------------------------------
}
function Success_GetCowInfo(result) {
    var xx = result.d;
    $("#CowInfo").html(xx);;
}

//===============================================================
// BolusIDList Section---------Begin-------------------------------
function BolusIDList_Show(ds, vl) {
    $("#BolusIDList").dxSelectBox({
        //items: AnimalIDlist,
        placeholder: "Choose Animal_id",
        showClearButton: true,
        dataSource: ds,
        displayExpr: "animal_id",
        valueExpr: "bolus_id",
        value: vl,//ds[0].bolus_id,   
        //------------------------------------------------------

        onValueChanged: function (e) {
            var bid = e.value;
            $("#Bolus_id").val(bid);

            var aid_c = ds.filter(a => a.bolus_id == bid);
            $("#Animal_id").val(aid_c[0].animal_id);
            var dto = new Date($("#dateTo").dxDateBox("instance").option('value'));
            var dfr = new Date($("#dateFrom").dxDateBox("instance").option('value'));

            //ChartCreate("#chart", ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid, "full");
            ChartCreate("#chart_temp", ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid, "temp");

            IntakesChart_Show0(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
            GetTotalIntakes(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
            GetAverTemperature(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid); 

            GetCowInfo(bid);
            GetCowsLogs(aid_c[0].animal_id);

        }
    });
}
// BolusIDList Section---------End-------------------------------

//===============================================================
// Logs list Section---------Begin-------------------------------
function ShowLogList(cl) {
    var listWidget = $("#CowsLogList").dxList({
        dataSource: cl,
        height: 200,
        allowItemDeleting: false,
        itemDeleteMode: "toggle",
    }).dxList("instance");
};

function GetCowsLogs(aid_par) {
    //-------------------------------------------
    var url = 'BolusChart.aspx?SP=GetCowsLogs&Animal_id=' + aid_par;
    var Param = {};
    Param.SP = "GetCowsLogs";
    Param.Animal_id = aid_par;
    myAjaxRequestJsonE(url, Param, Success_GetCowsLogs, Error_GetCowsLogs);
    //----------------------------------------------------
}
function Success_GetCowsLogs(result) {
    ShowLogList(result);
    return;
}
function Error_GetCowsLogs(xhr, status, error) {
    return false;
}
function myAjaxRequestJsonE(URL, Param, Success_function_name, Error_function_name) {
    //var obj = {};
    //obj.DateSearch = Param;
    $.ajax({
        type: "POST",
        url: URL,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: Success_function_name,
        error: Error_function_name
    });
    return false;
}
//// Logs list Section----------End---------------------------

//Intakes chart Section---------------Begin--------------------
function IntakesChart_Show0(df, dt, bid) {
    //BolusChart_new.aspx/GetIntakesData?DateFrom=2019/11/25&DateTo=2019/11/26&Bolus_id=107&SP=GetIntakesData"
    var WaterVol = GetTotalIntakes(df, dt, bid);
    var ds = "BolusChart.aspx/GetIntakesData?DateFrom=" + df + "&DateTo=" + dt + "&Bolus_id=" + bid + "&SP=GetIntakesData";
    $("#IntakesChart").dxChart({
        dataSource: ds,
        legend: {
            visible: false
        },
        series: {
            type: "bar"
        },
        commonSeriesSettings: {
            barPadding: 0.1,
            argumentField: "arg",
            type: "bar"
        },
        argumentAxis: {
            tickInterval: 10,
            label: {
                format: {
                    type: "decimal"
                }
            }
        },
        title: "Intakes " + WaterVol+ ", Litres",
        tooltip: {
            enabled: true,
            customizeTooltip: function (arg) {
                return {
                    text: arg.valueText + ", Litres" + "<br />" + arg.argumentText
                };
            }
        }
    });
}
function GetTotalIntakes(dt1, dt2, bid) {

    //-------------------------------------------
    var url = "BolusChart.aspx/GetTotalIntakes";
    var Param = {};
    Param.SP = "GetTotalIntakes";
    Param.DateFrom = dt1;
    Param.DateTo = dt2;
    Param.bid = bid;
    myAjaxRequestJsonE(url, Param, Success_GetTotalIntakes, Error_GetTotalIntakes);
}
function Success_GetTotalIntakes(result) {

    $("#TotalIntakes").val(result.d);
    $("#IntakesChart").dxChart("instance").option("title", "Intakes total=" + result.d + ", Litres");

    return result.d;
}
function Error_GetTotalIntakes(xhr, status, error) {
    return false;
}
//Intakes chart Section---------------End--------------------

//Average Temperature
function GetAverTemperature(dt1, dt2, bid) {

    //-------------------------------------------
    var url = "BolusChart.aspx/GetAverageTemperature";
    var Param = {};
    Param.SP = "GetAverageTemperature";
    Param.DateFrom = dt1;
    Param.DateTo = dt2;
    Param.bid = bid;
    myAjaxRequestJsonE(url, Param, Success_GetAverTemperature, Error_GetAverTemperature);
}
function Success_GetAverTemperature(result) {

    //$("#chart").dxChart("instance").option("valueAxis").constantLines[2].value = result.d;'value: ' + Number(result.d)
    //$("#chart").dxChart("instance").option("valueAxis.constantLines[2].value", Number(result.d));
    $("#chart_temp").dxChart("instance").option("valueAxis.constantLines[2].value", Number(result.d));
    return result.d;
}
function Error_GetAverTemperature(xhr, status, error) {
    return false;
}