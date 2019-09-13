<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stat.aspx.cs" Inherits="BoluSys.Farm.Stat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table style = "width:90%; text-align:center; ">
        <tr>
            <td style="color:black;font-weight:bold;">Date:</td><td><div id="date"></div></td>
        </tr>
    </table>
      

    <div style="width: 100%; text-align: center;">
        <div class="demo-container" style="width: 90%; display: inline-block;">
            <div id="farmReport"></div>
        </div>
    </div>
    <!--Script Section-->
    <link href="../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../dx/js/dx.all.js"></script>
    <script src="FarmJS/Farm.js"></script>
</asp:Content>

