<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserCabinet.aspx.cs" Inherits="BoluSys.Farm.UserCabinet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="jumbotron">

            <table class="table">
                <tr>
                    <td style="text-align: left;">
                        <div class="dx-field">
                            <div class="dx-field-label" style="font-weight: bold;">Email</div>
                            <div class="dx-field-value">
                                <div id="email"></div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>Alerts</th>
                    <th>Regular Report</th>
                </tr>
                <tr>
                    <td>
                        <div style="padding: 10px;">
                            <div id="cbx_TempSignal"></div>
                        </div>
                        <div style="padding: 10px;">
                            <div id="cbx_IntakesSignal"></div>
                        </div>
                        <div style="padding: 10px;">
                            <div id="cbx_OutOfRangeSignal"></div>
                        </div>
                    </td>
                    <td>
                        <div style="padding: 10px;">
                            Animals List:<div id="Sct_AnimalList"></div>
                        </div>
                        <div style="padding: 10px;">
                            <div id="cbx_TempChart"></div>
                        </div>
                        <div style="padding: 10px;">
                            <div id="cbx_StatTab"></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="popup">
                            <ul id="Btn_ShowAnamaliesTemp"></ul>
                            <div class="popup"></div>
                        </div>
                    </td>
                    <td>
<%--                        <ul id="Btn_ShowReportTemp"></ul>--%>
                        <div id="popup_Rep">
                            <ul id="Btn_ShowReportTemp"></ul>
<%--                            <div class="popup"></div>--%>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <div id="Btn_SendAlerts"></div>
                        <div id="Btn_SendReport"></div>
                        <div id="Btn_SaveSettings"></div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none;">
            <input id="AlertEmail" type="text" value="<%= AlertEmail%>" />
            Temperature<input id="AlertTemperatureInd" value="<%= AlertTemperatureInd%>" />
            Water<input id="AlertWaterIntakesInd" value="<%= AlertWaterIntakesInd%>" />
            OutOfRange<input id="AlertOutOfRangeInd" value="<%= AlertOutOfRangeInd%>" />

            <input id="ReportAnimalsIdList" type="text" value="<%= ReportAnimalsIdList%>" />
            <input id="ReportTempChartInd" type="text" value="<%= ReportTempChartInd%>" />
            <input id="ReportStatInd" type="text" value="<%= ReportStatInd%>" />
            AnimalsIdListIni<input id="AnimalsIdListIni" type="text" value="<%= AnimalsIdListIni%>" />
        </div>
        <!--Script Section-->
        <link href="../dx/css/dx.light.css" rel="stylesheet" />
        <link href="../dx/css/dx.common.css" rel="stylesheet" />
        <script src="../dx/js/dx.all.js"></script>
        <script src="FarmJS/UserCabinet.js"></script>
    </div>
</asp:Content>
