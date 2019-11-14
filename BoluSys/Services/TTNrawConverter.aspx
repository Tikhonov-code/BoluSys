<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TTNrawConverter.aspx.cs" Inherits="BoluSys.Services.TTNrawConverter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Raw:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txb_Raw" runat="server" Width="846px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Time Z:"></asp:Label></td>
                <td>
                    <asp:TextBox ID="txb_TimeZ" runat="server" Width="846px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btn_ConvertData" runat="server" Text="Convert  Data" OnClick="btn_ConvertData_Click" /></td>
                <td></td>
            </tr>
        </table>
        <br />Results:
        <div>
            <asp:TextBox ID="txb_Results" runat="server" TextMode="MultiLine" BackColor="#CCCCCC" BorderColor="Black" BorderStyle="Solid" Height="587px" Width="968px"></asp:TextBox>
        </div>
    </form>
    <p>
        &nbsp;
    </p>
</body>
</html>
