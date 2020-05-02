var employee = {
    "ID": 1,
    "FirstName": "John",
    "LastName": "Heart"
};


var dt = new Date();
var dt10 = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0);
var dt20 = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 24, 0, 0, 0);


$(function () {

    var farmName = $.getJSON("CowPage.aspx?SP=GetFarmName"
        , function (result) {
            ShowForm(result);
            //ShowForm(animalList);
        },
        function (result, status, xhr) {
            var d = result;
        }
    );


});

function ShowForm(animalList) {

    //------------------------------------------
    $("#form").dxForm({
        formData: employee,
        items: [{
            itemType: "group",
            cssClass: "first-group",
            colCount: 3,
            items: [{
                template: "<div class='form-avatar'></div>"
            }, {
                itemType: "group",
                colCount: 2,
                items: [
                    {
                        dataField: "Animal_id",
                        editorType: "dxSelectBox",
                        editorOptions: {
                            items: animalList.al,
                            value: animalList.al[0],
                            displayExpr: "animal_id",
                            valueExpr: "bolus_id",
                            elementAttr: {
                                id: 'Animal_id'
                            }
                        }
                    },
                    {
                        dataField: "Switch to",
                        editorType: "dxDateBox",
                        editorOptions: {
                            type: "date",
                            value: dt,
                            elementAttr: {
                                id: 'Switch_to'
                            },
                            applyValueMode: "instantly",
                            onValueChanged: function (e) {
                                var dt = new Date(e.value);
                                var dt1 = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0);
                                var dt2 = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 24, 0, 0, 0);
                                $("#From").dxDateBox("instance").option('value', dt1);
                                $("#To").dxDateBox("instance").option('value', dt2);
                            },
                            buttons: [{
                                name: "today",
                                location: "before",

                                options: {
                                    text: "Today",
                                    onClick: function () {
                                        dateEditor.option("value", new Date().getTime());
                                    }
                                }
                            }, {
                                name: "prevDate",
                                location: "before",
                                options: {
                                    icon: "spinprev",
                                    stylingMode: "text",
                                    onClick: function () {
                                        var currentDate = dateEditor.option("value");
                                        dateEditor.option("value", currentDate - millisecondsInDay);
                                    }
                                }
                            }, {
                                name: "nextDate",
                                location: "after",
                                options: {
                                    icon: "spinnext",
                                    stylingMode: "text",
                                    onClick: function () {
                                        var currentDate = dateEditor.option("value");
                                        dateEditor.option("value", currentDate + millisecondsInDay);
                                    }
                                }
                            }, "dropDown"]
                        }
                    },
                    {
                        dataField: "From",

                        editorType: "dxDateBox",
                        type: "datetime",
                        editorOptions: {
                            type: "datetime",
                            value: dt10,
                            elementAttr: {
                                id: 'From'
                            }
                        }
                    },
                    {
                        dataField: "To",
                        editorType: "dxDateBox",
                        type: "datetime",
                        editorOptions: {
                            type: "datetime",
                            value: dt20,
                            elementAttr: {
                                id: 'To'
                            }
                        },
                    },
                    {
                        editorType: "dxButton",
                        editorOptions: {
                            stylingMode: "contained",
                            text: "Report",
                            type: "success",
                            width: 120,
                            onClick: function () {
                                var dt1 = ConvertDateToMyF($("#From").dxDateBox("instance").option('value'));
                                var dt2 = ConvertDateToMyF($("#To").dxDateBox("instance").option('value'));
                                var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                                ChartCreate('#chart_temp', dt1, dt2, bid);
                                IntakesChart_Show(dt1, dt2, bid);
                                GetCowInfo(bid);
                            }
                        }
                    }
                ]
            }]
        }            
        ]
    });

    //------------------------------------------
    var millisecondsInDay = 24 * 60 * 60 * 1000;
    var dateEditor = $("#Switch_to").dxDateBox({
        value: new Date().getTime(),
        //stylingMode: "outlined",
        buttons: [{
            name: "today",
            location: "before",

            options: {
                text: "Today",
                onClick: function () {
                    dateEditor.option("value", new Date().getTime());
                }
            }
        }, {
            name: "prevDate",
            location: "before",
            options: {
                icon: "spinprev",
                stylingMode: "text",
                onClick: function () {
                    var currentDate = dateEditor.option("value");
                    dateEditor.option("value", currentDate - millisecondsInDay);
                }
            }
        }, {
            name: "nextDate",
            location: "after",
            options: {
                icon: "spinnext",
                stylingMode: "text",
                onClick: function () {
                    var currentDate = dateEditor.option("value");
                    dateEditor.option("value", currentDate + millisecondsInDay);
                }
            }
        }, "dropDown"]
    }).dxDateBox("instance");
}

function ChartCreate(chartSelector, df, dt, bid) {

    if (df == "" || dt == "") {
        return;
    }
    //------------------------------------------------------------------------------------
    var ds = "CowPage.aspx?DateFrom=" + df + "&DateTO=" + dt + "&Bolus_id=" + bid + "&SP=GetDataForChart";
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
            if (this.value >= 41.0) {
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
                    label: {
                        visible: true,
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
    //    DataGapsShow();
}

//Intakes chart Section---------------Begin--------------------
function IntakesChart_Show(dt1, dt2, bid) {
    //BolusChart_new.aspx/GetIntakesData?DateFrom=2019/11/25&DateTo=2019/11/26&Bolus_id=107&SP=GetIntakesData"
    var WaterVol = GetTotalIntakes(dt1, dt2, bid);

    var ds = "CowPage.aspx?DateFrom=" + dt1 + "&DateTo=" + dt2 + "&Bolus_id=" + bid + "&SP=GetIntakesData";

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
        title: "Intakes " + WaterVol + ", Litres",
        tooltip: {
            enabled: true,
            customizeTooltip: function (arg) {
                return {
                    text: arg.valueText + ", Litres" + "<br />" + DateTimeFormat(arg.argumentText)
                };
            }
        }
    });
}
function GetTotalIntakes(dt1, dt2, bid) {

    //-------------------------------------------
    var url = "CowPage.aspx/GetTotalIntakes";
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

function DateTimeFormat(dtpar) {
    var dt = new Date(dtpar);
    var result = dt.getFullYear() + '-' + Number(dt.getMonth() + 1) + '-' + dt.getDate() + ' ' + dt.getHours() + ':' + dt.getMinutes();
    return result;
}

//Cow data Info--------------------------------------------------------------
function GetCowInfo(bidpar) {
    //-------------------------------------------
    var url = 'CowPage.aspx/GetCowInfoSt?SP=GetCowInfoSt';
    var Param = {};
    Param.SP = "GetCowInfoSt";
    Param.bolus_id = bidpar;
    myAjaxRequestJson(url, Param, Success_GetCowInfo);
    //----------------------------------------------------
}
function Success_GetCowInfo(result) {

    $("#CowInfoDiv").attr("style","visibility: visible;");
    var xx = result.d;
    $("#CowInfo").html(xx);;
}

//===============================================================
