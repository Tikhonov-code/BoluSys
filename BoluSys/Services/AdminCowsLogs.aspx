<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminCowsLogs.aspx.cs" Inherits="BoluSys.Services.AdminCowsLogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="demo-container">
        <div id="grid"></div>
        <div id="action-add"></div>
        <div id="action-remove"></div>
        <div id="action-edit"></div>

        
    </div>

    <!--Script Section-->
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/19.2.4/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/19.2.4/css/dx.greenmist.css" />
    <script src="https://cdn3.devexpress.com/jslib/19.2.4/js/dx.all.js"></script>
<%--    <link href="../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../dx/js/dx.all.js"></script>--%>
    <script src="MyJS/AdmincowsLog.js"></script>
    <link href="MyCSS/AdminCowsLogs.css" rel="stylesheet" />
</asp:Content>
