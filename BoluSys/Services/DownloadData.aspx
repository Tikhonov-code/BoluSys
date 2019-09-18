<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadData.aspx.cs" Inherits="BoluSys.Services.DownloadData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DownLoad Data</title>
    <script src="../Scripts/jquery-3.3.1.min.js"></script>
    <script type="text/javascript">
        function CompareDate() {
            var d1 = $("#DtFrom").val();
            var d2 = $("#DtTo").val();
            if (d2 >= d1) {
                $("#btn_Download").removeAttr("disabled");
                $("#MessErr").text("");
            }
            else {
                $("#btn_Download").attr("disabled", "disabled");
                $("#MessErr").text("*** Incorrect Dates!!!");
            }
        }
    </script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <div class="jumbotron">
            <h3>Data Downloading</h3>
            <p>
                Please, set Dates - From and To.<br />
            Click Button "Download Data In csv File".<br />
            File temperature.csv will be download on your computer in folder Downloads <br />
            and opened in Excell.
            </p>
        </div>
    
    <form id="form1" runat="server">
        <div>
            Date From: 
            <input id="DtFrom" type="date" onchange="CompareDate();" value="<%# DtFrom%>" runat="server" />&nbsp; To:&nbsp;
            <input id="DtTo" type="date" onchange="CompareDate();" value="<%# DtTo%>" runat="server" />
            <asp:Button ID="btn_Download" runat="server" Text="Download Data In csv File" OnClick="btn_Download_Click" Enabled="False" />
            <div id="MessErr" style="color: orangered; font-weight: bold;"></div>
        </div>
    </form>

    </div>
</body>
</html>
