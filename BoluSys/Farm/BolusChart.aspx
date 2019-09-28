<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BolusChart.aspx.cs" Inherits="BoluSys.Farm.BolusChart" %>

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
            <td>
                
            </td>
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
    <input id="Bolus_id" type="text" value="<%# Bolus_id %>" />
    <input id="Animal_id" type="text" value="<%# Animal_id %>" />DateSearch
    <input id="DateSearch" type="text" value="<%# DateSearch %>" />
    </div>
    <!---------------------------------------------------------------->
    <div class="demo-container">
        <div id="chart"></div>
    </div>

    <!--Script Section-->
    <link href="../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../dx/js/dx.all.js"></script>
    <script src="FarmJS/FarmGeneral.js"></script>
    <script src="FarmJS/BolusChart.js"></script>
    <style>
        #chart {
            height: 440px;
        }
    </style>
</asp:Content>
