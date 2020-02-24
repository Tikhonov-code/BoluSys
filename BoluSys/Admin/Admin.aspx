<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BoluSys.Admin.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--Menu section------------------------------------------------------------------->
    <div class="row" style="text-align: center; padding: 10px 0; border-width: thin; background-color: #b3d1ff;">
        <h3>Administrator Panel</h3>
        <button type="button" class="btn btn-primary" onclick="AlertsListShow();">Alerts List</button>
        <button type="button" class="btn btn-primary" onclick="DataGapsShow();">Data Gaps</button>
        <button type="button" class="btn btn-primary" onclick="CowsLogShow();">Cows Log</button>
        <button type="button" class="btn btn-primary" onclick="WIReportShow();">Water Intakes Report</button>
        <button type="button" class="btn btn-primary" onclick="TTNRawConverter();">TTN Raw Converter</button>
        <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/admincowsdata.aspx';">Cows Data</button>
        <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/DownloadData.aspx';">Download Data</button>
<%--        swagger   https://pilot001.data.thethingsnetwork.org/
    https://www.thethingsindustries.com/
    --%>
    </div>

    <!------------------------------------------------------------->
    <div id="PanelSWhow"></div>
    <!------------------------------------------------------------->
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="AdminJS/Adminjs.js"></script>
    <script src="../Farm/FarmJS/FarmGeneral.js"></script>
    <link href="../Services/MyCSS/AdminCowsLogs.css" rel="stylesheet" />
    <style type="text/css">
        .full-width-content {
            width: 100%;
            margin-top: 30px;
        }

            .full-width-content > .dx-widget {
                margin-bottom: 20px;
            }

            .full-width-content .dx-field {
                max-width: 385px;
            }
    </style>

    <!--Script Section-->
</asp:Content>
