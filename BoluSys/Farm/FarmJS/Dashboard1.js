//Palette for Ranges Section
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
                cssClass: 'cls',
                dataField: "animal_id",
                alignment: 'center',
                width: 80,
                cellTemplate: function (cellElement, cellInfo) {
                    var d = new Date();
                    var dt = d.getFullYear() + '-' + Number(d.getMonth() + 1) + '-' + d.getDate();
                    $('<a/>')
                        .attr({
                            //href: 'BolusChart?Animal_id=' + cellInfo.value + '&Bolus_id=' + cellInfo.key + '&DateSearch=' + dt + '&SP=ShowChart'
                            href: 'CowPage?bid_ext=' + cellInfo.key + '&dt_ext=' + dt
                        })
                        .text(cellInfo.value)
                        .appendTo(cellElement);
                }
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Event",
                dataField: "event",
                width: 60
            },
            {
                caption: "Message",
                cssClass: 'cls',
                dataField: "message",
                alignment: 'center',
                width: 550
            },
            {
                cssClass: 'cls',
                alignment: 'center',
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
                cssClass: 'cls',
                alignment: 'center',
                caption: "Animal_id",
                dataField: "animal_id",
                alignment: 'center',
                width: 80,
                cellTemplate: function (cellElement, cellInfo) {
                    var d = new Date();
                    var dt = d.getFullYear() + '-' + Number(d.getMonth() + 1) + '-' + d.getDate();
                    $('<a/>')
                        .attr({
                            href: 'CowPage?bid_ext=' + cellInfo.key + '&dt_ext=' + dt
                        })
                        .text(cellInfo.value)
                        .appendTo(cellElement);
                }
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Event",
                dataField: "event",
                width: 60
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Message",
                dataField: "message",
                width: 550
            },
            {
                cssClass: 'cls',
                alignment: 'center',
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

//grid Data Integrity
var dataIntegrity = new DevExpress.data.CustomStore({
    loadMode: "raw",
    cacheRawData: true,
    key: "r",
    load: function (loadOptions) {
        return $.getJSON('Dashboard1.aspx?SP=GetDataIntegrity');
    }
});

///Pivot grid Section
$(function () {
    var salesPivotGrid = $("#gdi").dxPivotGrid({
        fieldChooser: false,
        showBorders: true,
        showRowGrandTotals: false,
        showColumnGrandTotals: false,
        showRowTotals: false,

        onCellPrepared: function (e) {
            if (e.area == "data") {
                e.cellElement.css("text-align", "center");
                e.cellElement.css("font-weight", "bold");

                if (Number(e.cell.value) <= 5.0) {
                    e.cellElement.css("color", "green");
                    e.cellElement.attr("title", "Acceptable Data Loss");
                }
                else {
                    e.cellElement.css("color", "red");
                }
            }
        },

        dataSource: {
            fields: [
                {
                    dataField: "DateCtrl",
                    dataType: "string",
                    area: "column"
                }, {

                    caption: "Gaps",
                    dataField: "Gaps",
                    dataType: "number",
                    summaryType: "sum",
                    format: "decimal",
                    area: "data"
                },
                {
                    caption: "Gaps, %",
                    dataField: 'r',
                    area: "row"
                }
            ],
            store: dataIntegrity
        }
    }).dxPivotGrid("instance");

});


//-----------Grid Data Lost Cows List-------------------------------------------------------------

var dataLostCows = new DevExpress.data.CustomStore({
    loadMode: "raw",
    cacheRawData: true,
    key: "bolus_id",
    load: function (loadOptions) {
        return $.getJSON('Dashboard1.aspx?SP=GetLostCowsList&period_hour=1');
    }
});

///grid for Lost Cows List
$(function () {
   
    $("#period_hour").dxNumberBox({
        value: 1,
        min: 1,
        max: 8,
        width: 50,
        showSpinButtons: true,
        onValueChanged: function (e) {
            dataLostCows = new DevExpress.data.CustomStore({
                loadMode: "raw",
                cacheRawData: true,
                key: "bolus_id",
                load: function (loadOptions) {
                    return $.getJSON('Dashboard1.aspx?SP=GetLostCowsList&period_hour='+e.value);
                }
            });

            $("#lostCows_grid").dxDataGrid('instance').option('dataSource', dataLostCows);
        }
    });
    $("#lostCows_grid").dxDataGrid({
        dataSource: dataLostCows,
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
                cssClass: 'cls',
                alignment: 'center',
                caption: "Animal_id",
                dataField: "animal_id",
                alignment: 'center',
                width: 80,
                cellTemplate: function (cellElement, cellInfo) {
                    var d = new Date();
                    var dt = d.getFullYear() + '-' + Number(d.getMonth() + 1) + '-' + d.getDate();
                    $('<a/>')
                        .attr({
                            href: 'CowPage?bid_ext=' + cellInfo.key + '&dt_ext=' + dt
                        })
                        .text(cellInfo.value)
                        .appendTo(cellElement);
                }
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Date",
                dataField: "lastdate",
                dataType: "date",
                format: "yyyy-MM-dd hh:mm:ss",
                width: 150
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Lactation Stage",
                dataField: "Current_Stage_Of_Lactation",
                width: 150
            }
        ]
    });

});
