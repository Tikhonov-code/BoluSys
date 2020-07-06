<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BoluSys.Admin.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--Menu section------------------------------------------------------------------->
    <div class="row" style="text-align: center; padding: 10px 0; border-width: thin; background-color: #b3d1ff;">
        <h3>Administrator Panel</h3>
        <%--        <button type="button" class="btn btn-primary" onclick="AlertsListShow();">Alerts List</button>--%>
        <div class="dropdown">
            <button class="auto-style1">Alert Service</button>
            <div class="dropdown-content">
                <button type="button" class="btn btn-primary" onclick="AlertsListShow();">Emails List</button>
                <button type="button" class="btn btn-primary" onclick="SMSListShow();">SMS List</button>
                <%--<a href="#">Intervals in min</a>
                <a href="#">Gaps % for Herd</a>--%>
            </div>
        </div>
        <%--        <button type="button" class="btn btn-primary" onclick="DataGapsShow();">Data Gaps</button>--%>
        <div class="dropdown">
            <button class="auto-style1">Data Gaps Analysis</button>
            <div class="dropdown-content">
                <button type="button" class="btn btn-primary" onclick="DataGapsShow();">Intervals in min</button>
                <button type="button" class="btn btn-primary" onclick="GapsByFarmHerdShow();">Gaps in % for Herd</button>
                <button type="button" class="btn btn-primary" onclick="GapsMapShow();">Gaps Map for Herd</button>
                <%--<a href="#">Intervals in min</a>
                <a href="#">Gaps % for Herd</a>--%>
            </div>
        </div>
        <button type="button" class="btn btn-primary" onclick="CowsLogShow();">Cows Log</button>
        <button type="button" class="btn btn-primary" onclick="WIReportShow();">Water Intakes Report</button>
        <button type="button" class="btn btn-primary" onclick="BolusesSetShow();">Settings</button>
        <%--        <button type="button" class="btn btn-primary" onclick="TTNRawConverter();">TTN Raw Converter</button>--%>
        <%--        <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/admincowsdata.aspx';">Cows Data</button>--%>
        <button type="button" class="btn btn-primary" onclick="UserForm();">Users</button>
        <%--        <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/DownloadData.aspx';">Download Data</button>--%>
        <div class="dropdown">
            <button class="auto-style1">Data</button>
            <div class="dropdown-content">
                <button type="button" class="btn btn-primary" onclick="TempIntakesData();">Temperature + Intakes</button>
                <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/DownloadData.aspx';">Download</button>
                <button type="button" class="btn btn-primary" onclick="TTNRawConverter();">TTN Raw Converter</button>
                <button type="button" class="btn btn-primary" onclick="window.location.href = '../services/admincowsdata.aspx';">Cows Data</button>

            </div>
        </div>
        <%--        swagger   https://pilot001.data.thethingsnetwork.org/
    https://www.thethingsindustries.com/
        --%>
    </div>
    <!------------------------------------------------------------------------------------>
    <!--Menu section------END------------------------------------------------------------->
    <!------------------------------------------------------------->
    <div id="PanelSWhow"></div>
    <!------------------------------------------------------------->

    <div class="demo-container">
        <div id="form-demo">
            <div class="widget-container">
                <div id="select_farm"></div>
                <div id="form"></div>
            </div>

        </div>
    </div>

    <!--Script Section-->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.5/jszip.min.js"></script>
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <!-------------------------------------------->
<%--    <script type="text/javascript" src="https://cdn3.devexpress.com/jslib/20.1.4/js/dx.all.js"></script>--%>
    <!-------------------------------------------->
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
            padding: 6px 12px;
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
            background-color: #b3d1ff; /*#337ab7;*/
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

                .dropdown-content button:hover {
                    background-color: #2d6a9f
                }

        .dropdown:hover .dropdown-content {
            display: block;
        }

        .dropdown:hover .dropbtn {
            background-color: #2d6a9f;
        }

        .auto-style1 {
            display: inline-block;
            padding: 6px 12px;
            margin-bottom: 0;
            font-size: 14px;
            font-weight: normal;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            touch-action: manipulation;
            cursor: pointer;
            user-select: none;
            color: white;
            background-color: #337ab7;
            border: 1px solid transparent;
            border-radius: 4px;
        }
    </style>
    <!------------------------------------------------------>
    <style type="text/css">
        #form-container {
            margin: 10px;
        }

        .long-title h3 {
            font-family: 'Segoe UI Light', 'Helvetica Neue Light', 'Segoe UI', 'Helvetica Neue', 'Trebuchet MS', Verdana;
            font-weight: 200;
            font-size: 28px;
            text-align: center;
            margin-bottom: 20px;
        }
    </style>
    <style type="text/css">
        .dx-switch-off {padding-left: 2px;
    color: red;
}
        .dx-switch-on {padding-left: 2px;
    color:darkgreen;
}
        
    </style>
    <!-- Script Section-->
</asp:Content>
