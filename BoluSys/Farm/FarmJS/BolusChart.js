﻿
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
        GetGapsDataValue(d + ' 12:00 AM', d + ' 11:59 PM', bidc);

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
            GetGapsDataValue(ConvertDateToMyF(newValue), ConvertDateToMyF(dt), $("#Bolus_id").val());
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
            GetGapsDataValue(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
        }
    });
};

function ChartCreate(chartSelector, df, dt, bid, chart_type) {
    
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
        customizePoint: function () {
            if (this.value >= 41.0 ) {
                return { image: { url: "imgs/cyclered.jpg", width: 20, height: 20 }, visible: true };
            }
            if (this.value >= 40.5 && this.value < 41.0) {
                return { image: { url: "imgs/cycleyellow.jpg", width: 20, height: 20 }, visible: true };
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

    // Show Data Gaps Table-------------------------------------------
    DataGapsShow();
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
            GetGapsDataValue(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
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
            //argumentType: 'datetime',
            tickInterval: 10
        },
        title: "Intakes " + WaterVol+ ", Litres",
        tooltip: {
            enabled: true,
            customizeTooltip: function (arg) {
                return {
                    text: arg.valueText + ", Litres"// + "<br />" + DateTimeFormat(arg.argumentText)
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

//Data Gaps Grid---------------------------------------------
function DataGapsShow() {

    var data_db = new DevExpress.data.CustomStore({
                loadMode: "raw",
                cacheRawData: true,
                key: "bolus_id",
                load: function (loadOptions) {
                    //var dt0 = ConvertDateToMyF($("#dateFrom").dxDateBox("instance").option("value"));
                    //var dt1 = ConvertDateToMyF($("#dateTo").dxDateBox("instance").option("value"));

                    var dt0 = $("#dateFrom").dxDateBox("instance").option("value");
                    var dt1 = $("#dateTo").dxDateBox("instance").option("value");
                    var bid = $("#Bolus_id").val();

                    return $.getJSON('BolusChart.aspx?SP=GetGapsData&DateFrom=' + dt0 + '&DateTo=' + dt1+'&bolus_id='+bid);
                }
            });

    FillDataGaps(data_db);
};

function FillDataGaps(data_db) {
    $("#DataGapsIndividual").dxDataGrid({
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
                dataField: "dt_from",
                dataType: "datetime"
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Date To",
                dataField: "dt_to",
                dataType: "datetime"
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Interval, min",
                dataField: "interval"
            }]
    });

};

function GetGapsDataValue(dt0, dt1, bid) {
    if (dt0 == undefined || dt1 == undefined || bid == undefined) {
        $("#DataGapsValue").html("No Data");
    }
    else {
        var url = 'BolusChart.aspx?SP=GetGapsDataValue&DateFrom=' + dt0 + '&DateTo=' + dt1 + '&bolus_id=' + bid;
        $.getJSON(url, function (result) {
            var ind1 = result.lastIndexOf(":")+1;
            var ind2 = result.length;
            var gap = result.slice(ind1 - ind2);
            var mark = (Number(gap) < 5) ? "<i class='far fa-thumbs-up' style='font-size:36px; color:green;' ></i>" : "<i class='fa fa-warning' style='font-size: 48px; color: red'></i>";
            $("#DataGapsValue").html(result + "% "+mark);
        });        
    }

}

function DateTimeFormat(dtpar) {
    var dt = new Date(dtpar);
    var result = dt.getFullYear() + '-' + dt.getMonth() + '-' + dt.getDate() + ' ' + dt.getHours() + ':' + dt.getMinutes();
    return result;
}

//------Options Section--------------------------------------------
//$(function () {
//    var now = new Date();

//    $("#DaySet").dxDateBox({
//        type: "date",
//        value: now,
//        onValueChanged: function (data) {
//            //DevExpress.ui.notify(data.value, "warning", 500);
//            //$("#dateFrom").dxDateBox("instance").option('value', data.value);
//            //dateFrom_Show(data.value);
//            //dateTo_Show(data.value);

//            //ChartCreate("#chart_temp", ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val(), "temp");
//            //IntakesChart_Show0(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
//            //GetTotalIntakes(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
//            //GetAverTemperature(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
//            //GetGapsDataValue(ConvertDateToMyF(df), ConvertDateToMyF(newValue), $("#Bolus_id").val());
//        }
//    });
//    $("#dateFrom1").dxDateBox({
//        type: "date",
//        value: now,
//        onValueChanged: function (data) {
//        }
//    });
//    $("#dateTo1").dxDateBox({
//        type: "date",
//        value: now,
//        onValueChanged: function (data) {
//        }
//    });
//    var ds = [
//        {
//            bolus_id: "1",
//            animal_id:"7070"
//        },
//        {
//            bolus_id: "2",
//            animal_id: "7071"
//        }

//    ];
//    $("#animal_id").dxSelectBox({
//        //items: AnimalIDlist,
//        placeholder: "Choose Animal_id",
//        showClearButton: true,
//        dataSource: ds,
//        displayExpr: "animal_id",
//        valueExpr: "bolus_id",
//        width:"200px",
//        //value: vl,//ds[0].bolus_id,   
//        //------------------------------------------------------

//        onValueChanged: function (e) {
//            var bid = e.value;
//            $("#Bolus_id").val(bid);

//            var aid_c = ds.filter(a => a.bolus_id == bid);
//            $("#Animal_id").val(aid_c[0].animal_id);
//            var dto = new Date($("#dateTo").dxDateBox("instance").option('value'));
//            var dfr = new Date($("#dateFrom").dxDateBox("instance").option('value'));

//            ////ChartCreate("#chart", ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid, "full");
//            //ChartCreate("#chart_temp", ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid, "temp");

//            //IntakesChart_Show0(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
//            //GetTotalIntakes(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
//            //GetAverTemperature(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
//            //GetGapsDataValue(ConvertDateToMyF(dfr), ConvertDateToMyF(dto), bid);
//            //GetCowInfo(bid);
//            //GetCowsLogs(aid_c[0].animal_id);

//        }
//    });
//});
//------Options Section--------------------------------------------

//Cow Personal Section---------------------------------------------
//$(function () {
//    var form = $("#form_CPS").dxForm({
//        formData: cow_data[0],
//        readOnly: false,
//        showColonAfterLabel: true,
//        labelLocation: "top",
//        minColWidth: 300,
//        colCount: 4
//    }).dxForm("instance");
//    $("#read-only").dxCheckBox({
//        text: "readOnly",
//        value: false,
//        onValueChanged: function (data) {
//            form.option("readOnly", data.value);
//        }
//    });
//    $("#normal-contained").dxButton({
//        stylingMode: "contained",
//        text: "Contained",
//        type: "normal",
//        width: 120,
//        onClick: function () {
//            DevExpress.ui.notify("The Contained button was clicked");
//        }
//    });
//});
//var cow_data = [{
//    "Animal_id": "96",
//    "Lactation": 2,
//    "Current Stage of Lactation": "N/A",
//    "Health Concerns Illness History": "No",
//    "Overall Health": "good",
//    "Date of Birth": "3/5/2018",
//    "Calving Due Date": "3/5/2019",
//    "Actual Calving Date": "3/5/2020"
//}];