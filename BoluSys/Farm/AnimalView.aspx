<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AnimalView.aspx.cs" Inherits="BoluSys.Farm.AnimalView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!----Parameters Section---------------------------------------------------------------------------->
    <div class="demo-container" style="background-color: #e6e6e6;">
        <div class="long-title">
            <h3>Parameters</h3>
            <div id="form-container">
                <div id="form"></div>
            </div>
        </div>
    </div>
    <!----End of Parameters Section--------------------------------------------------------------------->
    <!----Combo Chart Section---------------------------------------------------------------------------->
    <div class="demo-container">
        <div id="chart-demo">
            <div id="chart"></div>
            <div class="options">
                <div class="caption">Options</div>
                <div class="option">
                    <div id="useAggregationToggle"></div>
                </div>
                <div class="option">
                    <span>Interval:</span>
                    <div id="aggregateIntervalSelector"></div>
                </div>
                <div class="option">
                    <span>Method:</span>
                    <div id="aggregateFunctionSelector"></div>
                </div>
            </div>
        </div>
    </div>
    <!----END of Combo Chart Section---------------------------------------------------------------------------->
    <!--Script Section-->
    <link href="../../dx/css/dx.light.css" rel="stylesheet" />
    <link href="../../dx/css/dx.common.css" rel="stylesheet" />
    <script src="../../dx/js/dx.all.js"></script>
    <script src="FarmJS/FarmGeneral.js"></script>
    <script src="FarmJS/AnimalView.js"></script>
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
    <style type="text/css">
        #form-container {
            margin: 10px 10px 30px;
        }

        #button {
            float: right;
            margin-top: 20px;
        }

        .long-title h3 {
            font-family: 'Segoe UI Light', 'Helvetica Neue Light', 'Segoe UI', 'Helvetica Neue', 'Trebuchet MS', Verdana;
            font-weight: 200;
            font-size: 28px;
            text-align: center;
            margin-bottom: 20px;
        }
    </style>

    <style type="text/css">
        #chart-demo {
            height: 700px;
            width: 100%;
        }

        #chart {
            height: 500px;
            margin: 0 0 15px;
        }

        .options {
            padding: 20px;
            margin-top: 20px;
            background-color: rgba(191, 191, 191, 0.15);
        }

        .caption {
            font-size: 18px;
            font-weight: 500;
        }

        .option {
            margin-top: 10px;
        }

            .option > span {
                width: 50px;
                display: inline-block;
                margin-right: 10px;
            }

            .option > .dx-selectbox {
                display: inline-block;
                vertical-align: middle;
            }
    </style>
</asp:Content>

