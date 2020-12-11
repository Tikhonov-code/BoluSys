var dt = new Date();
var dt10 = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0);
var dt20 = new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 24, 0, 0, 0);


$(function () {

    var bid_ext = $("#MainContent_bid_ext").val();
    // alert("bid_ext = " + bid_ext);

    var farmName = $.getJSON("CowPage.aspx?SP=GetFarmName"
        , function (result) {
            ShowForm(result, bid_ext);
            if (bid_ext == null || bid_ext == "") {
                bid_ext = result.al[0].bolus_id;
            }
            //alert("here bid_ext= " + bid_ext);
            TodayCowpage(bid_ext);
        },
        function (result, status, xhr) {
            var d = result;
        }
    );
});
function TodayCowpage(bid_ext) {

    var dt = new Date();
    var dt1 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0));
    var dt2 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 24, 0, 0, 0));

    ChartCreate('#chart_temp', dt1, dt2, bid_ext);
    IntakesChart_Show(dt1, dt2, bid_ext);
    DataGapsShow(bid_ext, dt1, dt2);
    GetCowInfo(bid_ext);
    GetGapsDataValue(dt1, dt2, bid_ext);
    // farmer feedback table
    FarmerFeedbackTable(bid_ext);
}

function FindBolusIDindex(bidlist, par) {
    var bidindex = 0;
    var num = bidlist.length;
    for (var i = 0; i < num; i++) {
        if (bidlist[i].bolus_id == par) {
            bidindex = i;
            break;
        }
    }
    return bidindex;
}

//window.onload = function () {
//    alert("Now");
//};

