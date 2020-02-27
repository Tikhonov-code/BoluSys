<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IntakesData.aspx.cs" Inherits="BoluSys.Admin.IntakesData" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="text-align: center; padding: 10px 0; border-width: thin; background-color: #f2f2f2;">
        <h2>Water Intakes analysis</h2>
    </div>
    <!------Dates section----------------------------------------------------------->
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-1" style="text-align: right; padding: 10px 0;">Date From:</div>
            <div class="col-sm-2" style="">
                <div id="DateFrom"></div>
            </div>
            <div class="col-sm-1" style="text-align: right; padding: 10px 0;">Date To:</div>
            <div class="col-sm-2">
                <div id="DateTo"></div>
            </div>
            <div class="col-sm-1" style="text-align: right; padding: 10px 0;">Animal ID:</div>
                <div class="col-sm-2">
                    <div id="BolusIDList"></div>
                </div>
            <div class="col-sm-2">
                <div id="CheckIntakes"></div>
            </div>
        </div>
    </div>
    
    <!------Table Section----------------------------------------------------------->
    <div class="container">
        <div id="GridGaps"></div>
    </div>
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="AdminJS/IntakesData.js"></script>
    <script src="../../Farm/FarmJS/FarmGeneral.js"></script>
</asp:Content>
