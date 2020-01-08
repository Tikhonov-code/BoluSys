﻿//Palette for Ranges Section
$(function () {
    $("#gaugeAtRisk").dxCircularGauge({
        scale: {
            startValue: 0,
            endValue: $("#TotalCowsNumberInfo").val(),
            tick: {
                color: "black"
            },
            minorTick: {
                color: "black",
                visible: true
            },
            tickInterval: 1
        },
        rangeContainer: {
            backgroundColor: "red"
        },
        title: {
            text: "Cows At Risk",
            font: { size: 14, weight: "bold" }
        },
        "export": {
            enabled: false
        },
        value: $("#CowsAtRiskNumber").val(),
        subvalues: [$("#CowsAtRiskNumber").val()],
        subvalueIndicator: {
            type: "triangleMarker",
            color: "red"
        },
        valueIndicator: {
            color: "red"
        },
        subvalueIndicator: {
            type: "textCloud",
            color: "red"
        }

    });
});
$(function () {
    $("#gaugeToCheck").dxCircularGauge({
        scale: {
            startValue: 0,
            endValue: $("#TotalCowsNumberInfo").val(),
            tick: {
                color: "black"
            },
            minorTick: {
                color: "black",
                visible: true
            },
            tickInterval: 1
            //,minorTickInterval: 0
        },
        rangeContainer: {
            backgroundColor: "#ffcc00"
        },
        title: {
            text: "Cows To Check",
            font: { size: 14, weight: "bold" }
        },
        "export": {
            enabled: false
        },
        value: $("#CowsToCheckNumber").val(),
        subvalues: [$("#CowsToCheckNumber").val()],
        subvalueIndicator: {
            type: "triangleMarker",
            color: "red"
        },
        valueIndicator: {
            color: "#e6b800"
        },
        subvalueIndicator: {
            type: "textCloud",
            color: "#e6b800"
        }
    });
});

// Grid section
$(function () {
    $("#gridRisk").dxDataGrid({
        dataSource: riskdata,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10],
            showInfo: true
        },
        columns: [
            {
                caption: "Animal_id",
                dataField: "animal_id",
                alignment: 'center',
                width: 80,
                cellTemplate: function (cellElement, cellInfo) {
                    var d = new Date();
                    var dt = d.getFullYear() + '-' + Number(d.getMonth() + 1) + '-' + d.getDate();
                    $('<a/>')
                        .attr({
                            href: 'BolusChart?Animal_id=' + cellInfo.value + '&Bolus_id=' + cellInfo.key + '&DateSearch=' + dt + '&SP=ShowChart'
                        })
                        .text(cellInfo.value)
                        .appendTo(cellElement);
                }
            },
            {
                caption: "Event",
                dataField: "event",
                width: 60
            },
            {
                caption: "Message",
                dataField: "message",
                width: 550
            },
            {
                caption: "Date",
                dataField: "date_emailsent",
                dataType: "date",
                format: "yyyy-MM-dd hh:mm:ss",
                width: 150
            }
        ]
    });

});
var riskdata = new DevExpress.data.CustomStore({
    loadMode: "raw",
    cacheRawData: true,
    key: "bolus_id",
    load: function (loadOptions) {
        return $.getJSON('Dashboard1.aspx?SP=GetRiskData');
    }
});

// Check
var checkdata = new DevExpress.data.CustomStore({
    loadMode: "raw",
    cacheRawData: true,
    key: "bolus_id",
    load: function (loadOptions) {
        return $.getJSON('Dashboard1.aspx?SP=GetCheckData');
    }
});
$(function () {
    $("#gridCheck").dxDataGrid({
        dataSource: checkdata,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10],
            showInfo: true
        },
        columns: [
            {
                caption: "Animal_id",
                dataField: "animal_id",
                alignment: 'center',
                width: 80,
                cellTemplate: function (cellElement, cellInfo) {
                    var d = new Date();
                    var dt = d.getFullYear() + '-' + Number(d.getMonth() + 1) + '-' + d.getDate();
                    $('<a/>')
                        .attr({
                            href: 'BolusChart?Animal_id=' + cellInfo.value + '&Bolus_id=' + cellInfo.key + '&DateSearch=' + dt + '&SP=ShowChart'
                        })
                        .text(cellInfo.value)
                        .appendTo(cellElement);
                }
            },
            {
                caption: "Event",
                dataField: "event",
                width: 60
            },
            {
                caption: "Message",
                dataField: "message",
                width: 550
            },
            {
                caption: "Date",
                dataField: "date_emailsent",
                dataType: "date",
                format: "yyyy-MM-dd hh:mm:ss",
                width: 150
            }
        ]
    });

});

// Auto Refresh PAge by Timer
//$(function () {
//    $("#AutoRefresh").dxCheckBox({
//        value: true,
//        width: 110,
//        text: "Auto Refresh",
//        //onInitialized: function (e) {
//        //        var myTimer1 = setTimeout(function () { window.location.reload(1); }, 30000);
//        //    },
//        onValueChanged: function (e) {
//            //$("#AutoRefresh").dxCheckBox("instance").option('value',e.value);
//            if (e.value) {
//                var myTimer1 = setTimeout(function () { window.location.reload(1); }, 30000);
//            }
//            else {
//                clearTimeout(myTimer1);
//            }
//        }
//    });
//});

