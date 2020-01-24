<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataGaps.aspx.cs" Inherits="BoluSys.Admin.DataGaps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="text-align: center; padding: 10px 0; border-width: thin; background-color: #f2f2f2;"><h2>Data Gaps</h2></div>
    <!------Dates section----------------------------------------------------------->
    <div class="container-fluid">
        <div class="row" >
            <div class="col-md-1" style="text-align: right; padding: 10px 0;">Date From:</div>
            <div class="col-md-3" style="">
                <div id="DateFrom"></div>
            </div>
            <div class="col-md-1" style="text-align: right; padding: 10px 0;"> Date To:</div>
            <div class="col-md-3">
               <div id="DateTo"></div>
            </div>
            <div class="col-md-4">
                <div id="SearchGaps"></div>
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
    <script src="AdminJS/DataGaps.js"></script>
    <script src="../Farm/FarmJS/FarmGeneral.js"></script>
    <style type="text/css">
        /*#DateFrom {
        background-color:lawngreen;
        }*/
    </style>
    <style type="text/css">
        .dx-datagrid-headers .cls {  
    background-color:  #f2f2f2;
    font-weight:bold;
    color:black;
}
    </style>
    <!--Script Section-->
</asp:Content>