function ShowForm(animalList, bid_ext) {

    var bidlist = animalList.al;
    var bid_index = 0;
    if (bid_ext != null) {
        bid_index = FindBolusIDindex(bidlist, bid_ext);
    }

    //------------------------------------------
    $("#form").dxForm({
        formData: animalList[0],
        colCount: 4,
        items: [
            {
                itemType: "group",
                //caption: "Animal ID",
                cssClass: "first-group",
                items: [{
                    dataField: "Animal_id",
                    editorType: "dxSelectBox",
                    //label: { visible: false },
                    editorOptions: {
                        items: animalList.al,
                        value: animalList.al[bid_index].bolus_id,
                        displayExpr: "animal_id",
                        valueExpr: "bolus_id",
                        elementAttr: {
                            id: 'Animal_id'
                        },
                        onValueChanged: function (e) {
                            var bid = e.value;
                            GetCowInfo(bid);
                            //------------------------------
                            //var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                            //var dt = new Date();
                            //var dt1 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0));
                            //var dt2 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 24, 0, 0, 0));
                            //$("#From").dxDateBox("instance").option('value', dt1);
                            // $("#To").dxDateBox("instance").option('value', dt2);
                            var dt1 = ConvertDateToMyF($("#From").dxDateBox("instance").option('value'));
                            var dt2 = ConvertDateToMyF($("#To").dxDateBox("instance").option('value'));

                            ChartCreate('#chart_temp', dt1, dt2, bid);
                            IntakesChart_Show(dt1, dt2, bid);
                            DataGapsShow(bid, dt1, dt2);
                            GetGapsDataValue(dt1, dt2, bid);
                            // farmer feedback table
                            FarmerFeedbackTable(bid);
                        }
                    }
                }]
            },
            {
                itemType: "group",
                cssClass: "first-group",
                colSpan: 3,
                colCount: 6,
                items: [
                    {
                        editorType: "dxButton",
                        editorOptions: {
                            stylingMode: "contained",
                            text: "Today",
                            hint: "Get today's Charts",
                            type: "success",
                            width: 120,
                            onClick: function () {

                                var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                                var dt = new Date();
                                var dt1 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0));
                                var dt2 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 24, 0, 0, 0));
                                $("#From").dxDateBox("instance").option('value', dt1);
                                $("#To").dxDateBox("instance").option('value', dt2);

                                ChartCreate('#chart_temp', dt1, dt2, bid);
                                IntakesChart_Show(dt1, dt2, bid);
                                DataGapsShow(bid, dt1, dt2);
                                GetGapsDataValue(dt1, dt2, bid);
                            }
                        }
                    },
                    {
                        editorType: "dxButton",
                        editorOptions: {
                            stylingMode: "contained",
                            text: "Week",
                            hint: "Get Charts for a week",
                            type: "success",
                            width: 120,
                            onClick: function () {
                                var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                                var dt = new Date();
                                var dt1 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate() - 7, 0, 0, 0, 0));
                                var dt2 = ConvertDateToMyF(new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 0, 0, 0, 0));
                                $("#From").dxDateBox("instance").option('value', dt1);
                                $("#To").dxDateBox("instance").option('value', dt2);

                                //----------------------------------------------------------------
                                ChartCreate('#chart_temp', dt1, dt2, bid);
                                IntakesChart_Show(dt1, dt2, bid);
                                DataGapsShow(bid, dt1, dt2);
                                GetGapsDataValue(dt1, dt2, bid);
                            }
                        }
                    },
                    {
                        editorType: "dxButton",
                        editorOptions: {
                            stylingMode: "contained",
                            icon: "chevronleft",
                            hint: "One day back",
                            type: "success",
                            width: 40,
                            onClick: function () {
                                var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                                var dt10 = new Date($("#From").dxDateBox("instance").option('value'));
                                var dt20 = new Date($("#To").dxDateBox("instance").option('value'));
                                var dt1 = ConvertDateToMyF(new Date(dt10.getFullYear(), dt10.getMonth(), dt10.getDate() - 1, 0, 0, 0, 0));
                                var dt2 = ConvertDateToMyF(new Date(dt20.getFullYear(), dt20.getMonth(), dt20.getDate() - 1, 0, 0, 0, 0));
                                $("#From").dxDateBox("instance").option('value', dt1);
                                $("#To").dxDateBox("instance").option('value', dt2);

                                //----------------------------------------------------------------
                                ChartCreate('#chart_temp', dt1, dt2, bid);
                                IntakesChart_Show(dt1, dt2, bid);
                                DataGapsShow(bid, dt1, dt2);
                                GetGapsDataValue(dt1, dt2, bid);
                            }
                        }
                    },
                    {
                        dataField: "From",
                        editorType: "dxDateBox",
                        type: "datetime",
                        label: { visible: false },
                        editorOptions: {
                            type: "datetime",
                            value: dt10,
                            elementAttr: {
                                id: 'From'
                            },
                            onValueChanged: function (e) {
                                if (e.event != undefined) {
                                    var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                                    var dt1 = ConvertDateToMyF(e.value);
                                    //dt1 = dt1.split('/').join('-');
                                    var dt2 = ConvertDateToMyF($("#To").dxDateBox("instance").option('value'));

                                    //----------------------------------------------------------------
                                    ChartCreate('#chart_temp', dt1, dt2, bid);
                                    IntakesChart_Show(dt1, dt2, bid);
                                    DataGapsShow(bid, dt1, dt2);
                                    GetGapsDataValue(dt1, dt2, bid);
                                }
                            }
                        }
                    },
                    {
                        dataField: "To",
                        editorType: "dxDateBox",
                        type: "datetime",
                        label: { visible: false },
                        editorOptions: {
                            type: "datetime",
                            value: dt20,
                            elementAttr: {
                                id: 'To'
                            },
                            onValueChanged: function (e) {
                                if (e.event != undefined) {
                                    var bid = $("#Animal_id").dxSelectBox("instance").option('value');

                                    var dt1 = ConvertDateToMyF($("#From").dxDateBox("instance").option('value'));
                                    var dt2 = ConvertDateToMyF(e.value);
                                    // dt2 = dt2.split('/').join('-');
                                    //----------------------------------------------------------------
                                    ChartCreate('#chart_temp', dt1, dt2, bid);
                                    IntakesChart_Show(dt1, dt2, bid);
                                    DataGapsShow(bid, dt1, dt2);
                                    GetGapsDataValue(dt1, dt2, bid);
                                }
                            }
                        }
                    },
                    {
                        editorType: "dxButton",
                        editorOptions: {
                            stylingMode: "contained",
                            icon: "chevronright",
                            hint: "One day ahead",
                            type: "success",
                            width: 40,
                            onClick: function () {
                                var bid = $("#Animal_id").dxSelectBox("instance").option('value');
                                var dt = new Date($("#From").dxDateBox("instance").option('value'));

                                var dt10 = new Date($("#From").dxDateBox("instance").option('value'));
                                var dt20 = new Date($("#To").dxDateBox("instance").option('value'));
                                var dt1 = ConvertDateToMyF(new Date(dt10.getFullYear(), dt10.getMonth(), dt10.getDate() + 1, 0, 0, 0, 0));
                                var dt2 = ConvertDateToMyF(new Date(dt20.getFullYear(), dt20.getMonth(), dt20.getDate() + 1, 0, 0, 0, 0));
                                $("#From").dxDateBox("instance").option('value', dt1);
                                $("#To").dxDateBox("instance").option('value', dt2);

                                //----------------------------------------------------------------
                                ChartCreate('#chart_temp', dt1, dt2, bid);
                                IntakesChart_Show(dt1, dt2, bid);
                                DataGapsShow(bid, dt1, dt2);
                                GetGapsDataValue(dt1, dt2, bid);
                            }
                        }
                    }
                ]
            }]
    });
}

