//Initial Data

var AlertEmail = $("#AlertEmail").val();
var AlertTemp = $("#AlertTemperatureInd").val();
var AlertWaterIntakesInd = $("#AlertWaterIntakesInd").val();
var AlertOutOfRangeInd = $("#AlertOutOfRangeInd").val();

var ReportAnimalsIdList = $("#ReportAnimalsIdList").val();
var ReportTempChartInd = $("#ReportTempChartInd").val();
var ReportStatInd = $("#ReportStatInd").val();

//Alerts Section--------------------------------------------------------------------
$(function () {
    //var AlertTemp = $("#AlertTemperatureInd").val();
    $("#cbx_TempSignal").dxCheckBox({
        value: parseInt(AlertTemp),
        text: "Temperature High/Low",
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert("Set= " + newValue);
        }
    });
});
$(function () {
    $("#cbx_IntakesSignal").dxCheckBox({
        "text": "Water Intakes Often/Rarely",
        value: parseInt(AlertWaterIntakesInd),
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert("Set= " + newValue);
        }
    });
});
$(function () {
    $("#cbx_OutOfRangeSignal").dxCheckBox({
        "text": "Out Of Range > 2 Hours",
        value: parseInt(AlertOutOfRangeInd),
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert("Set= " + newValue);
        }
    });
});

//Anomalies button section---------------------

$(function () {
    $("#Btn_ShowAnamaliesTemp").dxButton({
        stylingMode: "contained",
        text: "Show Message",
        hint: "Show Anomalies Template",
        type: "normal",
        width: 150,
        onClick: function () {
            showInfoAnom(data_Anomaly);
        }
    });

});
//Anomalies Popover section 
var data_Anomaly = {
    "Animal_ID": "1025",
    "Temperature": "45",
    "WaterIntakes": "12",
    "OutOfRange": "From 14:00 To 18:00"
};

var msg_Anomaly = {},
    popup = null,
    popupOptions = {
        width: 600,
        height: 250,
        contentTemplate: function () {
            var TempSignal = $("#cbx_TempSignal").dxCheckBox("instance").option("value");
            var IntakesSignal = $("#cbx_IntakesSignal").dxCheckBox("instance").option("value");
            var OutOfRangeSignal = $("#cbx_OutOfRangeSignal").dxCheckBox("instance").option("value");
            // Do not send Alarts
            var result = null;
            if (TempSignal == false && IntakesSignal == false && OutOfRangeSignal == false) {
                result = "<p>Do not send Alerts!</p>";
                return $("<div />").append($(result));
            }
            // Table header 
            var result = "<table class='table table-bordered'>";
            if (TempSignal) {
                result += "<tr><th>Animal_ID</th><th>Temperature</th>";
            }
            if (IntakesSignal) {
                result += "<th>Water Intakes</th>";
            }
            if (OutOfRangeSignal) {
                result += "<th>Out Of Range</th>";
            }
            result += "</tr>";
            //---------------------Data Row----------------------------
            result += "<tr>";
            if (TempSignal) {
                result += "<td>1025</td><td style='background-color: #ffcccc;'>42.0</td>";
            }
            if (IntakesSignal) {
                result += "<td>2</td>";
            }
            if (OutOfRangeSignal) {
                result += "<td>0</td>";
            }
            result += "</tr>";
            return $("<div />").append(
                $(result),
                $("<p>Date         : <span>" + Date() + "</span></p>"
                )
            );
        },
        showTitle: true,
        title: "Anomalies List",
        visible: false,
        dragEnabled: false,
        closeOnOutsideClick: true
    };

var showInfoAnom = function (data) {
    msg_Anomaly = data;
    if (popup) {
        $(".popup").remove();
    }
    var $popupContainer = $("<div />")
        .addClass("popup")
        .appendTo($("#popup"));
    popup = $popupContainer.dxPopup(popupOptions).dxPopup("instance");
    popup.show();
};
//Alerts Section End--------------------------------------------------------------------

//Report Section--------------------------------------------------------------------
//select animal list for Report
var animals = new DevExpress.data.CustomStore({
    loadMode: "raw",
    load: function () {
        return $.getJSON("UserCabinet.aspx?PARAM=Get_AnimalsIdList");
    }
});
//var animals = str.substring(0, str.length - 1).substring(1); ///[{ID:10,Animal_ID:1125}];//
//alert(animals);
var animals_ini = JSON.parse($("#AnimalsIdListIni").val());


