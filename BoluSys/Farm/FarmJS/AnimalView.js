$(function () {
    //[{ bolus_id: 6, animal_id: 66 }, { bolus_id: 7, animal_id: 77 }];
    var animals = $.getJSON('AnimalView.aspx?SP=GetAnimalList'
        , function (result) {
            ShowParametersForm(result);
        });
});

function ShowParametersForm(animals) {
    var now = new Date();

    $("#form").dxForm({
        colCount: 4,
        //formData: parameters,
        items: [{
            dataField: "DateFrom",
            editorType: "dxDateBox",
            editorOptions: {
                type: "datetime",
                value: now,
                width: "100%"
            }
        }, {
            dataField: "DateTo",
            editorType: "dxDateBox",
            editorOptions: {
                type: "datetime",
                value: now,
                width: "100%"
            }
        }, {
            dataField: "Animal_id",
            editorType: "dxSelectBox",
            editorOptions: {
                items: animals,
                searchEnabled: true,
                displayExpr: "animal_id",
                valueExpr: "bolus_id",
                value: animals[0].animal_id,//ds[0].bolus_id, 
            }
            }, {
                editorType: "dxButton",
                editorOptions: {
                    text: "Do Chart",
                    type: "default",
                    onClick: function (e) {
                        var dt0 = ConvertDateToMyF( $("#form").dxForm("instance").getEditor("DateFrom").option('value'));
                        var dt1 = ConvertDateToMyF($("#form").dxForm("instance").getEditor("DateTo").option('value'));
                        var bid = $("#form").dxForm("instance").getEditor("Animal_id").option('value');

                        var url = 'AnimalView.aspx?SP=GetDataForChart&dt0=' + dt0 + '&dt1=' + dt1 + '&bid=' + bid;
                        var datachart = $.getJSON(url
                            , function (result) {
                                ShowChart(result);
                            });
                    }
                }
            }
        ]
    });

   // $("#form").dxForm("instance").validate();
}

//Chart section--------------------------------------
var intervals = [{
    displayName: "One week",
    interval: "week"
}, {
    displayName: "Two weeks",
    interval: { weeks: 2 }
}, {
    displayName: "Month",
    interval: "month"
}];

var functions = [{
    displayName: "Average",
    func: "avg"
}, {
    displayName: "Minimum",
    func: "min"
}, {
    displayName: "Maximum",
    func: "max"
}];



//----------------------------------------
function ShowChart(dataSource) {
    var chart = $("#chart").dxChart({
        dataSource: dataSource,
        title: "Temperature and Water Intakes",
        commonSeriesSettings: {
            argumentField: "bolus_full_date",
            type: "spline"
        },
        argumentAxis: {
            argumentType: "datetime",
            aggregationInterval: "week",
            valueMarginsEnabled: false
        },
        valueAxis: [{
            name: "temperature",
            title: {
                text: "Temperature, °C",
                font: {
                    color: "#e91e63"
                }
            },
            label: {
                font: {
                    color: "#e91e63"
                }
            }
        }, {
            name: "precipitation",
            position: "right",
            title: {
                text: "Water Intakes, litrs",
                font: {
                    color: "#03a9f4"
                }
            },
            label: {
                font: {
                    color: "#03a9f4"
                }
            }
        }
        ],
        legend: {
            visible: false
        },
        series: [{
            axis: "precipitation",
            color: "#03a9f4",
            type: "bar",
            valueField: "precip",
            name: "Precipitation"
        }, {
            axis: "temperature",
            color: "#ffc0bb",
            type: "rangeArea",
            rangeValue1Field: "minTemp",
            rangeValue2Field: "maxTemp",
            aggregation: {
                enabled: true,
                method: "custom",
                calculate: function (aggregationInfo, series) {
                    if (!aggregationInfo.data.length) {
                        return;
                    }
                    var temp = aggregationInfo.data.map(function (item) { return item.temp; }),
                        maxTemp = Math.max.apply(null, temp),
                        minTemp = Math.min.apply(null, temp);

                    return {
                        date: aggregationInfo.intervalStart,
                        maxTemp: maxTemp,
                        minTemp: minTemp
                    };
                }
            },
            name: "Temperature range"
        }, {
            axis: "temperature",
            color: "#e91e63",
                valueField: "temperature",
            point: {
                size: 7
            },
            aggregation: {
                enabled: true
            },
            name: "Average temperature"
        }],
        tooltip: {
            enabled: true,
            customizeTooltip: function (arg) {
                return customTooltipHandlers[arg.seriesName](arg, arg.point.aggregationInfo);
            }
        }
    }).dxChart("instance");

    $("#useAggregationToggle").dxCheckBox({
        text: "Aggregation enabled",
        value: true,
        onValueChanged: function (data) {
            chart.option("series[1].aggregation.enabled", data.value);
            chart.option("series[2].aggregation.enabled", data.value);
        }
    });

    $("#aggregateIntervalSelector").dxSelectBox({
        items: intervals,
        value: "week",
        valueExpr: "interval",
        displayExpr: "displayName",
        onValueChanged: function (data) {
            chart.option("argumentAxis.aggregationInterval", data.value);
        }
    });

    $("#aggregateFunctionSelector").dxSelectBox({
        items: functions,
        value: "avg",
        valueExpr: "func",
        displayExpr: "displayName",
        onValueChanged: function (data) {
            chart.option("series[2].aggregation.method", data.value);
        }
    });

    var customTooltipHandlers = {
        "Average temperature": function (arg, aggregationInfo) {
            var start = aggregationInfo && aggregationInfo.intervalStart,
                end = aggregationInfo && aggregationInfo.intervalEnd;

            return {
                text: (!aggregationInfo ?
                    "Date: " + arg.argument.toDateString() :
                    "Interval: " + start.toDateString() +
                    " - " + end.toDateString()) +
                    "<br/>Temperature: " + arg.value.toFixed(2) + " °C"
            };
        },
        "Temperature range": function (arg, aggregationInfo) {
            var start = aggregationInfo.intervalStart,
                end = aggregationInfo.intervalEnd;

            return {
                text: "Interval: " + start.toDateString() +
                    " - " + end.toDateString() +
                    "<br/>Temperature range: " + arg.rangeValue1 +
                    " - " + arg.rangeValue2 + " °C"
            };
        },
        "Precipitation": function (arg) {
            return {
                text: "Date: " + arg.argument.toDateString() +
                    "<br/>Precipitation: " + arg.valueText + " mm"
            };
        }
    };
}
