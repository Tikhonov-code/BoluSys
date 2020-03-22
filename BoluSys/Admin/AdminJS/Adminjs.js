var titlecolor = "#e6f0ff";
var tableborders = '';//"style='border-color: #b3d1ff; border-style:solid;'";

var alertlist = "<div id='AlertsList' " + tableborders + "><div class='row' style = 'text-align: center; padding: 10px 0; border-width: thin; background-color: " + titlecolor + ";'>" +
    "<h3>Alerts List</h3>300 latest records</div ><div class='demo-container'><div id='gridContainer'></div></div></div >";

var datagaps = "<div id='DataGapsDiv' " + tableborders + "><div class='row' style='text-align: center; padding: 10px 0; border-width: thin; background-color: " + titlecolor + ";'>" +
    "<h3>Data Gaps</h3></div><div class='row'><div class='col-md-1' style='text-align: right; padding: 10px 0;'>Date From:</div>" +
    "<div class='col-md-3'><div id='DateFrom'></div></div><div class='col-md-1' style='text-align: right; padding: 10px 0;'>Date To:</div>" +
    "<div class='col-md-3'><div id='DateTo'></div></div><div class='col-md-4'><div id='SearchGaps'></div></div></div><div class='container'>" +
    "<div id='GridGaps'></div></div></div></div>";
var datagapsPercent = "<div id='DataGapsDiv' " + tableborders + "><div class='row' style='text-align: center; padding: 10px 0; border-width: thin; background-color: " + titlecolor + ";'>" +
    "<h3>Data Gaps In % For Herd</h3></div><div class='row'>" +
    "<div class='col-sm-1' style='text-align: left; padding: 10px 0;'>Farm:</div>" +
    "<div class='col-sm-1'  id='farm_user'></div>"+
    "<div class='col-sm-1' style='text-align: right; padding: 10px 0;'>Date From:</div>" +
    "<div class='col-sm-3'><div id='DateFrom'></div></div>" +
    "<div class='col-sm-1' style='text-align: right; padding: 10px 0;'>Date To:</div>" +
    "<div class='col-sm-2'><div id='DateTo'></div></div>" +
    "<div class='col-sm-2' style='text-align: left;'><div id='SearchGaps'></div></div></div><div class='container'>" +
    "<div id='GridGaps'></div></div></div></div>";

var wiReport = "<div id='WiReportDiv' " + tableborders + "><div class='row' style='text-align: center; padding: 10px 0; border-width: thin; background-color: " + titlecolor + ";'>" +
    "<h3>Water Intakes Report</h3></div><div class='row'><div class='col-md-1' style='text-align: right; padding: 10px 0;'>Report Date:</div>" +
    "<div class='col-md-3'><div id='DateWiReport'></div></div>" +
    "<div class='col-md-3'></div><div class='col-md-4'><div id='WiReportGo'></div></div></div><div class='container'>" +
    "<div id='GridWiReport'></div></div></div></div>";

var admincowslogs = "<div id='admincowslogs' " + tableborders + "> <div class='row' style='text-align: center; padding: 10px 0; border-width: thin; background-color: " + titlecolor + ";'>" +
    "<h3>Cows Log</h3></div><div class='demo-container'><div id ='grid'></div ><div id='action-add'></div><div id='action-remove'></div><div id='action-edit'></div></div>";

var TNNRConvert = "<div id='TTNRawConverter' " + tableborders + "><div class='row' style='text-align: center; padding: 10px 0; border-width: thin; background-color:  " + titlecolor + ";' >" +
    "<h3>TTN Raw Converter</h3></div></div><div style='border-color: #b3d1ff; border-style: solid;'><div class='form'><div class='dx-fieldset'><div class='dx-field'>" +
    "<div class='dx-field-label'>Input Raw Value:</div><div class='dx-field-value'><div id='RawValue'></div></div></div><div class='dx-field'>" +
    "<div class='dx-field-label'>Input Raw Value:</div><div class='dx-field-value'><div id='TimeZ'></div></div></div><div style='text-align: center;'>" +
    "<div id='RunConverter'></div></div></div></div><div><div class='dx-fieldset'><div class='dx-fieldset-header'>Results:</div><div class='dx-field'>" +
    "<div id='ResultsBox'></div></div></div></div></div>";
