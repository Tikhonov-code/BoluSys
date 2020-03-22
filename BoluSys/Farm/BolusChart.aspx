<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BolusChart.aspx.cs" Inherits="BoluSys.Farm.BolusChart_new" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td style="color: black; font-weight: bold;">Date From:</td>
            <td>
                <div id="dateFrom"></div>
            </td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
            <td style="color: black; font-weight: bold;">To:</td>
            <td>
                <div id="dateTo"></div>
            </td>
            <td></td>
            <td>
                <div class="dx-field">
                    <div class="dx-field-label">Animal ID:</div>
                    <div class="dx-field-value">
                        <div id="BolusIDList"></div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="display:none;">
        DateSearch
        <input id="DateSearch" type="text" value="<%# DateSearch %>" />
        <input id="Bolus_id_Ini" type="text" value="<%# Bolus_id_Ini %>" />
        <input id="Animal_id_Ini" type="text" value="<%# Animal_id_Ini %>" />        
        Total Water Intakes:<input id="TotalIntakes" style="width: 60px;" type="text" value="<%# TotalIntakes %>" />, Litres

    </div>
    <!---------------------------------------------------------------->
    <%--<div class="demo-container">
        <div id="chart"></div>
    </div>--%>
    <div class="demo-container">
        <div id="chart_temp"></div>
    </div>
    <div class="demo-container">
        <div id="IntakesChart"></div>
    </div>
    <div class="row">
        <!--------------The DataGrid Section Data Integrity------------------------------------------------------------>
        <%--        Pivot grid--%>
        <div class="well well-sm" style="text-align: center;">
            <h4>Percent of missing data (When the Percent of missing data is greater than 5%, the water intake is not representative)</h4>
            <h4 id="DataGapsValue" style="align-content: center;"></h4>
        </div>
        <div class="demo-container">
            <div id="DataGapsIndividual" style="align-content: center;"></div>
        </div>
    </div>
    <!---------------------------------------------------------------->
    <div class="jumbotron">
        Cow #:<input id="Animal_id" type="text" value="<%# Animal_id %>" style="border: hidden;" aria-disabled="True" disabled="disabled" />
        Bolus:<input id="Bolus_id" type="text" value="<%# Bolus_id %>" style="border: hidden;" disabled="disabled" /><br />
        <div id="CowInfo"><%# CowInfo %></div>

        <div id="CowsLogList" style="border-style: solid; border-color: #999999;"></div>
    </div>
    <!---------------------------------------------------------------->

    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="FarmJS/FarmGeneral.js"></script>
    <script src="FarmJS/BolusChart.js"></script>
    <script src='https://kit.fontawesome.com/a076d05399.js'></script>
    <style>
        #chart {
            height: 440px;
        }

        .auto-style1 {
            width: 840px;
            height: 222px;
        }
        /*   Logs list css       */
        .selected-data,
        .options {
            margin-top: 20px;
            padding: 20px;
            background: #0000cc;
        }

            .options .caption {
                font-size: 18px;
                font-weight: 500;
            }

        .option {
            margin-top: 10px;
        }

            .option > span {
                margin-right: 10px;
            }
    </style>
</asp:Content>

