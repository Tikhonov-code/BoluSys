<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="BoluSys.Farm.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-xl">
        <div class="row">
            <div class="col-lg-7" style="background-color: #ebf7f9;">
                <%--                Date:<input id="DateSearch" type="date" onchange="ChartsShowAllRequest(this.value);" />
                &nbsp&nbsp&nbsp<a id="ProgressBar" name="ProgressBar" style="visibility: hidden;"><progress></progress>&nbsp&nbspData Loading...</a>--%>

                <!----------------Dashboard------------------------------------------------>
                <div class="well" style="text-align: center; background-color: aqua;">
                    <mark style="font-size: large; font-weight: bold; background-color: aqua;">Under Monitoring : <%= TotalCowsNumberInfo %> cows</mark>
                </div>

                <%--    Herd section--%>
                <div class="well well-sm" style="background-color: #ffb3b3;">
                    <mark style="font-size: large; font-weight: bold; background-color: #ffb3b3;">Sick Cows: <%= num_HerdList_Sick %> , the last 24 hours temperature > 41&#176;C was detected

                    </mark>
                </div>
                <div id="herd_Sick">
                    <%=HerdList_Sick %>
                </div>
                <div class="well well-sm" style="background-color: yellow;">
                    <mark style="font-size: large; font-weight: bold; background-color: yellow;">Sad Cows: <%= num_HerdList_Sad %> ,  the last 3 hours average temperature > 40.5&#176;C was detected
                    </mark>
                </div>
                <div id="herd_Sad">
                    <%=HerdList_Sad %>
                </div>
                <div class="well well-sm" style="background-color: chartreuse;">
                    <mark style="font-size: large; font-weight: bold; background-color: chartreuse;">Healthy Cows: <%= num_HerdList_Healthy %> , OK!</mark>
                    <a href="#herd_Healthy" class="btn btn-success" data-toggle="collapse" title="Details">
                        <img src="imgs/iconfinder_cow_4591898.svg" class="rounded" width="30" height="30">
                        </a>
                </div>
                <div id="herd_Healthy" class="collapse">
                    <%=HerdList_Healthy %>
                </div>
            </div>
            <!--Info alerts zone-->
            <div class="col-lg-5" style="background-color: #ebf7f9;">
                <div class="demo-container">
                    <div class="dx-fieldset">
                        <div class="dx-fieldset-header" style="text-align: center;">Alerts</div>
                        <div class="dx-field">
                            <div id="AlertsLine"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-----Context Menu Section-------------->
    <div class="demo-container">
        <div id="context-menu"></div>
    </div>
    <!-----Context Menu Section-------------->
    <%--    Data section--%>
    <div style="display: none;">
        <input id="TotalCowsNumberInfo" type="number" value="<%= TotalCowsNumberInfo %>" />
        <%--<input id="CowsToCheck" type="number" value="<%= CowsToCheck %>" />
                    <input id="CowsAtRisk" type="number" value="<%= CowsAtRisk %>" />
                    <input id="CowsUnderMonitoring" type="number" value="<%= CowsUnderMonitoring %>" />--%>
    </div>
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="FarmJS/Dashboard.js"></script>
    <!--Script Section-->

    <script>
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

</asp:Content>
