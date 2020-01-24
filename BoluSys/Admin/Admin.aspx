<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BoluSys.Admin.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!--Menu section------------------------------------------------------------------->
    <div class="demo-container">
        <div class="form">
            <div>
                <div id="menu"></div>
                <div id="product-details" class="hidden">
                    <img src="" />
                    <div class="name"></div>
                    <div class="price"></div>
                </div>
            </div>
        </div>
    </div>
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="AdminJS/Adminjs.js"></script>
    <!--Script Section-->
</asp:Content>
