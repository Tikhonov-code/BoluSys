<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard1.aspx.cs" Inherits="BoluSys.Farm.Dashboard1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-xl">
        <%--<div class="row" style="text-align: center; background-color: #f2f2f2;">
            <div id="AutoRefresh"></div>
        </div>--%>
        <!----------------Top Line------------------------------------------------>
        <div class="well" style="text-align: center; background-color: #d9d9d9; border: solid; border-color: blue;">
            <div class="row">
                <div class="col-lg-6">
                    <mark style="font-size: large; font-weight: bold; background-color: #d9d9d9;">Total Number Of Cows: <%= TotalCowsNumberInfo %></mark>
                </div>
                <div class="col-lg-6">
                    <mark style="font-size: large; font-weight: bold; background-color: #d9d9d9;">Cows Under Monitoring: <%=  HealthyCowsNumber %></mark>
                </div>
            </div>
        </div>
        <!--------------Dashboard Section------------------------------------------------------------->
        <div class="row">
            <!--------------The Circular Gauge At Risk------------------------------------------------------------->
            <div class="col-lg-3" style="background-color: #f2f2f2;">
                <div id="gaugeAtRisk"></div>
            </div>
            <div class="col-lg-9">
                <div id="gridRisk"></div>
            </div>
        </div>
        <div class="row">
            <!--------------The Circular Gauge To Check------------------------------------------------------------->
            <div class="col-lg-3" style="background-color: #d9d9d9;">
                <div id="gaugeToCheck"></div>
            </div>
            <!--------------The DataGrid Section------------------------------------------------------------->
            <div class="col-lg-9">
                <div id="gridCheck"></div>
            </div>
        </div>
        <div class="row">
            <!--------------The DataGrid Section Data Integrity------------------------------------------------------------>
            <%--        Pivot grid--%>
            <div class="well well-sm" style="text-align: center;">
                <h4>Percent of missing data (When the Percent of missing data is greater than 5%, the water intake is not representative)</h4>
            </div>
            <div class="demo-container">
                <div id="gdi" style="align-content: center;"></div>
            </div>
        </div>

<!--------------The DataGrid Section Lost Cows------------------------------------------------------------>
            <%--        Pivot grid--%>
       <%-- <div class="row">
            
            <div class="well well-sm" style="text-align: center;">
                <h4>List Of Lost Cows</h4>
            </div>
            <div class="demo-container">
                <div id="lostCows_grid" style="align-content: center;"></div>
            </div>
        </div>--%>


        <%--    Data section--%>
        <div style="display: none;">
            <input id="TotalCowsNumberInfo" type="number" value="<%= TotalCowsNumberInfo %>" />
            <input id="CowsToCheckNumber" type="number" value="<%= CowsToCheckNumber %>" />
            <input id="CowsAtRiskNumber" type="number" value="<%= CowsAtRiskNumber %>" />
        </div>
    </div>
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="FarmJS/Dashboard1.js"></script>
    <style type="text/css">
        .dx-datagrid-headers .cls {
            background-color: #f2f2f2;
            font-weight: bold;
            color: black;
        }

        .dx-row-total .dx-grandtotal .cls {
            visibility: hidden
        }
    </style>
    <!--Script Section-->
</asp:Content>
