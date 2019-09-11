<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BoluSys.Test.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <input id="Button1" type="button" value="Request" onclick="RequestA();" />
    <div id="bolus_list"></div>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/json2/20130526/json2.min.js"></script>
    <script type="text/javascript">
        function RequestA() {
            var obj = {};
            obj.DateSearch = "09/08/2019";
            //obj.age = "101";
            $.ajax({
                type: "POST",
                url: "WebForm1.aspx/SendParameters",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: BolusList
            });
            return false;
        }
        function BolusList(result) {
            alert(result.d);
            var newInnerText = "<button type='button' class='btn btn - light' onclick='ChartsShowAllRequest();'>All</button>";

            var resultJson = JSON.parse(result.d);
            var i;
            for (i = 0; i < resultJson.length; i++) {
                newInnerText += "<button id='" + resultJson[i].bolus_id + "' type='button' class='btn btn - light' onclick='ShowChartByDateBolus_ID("
                    + resultJson[i].bolus_id + "," + resultJson[i].animal_id + ");'>"
                    + resultJson[i].animal_id   + "</button>";
            }

            document.getElementById("bolus_list").innerHTML = newInnerText;
            return;
        }
        function ShowChartByDateBolus_ID() {
            alert('Hey');
        }
    </script>

</asp:Content>