//---------------------------------------------------------------------------
function AlertsListShow() {

    $("#PanelSWhow").html(alertlist);

    var ds = new DevExpress.data.CustomStore({
        loadMode: "raw",
        load: function () {
            return $.getJSON("admin.aspx?SP=Get10");
        }
    });
    $("#gridContainer").dxDataGrid({
        dataSource: ds,
        showBorders: true,
        noDataText: "No Data",
        columnAutoWidth: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        headerFilter: {
            visible: true,
            allowSearch: true
        },
        columns: [

            {
                capture: "Farm",
                dataField: "Name",
                width: 150,
                allowFiltering: true
            },
            "bolus_id", "event",
            {
                dataField: "message",
                width: 500,
                height: 50
            },
            {
                dataField: "date_emailsent",
                width: 150,
                dataType: "datetime"
            },
            {
                dataField: "email",
                width: 200
            }]
    });

};

//-------Data Gaps Section----------------------------------------
//-------Intervals in mins----------------------------------------
function DataGapsShow() {
    $("#PanelSWhow").html(datagaps);

    //----------------------------------------------------
    var now = new Date();
    var now_begin = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
    var now_end = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59, 59);

    $("#DateFrom").dxDateBox({
        type: "datetime",
        name: "Date From:",
        value: now_begin
    });



    $("#DateTo").dxDateBox({
        type: "datetime",
        value: now_end
    });

    var data_db;

    $("#SearchGaps").dxButton({
        //stylingMode: "outlined",
        text: "Find Gaps In Data",
        elementAttr: {
            title: "Click To Find",
            style: "background-color: #337ab7; color:white;"
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
                    return $.getJSON('Admin.aspx?SP=GetGapsData&dt0=' + dt0 + '&dt1=' + dt1);
                }
            });

            FillData(data_db);
        }
    });
};
function FillData(data_db) {
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
        headerFilter: {
            visible: true,
            allowSearch: true
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
                dataField: "interval",
                dataType: "number"
            }]
    });

};
//-------Gaps in % for Herd----------------------------------------

function GapsByFarmHerdShow() {
    $("#PanelSWhow").html(datagapsPercent);

    $.getJSON('admin.aspx?SP=GetFarmNameList',
        function (result) {
            $("#farm_user").dxSelectBox({
                dataSource: result,
                displayExpr: "Name",
                valueExpr: "AspNetUser_Id",
                //value: result[0].ID,
                width: "200"
            });
        });

    //----------------------------------------------------
    var now = new Date();
    var now_begin = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0, 0);
    var now_end = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59, 59);

    $("#DateFrom").dxDateBox({
        type: "datetime",
        name: "Date From:",
        value: now_begin,
        width: "200"
    });

    $("#DateTo").dxDateBox({
        type: "datetime",
        value: now_end,
        width: "200"
    });

    var data_db;

    $("#SearchGaps").dxButton({
        //stylingMode: "outlined",
        text: "Report",
        elementAttr: {
            title: "Click To Find",
            style: "background-color: #337ab7; color:white;"
        },
        width: 80,
        onClick: function () {
            data_db = new DevExpress.data.CustomStore({
                loadMode: "raw",
                cacheRawData: true,
                key: "bolus_id",
                load: function (loadOptions) {
                    var dt0 = ConvertDateToMyF($("#DateFrom").dxDateBox("instance").option("value"));
                    var dt1 = ConvertDateToMyF($("#DateTo").dxDateBox("instance").option("value"));
                    var userid = $("#farm_user").dxSelectBox("instance").option("value");
                    return $.getJSON('Admin.aspx?SP=GetDataGapsPercent&dt0=' + dt0 + '&dt1=' + dt1+"&userid="+userid);
                }
            });

            FillDataGapsPercent(data_db);
        }
    });
};
function FillDataGapsPercent(data_db) {
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
        headerFilter: {
            visible: true,
            allowSearch: true
        },
        //columns: [
        //    {
        //        cssClass: 'cls',
        //        alignment: 'center',
        //        caption: "Bolus_id",
        //        dataField: "bolus_id",
        //        width:100
        //    },
        //    {
        //        cssClass: 'cls',
        //        alignment: 'center',
        //        caption: "Animal_id",
        //        dataField: "animal_id",
        //        width: 100
        //    }]
    });

};

//END-------Data Gaps Section----------------------------------------

