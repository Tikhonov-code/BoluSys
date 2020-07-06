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
function ConvertDateToMyF(dt) {
    var month, day, year, hours, minutes, seconds;
    if (typeof dt === 'string' || dt instanceof String) {
        return dt;
    }
    if (dt == "") {
        return null;
    }

    m = ("0" + (dt.getMonth() + 1)).slice(-2);
    d = ("0" + dt.getDate()).slice(-2);
    h = ("0" + dt.getHours()).slice(-2);
    min = ("0" + dt.getMinutes()).slice(-2);
    sec = ("0" + dt.getSeconds()).slice(-2);

    var mySQLDate = [dt.getFullYear(), m, d].join("-");
    var mySQLTime = [h, min, sec].join(":");
    var t = [mySQLDate, mySQLTime].join(" ");
    return t;
}