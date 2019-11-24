// Logs list Section---------Begin-------------------------------
    GetCowsLogs(831);

function ShowLogList(cl) {
    var listWidget = $("#CowsLogList").dxList({
        dataSource: cl,
        height: 200,
        allowItemDeleting: false,
        itemDeleteMode: "toggle",
    }).dxList("instance");
};

function GetCowsLogs(aid_par) {
    //-------------------------------------------
    var url = 'ht.aspx?SP=GetCowsLogs&Animal_id=' + aid_par;
    var Param = {};
    Param.SP = "GetCowsLogs";
    Param.Animal_id = aid_par;
    myAjaxRequestJsonE(url, Param, Success_GetCowsLogs, Error_GetCowsLogs);
    //----------------------------------------------------
}
function Success_GetCowsLogs(result) {
    ShowLogList(result);
    return ;
}
function Error_GetCowsLogs(xhr, status, error) {
    return false;
}
// Logs list Section----------End---------------------------

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