//--------Water Intakes Report Section----------------------------------------
function WIReportShow() {
    $("#PanelSWhow").html(wiReport);

    //----------------------------------------------------
    var now = new Date();

    $("#DateWiReport").dxDateBox({
        type: "datetime",
        name: "Do Report:",
        value: now
    });

    var widata_db;

    $("#WiReportGo").dxButton({
        //stylingMode: "outlined",
        text: "Do Report",
        elementAttr: {
            title: "Refresh Report",
            style: "background-color: #337ab7; color:white;"
        },
        width: 150,
        onClick: function () {
            widata_db = new DevExpress.data.CustomStore({
                loadMode: "raw",
                cacheRawData: true,
                key: "bolus_id",
                load: function (loadOptions) {
                    var dt0 = ConvertDateToMyF($("#DateWiReport").dxDateBox("instance").option("value"));
                    return $.getJSON('Admin.aspx?SP=GetWiReportData&dt0=' + dt0);
                }
            });

            FillDataWIReport(widata_db);
        }
    });
};
function FillDataWIReport(data_db) {
    $("#GridWiReport").dxDataGrid({
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
        headerFilter: {
            visible: true,
            allowSearch: true
        },
        onCellPrepared: function (e) {
            if (e.rowType == "data") {

                if (e.data.WI20 <= -20.0) {
                    e.cellElement.css("text-align", "center");
                    e.cellElement.css("font-weight", "bold");

                    e.cellElement.css("color", "red");
                    e.cellElement.attr("title", "WI down > 20%");
                }
            }
        },
        columns: [
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Farm",
                dataField: "Name"
            },
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
                caption: "wi",
                dataField: "wi",
                format: {
                    type: "fixedPoint",
                    precision: 2
                }
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "avr_wi",
                dataField: "avr_wi",
                format: {
                    type: "fixedPoint",
                    precision: 2
                }
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "gaps",
                dataField: "gaps"
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "WI20",
                dataField: "WI20",
                format: {
                    type: "fixedPoint",
                    precision: 2
                }
            }]
    });

};

//----------------------------------------------------------------------------

//-----------Settings section-------------------------------------------------
var BolusesSet = "<div id='adminBolusesSet' " + tableborders + "> <div class='row' style='text-align: center; padding: 10px 0; border-width: thin; background-color: " + titlecolor + ";'>" +
    "<h3>Boluses Status Settings</h3></div><div class='demo-container'><div id ='gridBolusesSet'></div ><div id='action-add'></div><div id='action-remove'></div><div id='action-edit'></div></div>";

function BolusesSetShow() {
    $("#PanelSWhow").html(BolusesSet);
    BolusesSetView();
};
function BolusesSetView() {
    $("#gridBolusesSet").dxDataGrid({
        dataSource: new DevExpress.data.CustomStore({
            loadMode: "raw",
            cacheRawData: true,
            key: "bolus_id",
            load: function (loadOptions) {
                return $.getJSON('Admin.aspx?SP=GetBolusesSet');
            },
            update: function (key, values) {
                var deferred = $.Deferred();
                $.post("Admin.aspx?SP=UpdateBolusStatus&bolus_id=" + encodeURIComponent(key) + "&status=" + values.status, values).done(function (data) {
                    deferred.resolve(data.key);
                    //alert('Status was updated for bolus_id=' + encodeURIComponent(key));
                });
                return deferred.promise();
            }
        }),
        showBorders: true,
        paging: {
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        headerFilter: {
            visible: true,
            allowSearch: true
        },
        editing: {
            mode: "batch",
            allowUpdating: true,
            //allowAdding: true
        },
        columns: [
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Farm",
                dataField: "Name",
                allowEditing: false
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Bolus_id",
                dataField: "bolus_id",
                allowEditing: false
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Animal_id",
                dataField: "animal_id",
                allowEditing: false
            },
            {
                cssClass: 'cls',
                alignment: 'center',
                caption: "Status",
                dataField: "status"
            }]

        ,
        //onRowUpdating: function (e) {
        //    //var d = $.Deferred();
        //    $.getJSON("Admin.aspx?SP=UpdateBolusStatus&bolus_id=" + e.oldData.bolus_id + "&status=" + e.newData.status);//, JSON.stringify(e.data))
        //    //    .then(function (result) {
        //    //        return result=='OK' ? d.resolve() : d.reject(result.errorText);
        //    //    })
        //    //    .fail(function () {
        //    //        return d.reject();
        //    //    })
        //    //e.cancel = d.promise();
        //    alert('Status was updated for bolus_id=' + e.oldData.bolus_id);
        //}
    });
}

//----------------------------------------------------------------------------

function CowsLogShow() {
    $("#PanelSWhow").html(admincowslogs);
    CowsLogFun()
}

