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
//NewBolusList(tdy);
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

    var sss = NewBolusList(DateSearch);
    if (!sss) {
        return;
    }
    else {
        var Param = {};
        Param.DateSearch = DateSearch;
        //myAjaxRequestJson('ChartsXY.aspx/GetDataAll', Param, ChartsShowAllRequestSuc);
        myAjaxRequestJsonE('ChartsXY.aspx/GetDataAll', Param, ChartsShowAllRequestSuc, ChartsShowAllRequestErr);
    }
}
function ChartsShowAllRequestErr(xhr) {
    var x = xhr;
    return;
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
    return true;
}
function BolusList(result) {
    //alert(result.d);
    var resultJson = JSON.parse(result.d);
    if (result.d == null) {
        //$("#bolus_list").text('No Data'); 
        $("#curve_chart").html('<h2>No Data</h2>');
        $("#ProgressBar").attr("style", "visibility: hidden");
        return false;
    }
    else {
        //var newInnerText = "<button type='button' class='btn btn - light' onclick='ChartsShowAllRequest();'>All</button>";
        var newInnerText = "<div class='w3-container'><a href='#' class='w3-btn w3-border w3-hover-light-grey' onclick='ChartsShowAllRequest();'>All </a></div>";
        var i;
        var dayPar = '&DateSearch=' + $("#DateSearch").val() +'&SP=ShowChart';
        for (i = 0; i < resultJson.length; i++) {
            var urlch = 'BolusChart.aspx?Animal_id=';
            urlch += + resultJson[i].animal_id + "&Bolus_id="+ resultJson[i].bolus_id +dayPar;
            newInnerText += "<div class='w3-container'><a href=" + urlch + " class='w3-btn w3-border w3-hover-light-grey'>" + resultJson[i].animal_id + "</a></div>";

        }
        $("#bolus_list").html(newInnerText);
    }
    $("#ProgressBar").attr("style", "visibility: hidden");
    return true;
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
    return true;
}

//Palette for Ranges Section
$(function () {
    $("#gauge").dxCircularGauge({
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
            backgroundColor: "green"
        },
        title: {
            text: "Healthy Cows Under Monitoring",
            font: { size: 14,  weight: "bold"}
        },
        "export": {
            enabled: false
        },
        value: $("#CowsUnderMonitoring").val(),
        subvalues: [$("#CowsUnderMonitoring").val()],
        subvalueIndicator: {
            type: "triangleMarker",
            color: "red"
        },
        valueIndicator: {
            color: "green"
        },
        subvalueIndicator: {
            type: "textCloud",
            color: "green"
        }
        
    });
});
$(function () {
    $("#gauge1").dxCircularGauge({
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
            font: { size: 14,  weight: "bold" }
        },
        "export": {
            enabled: false
        },
        value: $("#CowsToCheck").val(),
        subvalues: [$("#CowsToCheck").val()],
        subvalueIndicator: {
            type: "triangleMarker",
            color: "red"
        },
        valueIndicator: {
            color: "#ffcc00"
        },
        subvalueIndicator: {
            type: "textCloud",
            color: "#ffcc00"
        }
    });
});
$(function () {
    $("#gauge2").dxCircularGauge({
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
            backgroundColor: "#ffb3b3"
        },
        title: {
            text: "Cows At Risk",
            font: { size: 14, weight: "bold" }
        },
        "export": {
            enabled: false
        },
        value: $("#CowsAtRisk").val(),
        subvalues: [$("#CowsAtRisk").val()],
        subvalueIndicator: {
            type: "triangleMarker",
            color: "red"
        }        ,
        valueIndicator: {
            color: "red"
        },
        subvalueIndicator: {
            type: "textCloud",
            color: "red"
        }

    });
});
