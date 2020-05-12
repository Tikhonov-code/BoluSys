<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CowPage.aspx.cs" Inherits="BoluSys.Farm.CowPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="demo-container">

        <input type="hidden" runat="server" id="bid_ext" value="<%# bid_ext %>" />

        <div id="form" style="align-content:center;"></div>
        <div id="chart_temp"></div>
        <div id="IntakesChart"></div>
    </div>
    <div class="row">
        <!--------------The DataGrid Section Data Integrity------------------------------------------------------------>
        <%--        Pivot grid--%>
        <div class="well well-sm" style="text-align: center; padding: 20px 0; border-width: thin;">
            <h4>Percent of missing data (When the Percent of missing data is greater than 5%, the water intake is not representative)</h4>
            <h4 id="DataGapsValue" style="align-content: center;"></h4>
        </div>
        <div class="demo-container">
            <div id="DataGapsIndividual" style="align-content: center;"></div>
        </div>
    </div>
    <!---------------------------------------------------------------->
<%--    <div class="jumbotron" id="CowInfoDiv" style="visibility:hidden;">--%>
       <%-- Cow #:<input id="Animal_id" type="text" value="<%# Animal_id %>" style="border: hidden;" aria-disabled="True" disabled="disabled" />
        Bolus:<input id="Bolus_id" type="text" value="<%# Bolus_id %>" style="border: hidden;" disabled="disabled" /><br />--%>
<%--        <div id="CowInfo"><%# CowInfo %></div>--%>

<%--        <div id="CowsLogList" style="border-style: solid; border-color: #999999;"></div>--%>
<%--    </div>--%>
    <!------------------------------------------------->
    <!--Personal Data Section------------------------------------------------------>
    <div class="CowInfo-container">
        <div id="CowInfo_form"></div>
        <div id="popup">
            
        </div>
    </div>
    <!--END of Personal Data Section----------------------------------------------->
    <!------------------------------------------------------------->
    <div id="PanelSWhow"></div>
    <!------------------------------------------------------------->
    <style type="text/css">
        .first-group {
            padding: 20px;
            background-color: #dcefdc;
        }

    </style>
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="FarmJS/CowPage.js"></script>
    <script src="FarmJS/FarmGeneral.js"></script>
    <!--Script Section-->
</asp:Content>
