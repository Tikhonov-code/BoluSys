$(function () {
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
            return $.getJSON('AdminCowsLogs.aspx?SP=GetCowsLogs');
        },
        insert: function (values) {
            var deferred = $.Deferred();
            $.post("AdminCowsLogs.aspx?SP=InsertCowsLogs", values).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        },
        update: function (key, values) {
            var deferred = $.Deferred();
            $.post("AdminCowsLogs.aspx?SP=UpdateCowsLogs&id="+ encodeURIComponent(key), values).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        },
        remove: function (key) {
            var deferred = $.Deferred();
            $.post("AdminCowsLogs.aspx?SP=RemoveCowsLogs&id=" + encodeURIComponent(key)).done(function (data) {
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
            return $.getJSON('AdminCowsLogs.aspx?SP=GetAnimalList');
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
});
