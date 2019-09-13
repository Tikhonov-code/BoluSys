function myAjaxRequestJson(URL, Param, Success_function_name) {
    //var obj = {};
    //obj.DateSearch = Param;
    $.ajax({
        type: "POST",
        url: URL,
        data: JSON.stringify(Param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: Success_function_name
    });
    return false;
}
function GetToday() {
    var month, day, year, hours, minutes, seconds;
    var dt = new Date();
    m = ("0" + (dt.getMonth() + 1)).slice(-2);
    d = ("0" + dt.getDate()).slice(-2);
    h = ("0" + dt.getHours()).slice(-2);
    min = ("0" + dt.getMinutes()).slice(-2);
    sec = ("0" + dt.getSeconds()).slice(-2);

    var mySQLDate = [dt.getFullYear(), m, d].join("-");
    //var mySQLTime = [h, min, sec].join(":");
    var t = mySQLDate
    $("#DateSearch").val(t);
    return t;
}

//0. Clean description div
$("#Description").text('');
var tdy = GetToday();
//1. request DateSearch BolusList
NewBolusList(tdy);
$("#DateSearch").val(tdy);
//2. Charts Creating for all boluses
google.charts.load('current', { 'packages': ['corechart'] });
ChartsShowAllRequest();

// Request data for All Boluses from database
function ChartsShowAllRequest() {
    //0. Clean description div
    $("#Description").text('');
    var DateSearch = $("#DateSearch").val();
    $("#ProgressBar").attr("style", "visibility: visible");
    NewBolusList(DateSearch);
    var Param = {};
    Param.DateSearch = DateSearch;
    myAjaxRequestJson('ChartsXY.aspx/GetDataAll', Param, ChartsShowAllRequestSuc);
}
function ChartsShowAllRequestSuc(result) {
    $("#ProgressBar").attr("style", "visibility: hidden");
    //alert(result);
    ChartsShowAll(result.d);
}

function ChartsShowAll(result) {
    // Load the Visualization API and the piechart package.
    google.charts.load('current', { 'packages': ['corechart'] });
    // Set a callback to run when the Google Visualization API is loaded.
    google.charts.setOnLoadCallback(CreateChartForAll(result));
}
//// Function Show all charts
function CreateChartForAll(result) {
    //alert(result);
    var data1 = new google.visualization.arrayToDataTable(result);

    //Charts Decoration
    var options = {
        interpolateNulls: true,
        title: '',
        titlePosition: 'center',
        curveType: 'function',
        legend: { position: 'right' },
        vAxis: {
            gridlines: { count: 8 },
        },
        vAxis: {
            //ticks: [0, 10, 20, 29.5, 39.5, 42],
            gridlines: { color: "#000000" }
        },
        crosshair: { trigger: 'both' }
    };

    var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));
    chart.draw(data1, options);
}
//-------------------------------------------------------
function NewBolusList(dt) {
    $("#ProgressBar").attr("style", "visibility: visible");
    var Param = {};
    Param.DateSearch = dt;
    myAjaxRequestJson("ChartsXY.aspx/GetDayBolusList", Param, BolusList);
    return false;
}
function BolusList(result) {
    //alert(result.d);
    var resultJson = JSON.parse(result.d);
    if (resultJson.length == 0) {
        $("#bolus_list").text('No Data');
    }
    else {
        var newInnerText = "<button type='button' class='btn btn - light' onclick='ChartsShowAllRequest();'>All</button>";
        var i;
        for (i = 0; i < resultJson.length; i++) {
            newInnerText += "<button id='" + resultJson[i].bolus_id + "' type='button' class='btn btn - light' onclick='ShowChartByDateBolus_ID("
                + resultJson[i].bolus_id + "," + resultJson[i].animal_id + ");'>"
                + resultJson[i].animal_id + "</button>";
        }
        $("#bolus_list").html(newInnerText);
    }
    $("#ProgressBar").attr("style", "visibility: hidden");
    return;
}

//---------------------------------------------------------------------------------------
//Bolus_ID Chart-------------------------------------------------------------------------
var Bolus_ID_Current;
var Animal_ID_Current;
function ShowChartByDateBolus_ID(bolus_id, animal_id) {
    $("#ProgressBar").attr("style", "visibility: visible");
    var Param = {};
    Param.DateSearch = $("#DateSearch").val();
    Param.Bolus_id = bolus_id;

    Bolus_ID_Current = bolus_id;
    Animal_ID_Current = animal_id;

    myAjaxRequestJson('ChartsXY.aspx/GetData', Param, ShowChartByDateBolus_ID_Suc);
}
function ShowChartByDateBolus_ID_Suc(result) {
    $("#ProgressBar").attr("style", "visibility: hidden");
    if (result.length == 0) {
        alert('No Data');
        $("#curve_chart").empty();
        return;
    }
    //alert(result.d);
    ChartsShow(result.d, Bolus_ID_Current, Animal_ID_Current);
}
function ChartsShow(result, pbolusID, animalid) {

    // Load the Visualization API and the piechart package.
    google.charts.load('current', { 'packages': ['corechart'] });
    // Set a callback to run when the Google Visualization API is loaded.
    google.charts.setOnLoadCallback(drawChart(result, pbolusID, animalid));
}
// Callback that creates and populates a data table,
// instantiates the hart, passes in the data and
// draws it.
function drawChart(result, pbolusID, animalid) {
    var data = new google.visualization.DataTable();
    //console.log('53  bolisid=' + pbolusID + '  str()=' + pbolusID.toString());
    data.addColumn('datetime', 'Time');
    //data.addColumn('number', '#' + pbolusID.toString());
    data.addColumn('number', '#' + animalid.toString());
    //--------------------------------------
    data.addColumn({ id: 'Tmin', type: 'number', role: 'interval' });
    data.addColumn({ id: 'Tmax', type: 'number', role: 'interval' });

    //-----------------------------------------------
    var dtsh = $("#DateSearch").val();
    var m_year = dtsh.substr(0, 4);//ViewBag.Ch_Year;
    var m_month = dtsh.substr(5, 2) - 1;
    var m_day = dtsh.substr(8, 2);;
    for (var i in result) {
        data.addRows([
            [
                new Date(m_year, m_month, m_day, result[i].dhours, result[i].dminutes, result[i].dseconds, 0),
                result[i].temperature,
                result[i].tmin,
                result[i].tmax
            ]]);
    }
    var options = {
        legend: { position: 'right' },
        title: '',
        titlePosition: 'left',
        curveType: 'function',
        intervals: { 'style': 'area', 'color': 'green' },
        crosshair: { 'trigger': 'both' },
        vAxis: { gridlines: { count: 8 } }
    };
    
    var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));
   
    chart.draw(data, options);
   // alert(pbolusID);
    GetIndividualDescription(pbolusID);
}

//------------------------Description for Individual Chart-----------------------------------------
function GetIndividualDescription(bolus_id) {
    var Param = {};
    Param.Bolus_id = bolus_id;
    myAjaxRequestJson('ChartsXY.aspx/GetIndividualDescription', Param, GetIndividualDescription_Suc);
}
function GetIndividualDescription_Suc(result) {
    $("#Description").text(result.d);
}
