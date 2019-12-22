<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BoluSys.Farm.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Date:<input id="DateSearch" type="date" onchange="ChartsShowAllRequest(this.value);" />
    &nbsp&nbsp&nbsp<a id="ProgressBar" name="ProgressBar" style="visibility: hidden;"><progress></progress>&nbsp&nbspData Loading...</a>

    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <style>
        .w3-btn {
            width: 70px;
            background-color: #e6ff99;
        }

        #gauge, #gauge1, #gauge2 {
            height: 210px;
            width: 32%;
        }

        #gauge, #gauge1, #gauge2 {
            display: inline-block;
            background-color: #f2f2f2;
        }
    </style>
    <!--Chart Section-->
    <table>
        <tr>
            <td style="vertical-align: top;">Animal_ID<br />
                <div id="bolus_list" class="btn-group-vertical table-bordered">
                </div>
            </td>
            <td>
                <!----------------Dashboard Indicators------------------------------------------------>
                <div class="well" style="text-align: center;">
                    <mark style="font-size: large; font-weight: bold; background-color: #e6ff99;">Total Number of Cows: <%= TotalCowsNumberInfo %></mark>
                </div>

                <div id="gauge"></div>
                <div id="gauge1"></div>
                <div id="gauge2"></div>
                <div id="curve_chart" style="width: 1000px; height: 500px"></div>
            </td>
            <td style="vertical-align: top;">
                <div id="Description" style="background-color: #e6e6e6;"></div>
                <!----------------Dashboard Indicators------------------------------------------------>
            </td>
        </tr>
    </table>
    <%--<div style="display: none;">
        <input id="TotalCowsNumberInfo" type="number" value="<%= TotalCowsNumberInfo %>" />
        <input id="CowsToCheck" type="number" value="<%= CowsToCheck %>" />
        <input id="CowsAtRisk" type="number" value="<%= CowsAtRisk %>" />
        <input id="CowsUnderMonitoring" type="number" value="<%= CowsUnderMonitoring %>" />
    </div>--%>
    <!--Script Section-->
    <link href="../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../dx/js/dx.all.js"></script>
    <script src="FarmJS/ChartsXY.js"></script>
    <!--Script Section-->
</asp:Content>
