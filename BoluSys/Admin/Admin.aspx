<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BoluSys.Admin.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--Menu section------------------------------------------------------------------->
    <div class="row" style="text-align: center; padding: 10px 0; border-width: thin; background-color: #b3d1ff;">
        <h3>Administrator Panel</h3>
        <button type="button" class="btn btn-primary" onclick="AlertsListShow();">Alerts List</button>
<%--        <button type="button" class="btn btn-primary" onclick="DataGapsShow();">Data Gaps</button>--%>
        <div class="dropdown">
            <button class="auto-style1">Data Gaps Analysis</button>
            <div class="dropdown-content">
                <button type="button" class="btn btn-primary" onclick="DataGapsShow();">Intervals in min</button>
                <button type="button" class="btn btn-primary" onclick="GapsByFarmHerdShow();">Gaps in % for Herd</button>
                <%--<a href="#">Intervals in min</a>
                <a href="#">Gaps % for Herd</a>--%>
            </div>
        </div>
        <button type="button" class="btn btn-primary" onclick="CowsLogShow();">Cows Log</button>
        <button type="button" class="btn btn-primary" onclick="WIReportShow();">Water Intakes Report</button>
        <button type="button" class="btn btn-primary" onclick="BolusesSetShow();">Settings</button>
        <button type="button" class="btn btn-primary" onclick="TTNRawConverter();">TTN Raw Converter</button>
        <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/admincowsdata.aspx';">Cows Data</button>
        <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/DownloadData.aspx';">Download Data</button>
        
        <%--        swagger   https://pilot001.data.thethingsnetwork.org/
    https://www.thethingsindustries.com/
        --%>
    </div>
    <!------------------------------------------------------------------------------------>
    <!--Menu section------END------------------------------------------------------------->

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

        .dx-menu-base .dx-menu-items-container {
            width: 120px;
        }

        .dx-menu-base .dx-menu-item .dx-menu-item-content .dx-menu-item-text {
            background-color: #337ab7;
            border-color: #2e6da4;
            color: white;
        }

            .dx-menu-base .dx-menu-item .dx-menu-item-content .dx-menu-item-text:hover {
                background-color: #2d6a9f;
                border-color: #2e6da4;
                color: white;
            }
    </style>
    <!------------------------------------------------------>
    <style>
.dropbtn {
  background-color: #337ab7;
  color: white;
  padding:  6px 12px;;
  font-size: 14px;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.dropdown {
  position: relative;
  display: inline-block;
}

.dropdown-content {
  display: none;
  position: absolute;
  background-color: #b3d1ff;/*#337ab7;*/
  min-width: 160px;
  box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
  z-index: 1;
  border-radius: 4px;
}

.dropdown-content button {
   color: white;
  padding: 6px 12px;
  text-decoration: none;
  display: block;
  border-radius: 4px;
}

.dropdown-content button:hover {background-color: #2d6a9f}

.dropdown:hover .dropdown-content {
  display: block;
}

.dropdown:hover .dropbtn {
  background-color: #2d6a9f;
}
        .auto-style1 {
            border-style: none;
            border-color: inherit;
            border-width: medium;
            background-color: #337ab7;
            color: white;
            padding: 6px 12px;
;font-size: 14px;
            cursor: pointer;
            border-radius: 4px;
            height: 27px;
        }
    </style>    <!--Script Section-->
    
</asp:Content>
