<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChartsXY.aspx.cs" Inherits="BoluSys.Farm.ChartsXY" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Date:<input id="DateSearch" type="date" onchange="ChartsShowAllRequest(this.value);" />
    &nbsp&nbsp&nbsp<a id="ProgressBar" name="ProgressBar" style="visibility: hidden;"><progress></progress>&nbsp&nbspData Loading...</a>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="FarmJS/ChartsXY.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://cdnjs.cloudflare.com/ajax/libs/json2/20130526/json2.min.js"></script>
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
    <style>
        .w3-btn {
            width: 70px;
            background-color:#e6ff99;
        }
    </style>
    <!--Chart Section-->
    <table>
        <tr>
            <td style="vertical-align: top;">Animal_ID<br />
                <div id="bolus_list" class="btn-group-vertical table-bordered">
                </div>
            </td>
            <td>
                <div id="curve_chart" style="width: 1000px; height: 500px"></div>
            </td>
            <td>
                <div id="Description" style="background-color: #e6e6e6;"></div>
            </td>
        </tr>
    </table>
</asp:Content>