$(function () {
    var treeView;

    var syncTreeViewSelection = function (treeView, value) {
        if (!value) {
            treeView.unselectAll();
            return;
        }

        value.forEach(function (key) {
            treeView.selectItem(key);
        });
    };

    var makeAsyncDataSource = function (jsonFile) {
        return new DevExpress.data.CustomStore({
            loadMode: "raw",
            key: "ID",
            load: function () {
                return $.getJSON("data/" + jsonFile);
            }
        });
    };

    var getSelectedItemsKeys = function (items) {
        var result = [];
        items.forEach(function (item) {
            if (item.selected) {
                result.push(item.key);
            }
            if (item.items.length) {
                result = result.concat(getSelectedItemsKeys(item.items));
            }
        });
        return result;
    };

    $("#Sct_AnimalList").dxDropDownBox({
        //value: ["1"],
        text: "Animal IDs",
        valueExpr: "ID",
        displayExpr: "Animal_ID",
        placeholder: "Select a value...",
        showClearButton: true,
        dataSource: animals,//makeAsyncDataSource("treeProducts.json"),
        contentTemplate: function (e) {
            var value = e.component.option("value"),
                $treeView = $("<div>").dxTreeView({
                    dataSource: e.component.option("dataSource"),
                    dataStructure: "plain",
                    keyExpr: "ID",
                    parentIdExpr: "categoryId",
                    selectionMode: "multiple",
                    displayExpr: "Animal_ID",
                    selectByClick: true,
                    onContentReady: function (args) {
                        syncTreeViewSelection(args.component, animals_ini);
                    },
                    selectNodesRecursive: false,
                    showCheckBoxesMode: "normal",
                    onItemSelectionChanged: function (args) {
                        var nodes = args.component.getNodes(),
                            value = getSelectedItemsKeys(nodes);
                        e.component.option("value", value);
                    }
                });

            treeView = $treeView.dxTreeView("instance");

            //syncTreeViewSelection(treeView, animals_ini);

            e.component.on("valueChanged", function (args) {
                var value = args.value;
                syncTreeViewSelection(treeView, value);
            });

            return $treeView;
        }
    });

});
//Report parts section----------------------
$(function () {
    $("#cbx_TempChart").dxCheckBox({
        value: parseInt(ReportTempChartInd),
        text: "Temperature Chart",
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert("Set= " + newValue);
        }
    });
});
$(function () {
    $("#cbx_StatTab").dxCheckBox({
        value: parseInt(ReportStatInd),
        text: "Stat",
        onValueChanged: function (e) {
            var previousValue = e.previousValue;
            var newValue = e.value;
            // Event handling commands go here
            //alert("Set= " + newValue);
        }
    });
});



//Show Report buttons section------------------------ 
$(function () {
    $("#Btn_ShowReportTemp").dxButton({
        stylingMode: "contained",
        text: "Show Report",
        hint: "Show Report Template",
        type: "normal",
        width: 150,
        onClick: function () {
            //showInfoReport(data_Report);
            showInfoReport();
        }
    });
});

var showInfoReport = function () {
    //msg_Report = data;
    if (popup) {
        $(".popup").remove();
    }
    var $popupContainer = $("<div />")
        .addClass("popup")
        .appendTo($("#popup_Rep"));
    popup = $popupContainer.dxPopup(popupOptions).dxPopup("instance");
    popup.show();
};

var msg_Report = {},
    popup = null,
    popupOptions = {
        width: 1000,
        height: 700,
        contentTemplate: function () {
            var cbx_TempChart = $("#cbx_TempChart").dxCheckBox("instance").option("value");
            var cbx_StatTab = $("#cbx_StatTab").dxCheckBox("instance").option("value");
            // Do not send Alarts
            var result = "<p>Date         : <span>" + Date() + "</span></p>";
            if (cbx_TempChart == false && cbx_StatTab == false) {
                result += "<p>Report is Empty</p>";
                return $("<div />").append($(result));
            }
            //  
            if (cbx_TempChart && !cbx_StatTab) {
                result += "<img src='imgs/DayReportChart.jpg' height='480' width='800'>";
                return $("<div />").append($(result));
            }
            if (cbx_StatTab && !cbx_TempChart) {
                result += "<p><img src='imgs/DayReportStat.jpg' height='320' width='800'></p>";
                return $("<div />").append($(result));
            }
            if (cbx_TempChart && cbx_StatTab) {
                result += "<p><img src='imgs/DayReportFull.jpg' height='600' width='800'></p>";
                return $("<div style='overflow: scroll;'/>").append($(result));
            }
            //return $("<div style='overflow: scroll;'/>").append($(result));
        },
        showTitle: true,
        title: "Report Daily",
        visible: false,
        dragEnabled: false,
        closeOnOutsideClick: true
    };