//Data Gaps Grid---------------------------------------------
function DataGapsShow(bid, dt0, dt1) {

    var data_db = new DevExpress.data.CustomStore({
        loadMode: "raw",
        cacheRawData: true,
        key: "bolus_id",
        load: function (loadOptions) {
            return $.getJSON('CowPage.aspx?SP=GetGapsData&DateFrom=' + dt0 + '&DateTo=' + dt1 + '&bolus_id=' + bid);
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
        var url = 'CowPage.aspx?SP=GetGapsDataValue&DateFrom=' + dt0 + '&DateTo=' + dt1 + '&bolus_id=' + bid;
        $.getJSON(url, function (result) {
            var ind1 = result.lastIndexOf(":") + 1;
            var ind2 = result.length;
            var gap = result.slice(ind1 - ind2);
            //var mark = (Number(gap) < 5) ? "<i class='far fa-thumbs-up' style='font-size:36px; color:green;' ></i>" : "<i class='fa fa-warning' style='font-size: 48px; color: red'></i>";

            var mark = (Number(gap) < 5) ? "<i class='dx-icon-check' style='font-size:36px; color:green;' ></i>" : "<i class='fa fa-warning' style='font-size: 48px; color: red'></i>";
            $("#DataGapsValue").html(result + "% " + mark);
        });
    }

}
//end of Data Gaps Grid------------------------------------------------

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
    var x1 = dt1;
    var x2 = dt2;
    var x3 = 0;

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
function DateFormat(dtpar) {
    var dt = new Date(dtpar);
    var result = dt.getFullYear() + '-' + Number(dt.getMonth() + 1) + '-' + dt.getDate();
    return result;
}

//Cow data Info--------------------------------------------------------------
function GetCowInfo(bidpar) {
    //-------------------------------------------
    var url = 'CowPage.aspx/GetCowInfoSt';//?SP=GetCowInfoSt&bolus_id=' + bidpar;
    var Param = {};
    Param.SP = "GetCowInfoSt";
    Param.bolus_id = bidpar;
    myAjaxRequestJsonE(url, Param, Success_GetCowInfo, Error_GetCowInfo);
    return;
}
function Success_GetCowInfo(result) {
    var r = JSON.parse(result.d);
    //-----------------------------------------------------
    //r.DUE = ConvertDateToMyF(new Date(r.DUE));
    //r.DCC = ConvertDateToMyF(new Date(r.DCC));
    //r.DIM = ConvertDateToMyF(new Date(r.DIM));
    //-----------------------------------------------------
    //r.BirthDate = ConvertDateToMyF(new Date(r.BirthDate));
    //r.Calving_Due_Date = ConvertDateToMyF(new Date(r.Calving_Due_Date));
    //r.Actual_Calving_Date = ConvertDateToMyF(new Date(r.Actual_Calving_Date));
    //---------------------------------------------------------------------------
    //var ac_date = new Date(r.Actual_Calving_Date);
    //var today = new Date();
    //// To calculate the number of days between two dates 
    //var Difference_In_Days = Math.round((today - ac_date) / (1000 * 3600 * 24));
    //r.Lactation_Day = Difference_In_Days;
    cow_data_block(r);
    return;
}
function Error_GetCowInfo(xhr, status, error) {
    return false;
}

//===============================================================

//--Personal Data Section------------------------------------------------------

function cow_data_block(cow_data0) {
    CowsLogShow(cow_data0.Bolus_ID);
    var cow_data = cow_data0;
    //var cow_data = cow_data0,
    //    popup = null,
    //popupOptions = {
    //    width: 450,
    //    height: 300,
    //    contentTemplate: function () {
    //        var Openselected = '';
    //        var Dryselected = '';
    //        var Pregnantselected = '';
    //        switch (cow_data.lac_stage) {
    //            case "Open":
    //                Openselected = "selected";
    //                break;
    //            case "Dry":
    //                Dryselected = "selected";
    //                break;
    //            case "Pregnant":
    //                Pregnantselected = "selected";
    //                break;
    //            default:
    //        }
    //        return $("<div />").append(
    //            //<input id="Text1" type="text" />

    //            //$("<table><tr height='30'><td width='150px'>Birth Date:</td><td></td><td><input id='cow_bd' type='date' value=" + ConvertDateToMyF(new Date(cow_data.BirthDate)) + "/></td></tr>" +
    //            $("<table><tr height='30'><td width='150px'>Birth Date:</td><td></td><td><div id='cow_bd'></div></td></tr>" +
    //                "<tr height='30'><td width='150px'>Current Lactation:</td><td></td><td><input id='cow_clac' type='number' value='" + cow_data.Current_Lactation + "' min='0' max='9'/></td></tr>" +
    //                "<tr height='30'><td width='150px'>Lactation Stage:</td><td></td><td><select id='cow_lac_stg'>" +
    //                "<option value='Open' " + Openselected + ">Open</option>" +
    //                "<option value='Dry' " + Dryselected + ">Dry</option>" +
    //                "<option value='Pregnant' " + Pregnantselected + ">Pregnant</option></select></td></tr>" +
    //                //"<tr height='30'><td width='150px'>Calving Due Date:</td><td></td><td><input id='cow_cdd' type='date' value=" + ConvertDateToMyF(new Date(cow_data.Calving_Due_Date)) + "/></td></tr>" +
    //                "<tr height='30'><td width='150px'>Calving Due Date:</td><td></td><td><div id='cow_cdd'></div></td></tr>" +
    //                //"<tr height='30'><td width='150px'>Actual Calving Date:</td><td></td><td><input id='cow_acd' type='date' value=" + ConvertDateToMyF(new Date(cow_data.Actual_Calving_Date)) + "/></td></tr>" +
    //                "<tr height='30'><td width='150px'>Actual Calving Date:</td><td></td><td><div id='cow_acd'></div></td></tr>" +
    //                "<tr></tr><tr height='30'><td></td><td></td><td style='text-align: right; hight:'><input class='btn btn-success' type='button' value='Save' onclick='CowDataSave(" + cow_data.Bolus_ID + ");'>" +
    //                "</td></tr></table>")
    //        );
    //    },

    //    showTitle: true,
    //    title: "Information Animail_ID: " + cow_data0.Animal_ID,
    //    visible: false,
    //    dragEnabled: false,
    //    closeOnOutsideClick: true
    //};

    //var showInfo = function (data) {
    //    cow_data = data;

    //    if (popup) {
    //        popup.option("contentTemplate", popupOptions.contentTemplate.bind(this));
    //    } else {
    //        popup = $("#popup").dxPopup(popupOptions).dxPopup("instance");
    //    }

    //    popup.show();
    //    //-----------------------------------
    //    $("#cow_bd").dxDateBox({
    //        placeholder: "10/16/2018",
    //        showClearButton: true,
    //        useMaskBehavior: true,
    //        displayFormat: "shortdate",
    //        type: "date",
    //        value: ConvertDateToMyF(new Date(cow_data.BirthDate))
    //    });
    //    var xcow_cdd = cow_data.Calving_Due_Date;
    //    if (xcow_cdd == "N/A") {
    //        xcow_cdd = new Date();
    //    }

    //    $("#cow_cdd").dxDateBox({
    //        placeholder: "10/16/2018",
    //        showClearButton: true,
    //        useMaskBehavior: true,
    //        displayFormat: "shortdate",
    //        type: "date",
    //        value: ConvertDateToMyF(new Date(xcow_cdd))
    //    });
    //    var xcow_acd = cow_data.Actual_Calving_Date;
    //    if (xcow_acd == "N/A") {
    //        xcow_acd = new Date();
    //    }
    //    $("#cow_acd").dxDateBox({
    //        type: "datetime",
    //        value: ConvertDateToMyF(new Date(xcow_acd))
    //    });
    //};
    //===================================

    $("#CowInfo_form").dxForm({
        formData: cow_data,
        items: [{
            itemType: "group",
            cssClass: "first-group",
            colCount: 3,
            readOnlyItems: ["Bolus_ID"],
            items: [
                {
                    itemType: "group",
                    colCount: 3,
                    colSpan: 3,
                    items: [
                        {
                            dataField: "Group",
                            //editorType: "dxDateBox",
                            editorOptions: {
                                //value: act_cal_date,
                                readOnly: true,
                                width: 100
                            }
                        },
                        {
                            dataField: "DIM",//"Lactation_Day",
                            editorOptions: {
                                // value: lac_Day,
                                readOnly: true,
                                width: 160
                            }
                        },
                        {
                            dataField: "Bolus_ID",
                            editorOptions: {
                                readOnly: true,
                                width: 70
                            }
                        },
                        {
                            dataField: "Lactation_Number",
                            editorOptions: {
                                //value: lac_currentnum,
                                readOnly: true,
                                width: 100
                            }
                        },
                        
                        {
                            dataField: "DUE",
                            //editorType: "dxDateBox",
                            editorOptions: {
                                //value: cal_due_date,
                                readOnly: true,
                                width: 160
                            }
                        },
                        {
                            dataField: "Date_of_Birth",
                            editorOptions: {
                                readOnly: true,
                                width: 100
                            }
                        },
                        {
                            dataField: "Lactation_Stage",
                            editorOptions: {
                                //value: lac_stage,
                                readOnly: true,
                                 width: 100
                            }
                        },
                        {
                            dataField: "DCC",
                            //editorType: "dxDateBox",
                            editorOptions: {
                                //value: act_cal_date,
                                readOnly: true,
                                width: 160
                            }
                        },
                        {
                        itemType: "button",
                        buttonOptions: {
                            text: "Edit",
                            type: "success",
                            onClick: function () {

                                //showInfo(cow_data);
                                DevExpress.ui.notify("Under Construction", "warning", 1500);
                            }
                        }
                   }]
                    //]
                }]
        }]
    });

}

// Cow Data Save----------------------------------------------------------------------------------
function CowDataSave(Bolus_ID) {
    //var result = DevExpress.ui.dialog.confirm("<i>Are you sure?</i>", "Confirm changes");
    //result.done(function (dialogResult) {
    //    //alert(dialogResult ? "Confirmed" : "Canceled");
    //    if (dialogResult) {

    //rules -------------"Confirmed"----------------------------------
    var Actual_Calving_Date = $("#cow_acd").dxDateBox("instance").option("value");
    var Calving_Due_Date = $("#cow_cdd").dxDateBox("instance").option("value");
    var lactationStage = $("#cow_lac_stg").val();

    switch (lactationStage) {
        case "Open":

            if (Actual_Calving_Date == null || Actual_Calving_Date == undefined || Actual_Calving_Date == "") {
                DevExpress.ui.notify("Actual_Calving_Date required!", "warning", 1500);
                return;
            }

            $("#CowInfo_form").dxForm("instance").getEditor("Calving_Due_Date").option("value", "N/A");
            Calving_Due_Date = null;
            $("#CowInfo_form").dxForm("instance").getEditor("Actual_Calving_Date").option("value", Actual_Calving_Date);

            break;

        case "Dry":

            if (Calving_Due_Date == null || Calving_Due_Date == undefined || Calving_Due_Date == "") {
                DevExpress.ui.notify("Calving_Due_Date required!", "warning", 1500);
                return;
            }

            $("#CowInfo_form").dxForm("instance").getEditor("Calving_Due_Date").option("value", Calving_Due_Date);
            $("#CowInfo_form").dxForm("instance").getEditor("Actual_Calving_Date").option("value", "N/A");
            break;
        case "Pregnant":


            if (Calving_Due_Date == null || Calving_Due_Date == undefined || Calving_Due_Date == "") {
                DevExpress.ui.notify("Calving_Due_Date required!", "warning", 1500);
                return;
            }
            if (Actual_Calving_Date == null || Actual_Calving_Date == undefined || Actual_Calving_Date == "") {
                DevExpress.ui.notify("Actual_Calving_Date required!", "warning", 1500);
                return;
            }
            $("#CowInfo_form").dxForm("instance").getEditor("Calving_Due_Date").option("value", Calving_Due_Date);
            $("#CowInfo_form").dxForm("instance").getEditor("Actual_Calving_Date").option("value", Actual_Calving_Date);
            break;

        default:
            break;
    }
    var Cow_bd = $("#cow_bd").dxDateBox("instance").option("value");
    $("#CowInfo_form").dxForm("instance").getEditor("Current_Lactation").option("value", $("#cow_clac").val());
    $("#CowInfo_form").dxForm("instance").getEditor("BirthDate").option("value", Cow_bd);
    $("#CowInfo_form").dxForm("instance").getEditor("Lactation_Stage").option("value", lactationStage);
    var Age_Lactation = $("#cow_clac").val();

    var ac_date = new Date(Actual_Calving_Date);
    var today = new Date();
    // To calculate the no. of days between two dates 
    var Difference_In_Days = Math.round((today - ac_date) / (1000 * 3600 * 24));

    $("#CowInfo_form").dxForm("instance").getEditor("Lactation_Day").option("value", Difference_In_Days);
    //---------------------------------------------------------------------------------------------------------------
    //cow_data0.Date_of_Birth = $("#cow_bd").val();
    CowDataSaveUpdate(Bolus_ID, Cow_bd, Age_Lactation, lactationStage, Calving_Due_Date, Actual_Calving_Date);
    //---------------------------------------------------------------------------------------------------------------
    // hide popup edit form
    $("#popup").dxPopup("hide");
}
function CowDataSaveUpdate(bolus_id, Date_of_Birth, Age_Lactation, Current_Stage_Of_Lactation, Calving_Due_Date, Actual_Calving_Date) {
    //-------------------------------------------
    var url = "CowPage.aspx/CowDataSaveUpdate";
    var Param = {};
    Param.SP = "CowDataSaveUpdate";
    Param.bolus_id = bolus_id;
    Param.Date_of_Birth = Date_of_Birth;
    Param.Age_Lactation = Age_Lactation;
    Param.Current_Stage_Of_Lactation = Current_Stage_Of_Lactation;
    Param.Calving_Due_Date = Calving_Due_Date;
    Param.Actual_Calving_Date = Actual_Calving_Date;

    myAjaxRequestJsonE(url, Param, Success_CowDataSaveUpdate, Error_CowDataSaveUpdate);
}
function Success_CowDataSaveUpdate(result) {
    if (result.d == "Updated Successfully!") {
        DevExpress.ui.notify(result.d, "success", 1000);
    }
    else {
        DevExpress.ui.notify(result.d, "error", 1000);
    }
    return;
}
function Error_CowDataSaveUpdate(xhr, status, error) {
    DevExpress.ui.notify(error, "error", 1000);
    return false;
}
// END   Cow Data Save----------------------------------------------------------------------------------


//----Cows Logs Table Section-------------------------------------------------------------------- 
var titlecolor = "#dcefdc";
var tableborders = '';
var admincowslogs = "<div id='admincowslogs' " + tableborders + ">" +
    "<div class='demo-container'><div id ='gridcowlogs'></div ><div id='action-add'></div><div id='action-remove'></div><div id='action-edit'></div></div>";

function CowsLogShow(bid) {
    $("#PanelSWhow").html(admincowslogs);
    CowsLogFun(bid);
}
function CowsLogFun(bid) {
    var selectedRowIndex = -1;

    //----------------------------------------
    var cowsList = new DevExpress.data.CustomStore({
        loadMode: "raw",
        cacheRawData: true,
        key: "id",
        load: function (loadOptions) {
            return $.getJSON('CowPage.aspx?SP=GetCowsLogs&Bolus_id=' + bid);
        },
        insert: function (values) {
            var deferred = $.Deferred();

            var aid = $("#Animal_id").dxSelectBox("instance").option('displayValue');

            $.post("CowPage.aspx?SP=InsertCowsLogs&animal_id=" + aid, values).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        },
        update: function (key, values) {
            var deferred = $.Deferred();
            $.post("CowPage.aspx?SP=UpdateCowsLogs&id=" + encodeURIComponent(key), values).done(function (data) {
                deferred.resolve(data.key);
            });
            return deferred.promise();
        },
        remove: function (key) {
            var deferred = $.Deferred();
            $.post("CowPage.aspx?SP=RemoveCowsLogs&id=" + encodeURIComponent(key)).done(function (data) {
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
            return $.getJSON('CowPage.aspx?SP=GetAnimalList');
        }
    });

    var grid = $("#gridcowlogs").dxDataGrid({
        dataSource: cowsList,
        showBorders: true,
        keyExpr: "id",
        selection: {
            mode: "single"
        },
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
        export: {
            enabled: true,
            fileName: "CowLogs",
            allowExportSelectedData: true
        },

        editing: {
            mode: "popup",
            popup: {
                title: "Event Info",
                showTitle: true,
                width: 800,
                height: 450,
                position: {
                    my: "center",
                    at: "center",
                    of: window
                }
            },
            texts: {
                confirmDeleteMessage: "Are you sure?"
            },
            form: {
                items: [{
                    itemType: "group",
                    items: [
                        {
                            dataField: "Event_Date",
                            editorOptions: {
                                width: 200
                            }
                        },
                        {
                            dataField: "Event"
                        },
                        {
                            dataField: "Description",
                            editorType: "dxTextArea",
                            editorOptions: {
                                height: 200,
                                width: 646
                            }
                        }
                    ]
                    // or simply
                    // items: ["Prefix", "Full_Name", "Position"]
                }]
            }
        },

        onToolbarPreparing: function (e) {
            var dataGrid = e.component;

            e.toolbarOptions.items.unshift({
                location: "before",
                template: function () {
                    var mytemp = $("<div/>").addClass("informer").append(
                        $("<span />").addClass("name").text("Events List,           actions: "),
                        $("<div>").dxButton({
                            cssClass: "dx-button-success",
                            icon: "edit",
                            hint: "Edit",
                            type: "success",
                            onClick: function () {
                                grid.editRow(selectedRowIndex);
                                grid.deselectAll();
                            }
                        }),
                        $("<span />").addClass("name").text(" "),
                        $("<div>").dxButton({
                            icon: "trash",
                            hint: "Delete",
                            type: "success",
                            onClick: function () {
                                grid.deleteRow(selectedRowIndex);
                                grid.deselectAll();
                            }
                        }),
                        $("<span />").addClass("name").text(" "),
                        $("<div>").dxButton({
                            icon: "plus",
                            hint: "Add",
                            type: "success",
                            onClick: function () {
                                grid.addRow();
                                grid.deselectAll();
                            }
                        })
                    );

                    return mytemp;
                }
            });
        },
        onSelectionChanged: function (e) {
            selectedRowIndex = e.component.getRowIndexByKey(e.selectedRowKeys[0]);

            deleteSDA.option("visible", selectedRowIndex !== -1);
            editSDA.option("visible", selectedRowIndex !== -1);
        },
        columns: [
            {
                dataField: "Event_Date",
                dataType: "date",
                width: 125
            },
            {
                dataField: "Event",
                width: 125
            },
            {
                dataField: "Description",
            },

        ]
    }).dxDataGrid("instance");
}
//--End of  Cows Logs Table Section--------------------------------------------------------------------

//----Farmer's Feedback Table Section--------------------------------------------------------------------
function FarmerFeedbackTable(bolus_id) {
    $("#gridFermerFB").dxDataGrid({
        showBorders: true,
        showRowLines: true,
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
        dataSource: new DevExpress.data.CustomStore({
            loadMode: "raw",
            cacheRawData: true,
            load: function () {
                return $.getJSON("CowPage.aspx?SP=FermerFeedback&bolus_id=" + bolus_id);
            }
        }),
        columns: [
            {
                caption: "Visual Symptoms",
                dataField: "visual_symptoms",
                width: 140
            }, {
                caption: "Temperature,℃(rect.)",
                dataField: "rectal_temperature",
                dataType: "number",
                width: 180,
                alignment: "center"
            }, {
                caption: "Date",
                dataField: "rectal_temperature_measuring_time",
                dataType: "datetime",
                width: 150
            }, {
                dataField: "diagnosis",
                caption: "Diagnosis",
                cellTemplate: function (element, info) {
                    $("<div>")
                        .appendTo(element)
                        .text(info.value)
                        .css("width", info.column.width - 20)
                        .css("height", 100)
                        .css("white-space", "normal")
                        .css("overflow-wrap", 'break-word');
                }
            }, {
                dataField: "treatment_note",
                caption: "Treatment Note",
                cellTemplate: function (element, info) {
                    $("<div>")
                        .appendTo(element)
                        .text(info.value)
                        .css("width", info.column.width - 20)
                        .css("height", 100)
                        .css("white-space", "normal")
                        .css("overflow-wrap", 'break-word');
                }
            }, {
                dataField: "general_note",
                caption: "General Note",
                cellTemplate: function (element, info) {
                    $("<div>")
                        .appendTo(element)
                        .text(info.value)
                        .css("width", info.column.width - 20)
                        .css("height", 100)
                        .css("white-space", "normal")
                        .css("overflow-wrap", 'break-word');
                }
            }
        ]
    })
}
//----Farmer's Feedback Table Section--------------------------------------------------------------------