function CowsLogFun() {
    var selectedRowIndex = -1;

    $("#action-add").dxSpeedDialAction({
        label: "Add row",
        icon: "add",
        index: 1,
        onClick: function () {
            grid.addRow();
            grid.deselectAll();
        }
    }).dxSpeedDialAction("instance");

    var deleteSDA = $("#action-remove").dxSpeedDialAction({
        icon: "trash",
        label: "Delete row",
        index: 2,
        visible: false,
        onClick: function () {
            grid.deleteRow(selectedRowIndex);
            grid.deselectAll();
        }
    }).dxSpeedDialAction("instance");

    var editSDA = $("#action-edit").dxSpeedDialAction({
        label: "Edit row",
        icon: "edit",
        index: 3,
        visible: false,
        onClick: function () {
            grid.editRow(selectedRowIndex);
            grid.deselectAll();
        }
    }).dxSpeedDialAction("instance");

    //----------------------------------------
    var cowsList = new DevExpress.data.CustomStore({
        loadMode: "raw",
        cacheRawData: true,
        key: "id",
        load: function (loadOptions) {
            return $.getJSON('Admin.aspx?SP=GetCowsLogs');
        },
        insert: function (values) {
            var deferred = $.Deferred();
            $.post("Admin.aspx?SP=InsertCowsLogs", values).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        },
        update: function (key, values) {
            var deferred = $.Deferred();
            $.post("Admin.aspx?SP=UpdateCowsLogs&id=" + encodeURIComponent(key), values).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        },
        remove: function (key) {
            var deferred = $.Deferred();
            $.post("Admin.aspx?SP=RemoveCowsLogs&id=" + encodeURIComponent(key)).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        }
    });

    function Success_update(result) {
        return result.d;
    }
    function Error_update(xhrr, status, error) {
        return "";
    }
    //--------------------------------------------------------------------------------------------
    var AnimalList = new DevExpress.data.CustomStore({
        loadMode: "raw",
        cacheRawData: true,
        key: "id",
        load: function (loadOptions) {
            return $.getJSON('Admin.aspx?SP=GetAnimalList');
        }
    });

    var grid = $("#grid").dxDataGrid({
        dataSource: cowsList,
        showBorders: true,
        keyExpr: "id",
        selection: {
            mode: "single"
        },
        paging: {
            enabled: true
        },
        editing: {
            mode: "popup",
            texts: {
                confirmDeleteMessage: "Done"
            }
        },
        onSelectionChanged: function (e) {
            selectedRowIndex = e.component.getRowIndexByKey(e.selectedRowKeys[0]);

            deleteSDA.option("visible", selectedRowIndex !== -1);
            editSDA.option("visible", selectedRowIndex !== -1);
        },
        columns: [
            {
                dataField: "id",
                width: 125,
                allowEditing: false
            },
            {
                dataField: "animal_id",
                //allowEditing: false,
                width: 125,
                lookup: {
                    dataSource: AnimalList,
                    displayExpr: "animal_id",
                    valueExpr: "animal_id"
                }
            },
            {
                dataField: "Event",
                width: 125
            },
            {
                dataField: "Description",
            },
            {
                dataField: "Event_Date",
                dataType: "date",
                width: 125
            }
        ]
    }).dxDataGrid("instance");
}

// TTN Raw Converter--------------------------------------------------

function TTNRawConverter() {

    $("#PanelSWhow").html(TNNRConvert);

    $("#RawValue").dxTextBox({
        value: "DuAOzg7FDqINeQ9rD3EPdw9/D3UPYw9RD04PXA9aD04=",
        placeholder: "Paste Raw Value Here...",
        showClearButton: true
    });
    $("#TimeZ").dxTextBox({
        value: "2020-02-02T08:59:36.612605468Z",
        placeholder: "Paste Time Z Value Here...",
        showClearButton: true
    });


    $("#RunConverter").dxButton({
        //stylingMode: "outlined",
        text: "Convert Data",
        elementAttr: {
            title: "Convert Data",
            style: "background-color: #337ab7; color:white; text-align: center;"
        },
        width: 150,
        onClick: function () {
            var dt0 = $("#TimeZ").dxTextBox("instance").option("value");
            var rawval = $("#RawValue").dxTextBox("instance").option("value");
            var url = "Admin.aspx?SP=TTNRawConvertData&TimeZ=" + dt0 + "&RawValue=" + rawval;

            var Param = {};
            Param.SP = "TTNRawConvertData";
            myAjaxRequestJsonE(url, Param, Success_LongText, Error_LongText);

        }
    });
}

function Success_LongText(result) {

    TTNFillData(result);
    return result;
}
function Error_LongText(result) {

    var x = result.d;
    return result.d;
}

// Results Text Area-----------------------------------------------
function TTNFillData(longText) {
    $("#ResultsBox").dxTextArea({
        value: longText,
        height: 320,
        elementAttr: {
            style: "background-color: #e6f0ff; "
        },
    }).dxTextArea("instance");
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