//Email Section

$("#email").dxTextBox({})
    .dxValidator({
        validationRules: [{
            type: "required",
            message: "Email is required"
        }, {
            type: "email",
            message: "Email is invalid"
        }]
    });
$(function () {
    var emailEditor = $("#email").dxTextBox({
        value: AlertEmail,
        readOnly: false,
        hoverStateEnabled: false,
        hint: "This email is using for Alerts and Reports",
        type: "required",
        mode: "email"
    }).data("dxTextBox");
});

//Test Settings section
$(function () {
    $("#Btn_SendAlerts").dxButton({
        stylingMode: "contained",
        text: "Send Alerts",
        hint: "Send Alerts By email",
        type: "normal",
        width: 150,
        onClick: function () {
            DevExpress.ui.notify("Send Alerts By email");
        }
    });
});
$(function () {
    $("#Btn_SendReport").dxButton({
        stylingMode: "contained",
        text: "Send Report",
        hint: "Send Report By email",
        type: "normal",
        width: 150,
        onClick: function () {
            DevExpress.ui.notify("Send Report By email");
        }
    });
});

//Save Settings section
var data_btnindc, btn_idic;
$(function () {
    $("#Btn_SaveSettings").dxButton({
        stylingMode: "contained",
        text: "Save Settings",
        hint: "Save Settings",
        type: "normal",
        width: 150,
        template: function (data, container) {
            $("<div class='button-indicator'></div><span class='dx-button-text'>" + data.text + "</span>").appendTo(container);
            buttonIndicator = container.find(".button-indicator").dxLoadIndicator({
                visible: false
            }).dxLoadIndicator("instance");
        },
        onClick: function (data) {
            //indicator switched on
            btn_idic = buttonIndicator;
            data_btnindc = data;
            data.component.option("text", "Saving...");
            buttonIndicator.option("visible", true);
            //-------------------------------------------------------------
            AlertEmail = $("#email").dxTextBox("instance").option("value");
            AlertTemp = $("#cbx_TempSignal").dxCheckBox("instance").option("value");
            AlertWaterIntakesInd = $("#cbx_IntakesSignal").dxCheckBox("instance").option("value");
            AlertOutOfRangeInd = $("#cbx_OutOfRangeSignal").dxCheckBox("instance").option("value");
            ReportStatInd = $("#cbx_StatTab").dxCheckBox("instance").option("value");
            ReportTempChartInd = $("#cbx_TempChart").dxCheckBox("instance").option("value");

            var params = {};
            params.mail = AlertEmail;
            params.AlertTemp = AlertTemp ? 1 : 0;;
            params.AlertWaterIntakesInd = AlertWaterIntakesInd ? 1 : 0;
            params.AlertOutOfRangeInd = AlertOutOfRangeInd ? 1 : 0;;
            params.ReportStatInd = ReportStatInd ? 1 : 0;;
            params.ReportTempChartInd = ReportTempChartInd ? 1 : 0;;
            //
            var aidl = $("#Sct_AnimalList").dxDropDownBox("instance").option("text");//.split(',');
            if (aidl.length == 0) {
                alert("Please, set Animals List");
                buttonIndicator.option("visible", false);
                data.component.option("text", "Save Settings");
                return;
            }
            params.ReportAidList = aidl;

            //DevExpress.ui.notify("params=" + parms);
            //return $.getJSON("UserCabinet.aspx?PARAM=Get_AnimalsIdList");

            var urlf = 'UserCabinet.aspx/UserCabinetSaveSettings';//?PARAM=UserCabinetSaveSettings';

            var ress = myAjaxRequestJsonA(urlf, params, Success_function_SaveSettings, Error_SaveSettings);
            if (ress) {
                ;
            }
            ;
            //buttonIndicator.option("visible", false);
            //data1.component.option("text", "Save Settings");
        }
    });
});
function Success_function_SaveSettings(result) {
    //data_btnindc, btn_idic
    btn_idic.option("visible", false);
    data_btnindc.component.option("text", "Save Settings");

    alert("Settings were saved successfully, " + result.d);
    //refresh page
    //location.reload();
    return true;
}
function Error_SaveSettings(result) {
    //data_btnindc, btn_idic
    btn_idic.option("visible", false);
    data_btnindc.component.option("text", "Save Settings");
    alert("Settings saving was failed ex=, " + result.d);
    return false;
}
function myAjaxRequestJsonA(URL, Param, Success_function_name, Error_function_name) {
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