$(function () {
    var now = new Date();

    $("#DateFrom").dxDateBox({
        type: "datetime",
        name: "Date From:",
        value: now,
        width:180
    });
});
$(function () {
    var now = new Date();

    $("#DateTo").dxDateBox({
        type: "datetime",
        value: now,
        width: 180
    });
});
// ----------------Bolus ID select Section--------------------------------------- 
$(function() {
    var ds = new DevExpress.data.CustomStore({
        loadMode: "raw",
        load: function () {
            return $.getJSON("IntakesData.aspx?SP=GetBolusList");
        }
    });
    $("#BolusIDList").dxSelectBox({
        dataSource: ds,
        placeholder: "Choose Animal_id",
        showClearButton: true,
        displayExpr: "animal_id",
        valueExpr: "bolus_id",
        //value: ds[0].bolus_id,
        //------------------------------------------------------
        onValueChanged: function (e) {
            var bid = e.value;
            DevExpress.ui.notify('Selected =' + bid);

        }
    });
});

//------------Button Section------------------------------------
var IntakesRequest;
$(function () {
    $("#CheckIntakes").dxButton({
        stylingMode: "contained",
        text: "Check Intakes",
        type: "success",
        width: 150,
        onClick: function () {
            //DevExpress.ui.notify("The Contained button was clicked");
            var dt0 = ConvertDateToMyF($("#DateFrom").dxDateBox("instance").option("value"));
            var dt1 = ConvertDateToMyF($("#DateTo").dxDateBox("instance").option("value"));
            var bid = $("#BolusIDList").dxSelectBox("instance").option("value");
            var urlreq = "IntakesData.aspx?SP=CheckIntakes&dt0=" + dt0 + "&dt1=" + dt1 + "&bid=" + bid;
            IntakesRequest = new DevExpress.data.CustomStore({
                loadMode: "raw",
                load: function () {
                    //var dt0 = ConvertDateToMyF($("#DateFrom").dxDateBox("instance").option("value"));
                    //var dt1 = ConvertDateToMyF($("#DateTo").dxDateBox("instance").option("value"));
                    //var bid = $("#BolusIDList").dxSelectBox("instance").option("value");
                    //return $.getJSON('IntakesData.aspx?SP=CheckIntakes&dt0=' + dt0 + '&dt1=' + dt1+'&bid='+bid);
                    return $.getJSON(urlreq);
                }
            });
            var x = IntakesRequest;
        }
    });
});