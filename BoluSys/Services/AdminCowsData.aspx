<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminCowsData.aspx.cs" Inherits="BoluSys.Services.AdminCowsData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" AutoGenerateEditButton="True" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" DataKeyNames="id" DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="None" OnRowUpdated="GridView1_RowUpdated" PageSize="100">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <Columns>
                <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                <asp:BoundField DataField="bolus_id" HeaderText="bolus_id" ReadOnly="True" SortExpression="bolus_id" />
                <asp:BoundField DataField="animal_id" HeaderText="animal_id" ReadOnly="True" SortExpression="animal_id" />
                <asp:BoundField DataField="AnimalInfo" HeaderText="AnimalInfo" SortExpression="AnimalInfo" />
                <asp:BoundField DataField="Age_Lactation" HeaderText="Age_Lactation" SortExpression="Age_Lactation" />
                <asp:BoundField DataField="Current_Stage_Of_Lactation" HeaderText="Current_Stage_Of_Lactation" SortExpression="Current_Stage_Of_Lactation" />
                <asp:BoundField DataField="Health_Concerns_Illness_History" HeaderText="Health_Concerns_Illness_History" SortExpression="Health_Concerns_Illness_History" />
                <asp:BoundField DataField="Overall_Health" HeaderText="Overall_Health" SortExpression="Overall_Health" />
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
            </Columns>
            <EditRowStyle BorderColor="#66FF66" />
            <FooterStyle BackColor="Tan" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <SortedAscendingCellStyle BackColor="#FAFAE7" />
            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
            <SortedDescendingCellStyle BackColor="#E1DB9C" />
            <SortedDescendingHeaderStyle BackColor="#C2A47B" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT * FROM [Bolus]" UpdateCommand="UPDATE Bolus SET AnimalInfo = @AnimalInfo, Age_Lactation = @Age_loctation, Current_Stage_Of_Lactation = @Current_Stage_Of_Lactation, Health_Concerns_Illness_History = @Health_Concerns_Illness_History, Overall_Health = @Overall_Health, Comments = @Comments WHERE (id = @id)">
            <UpdateParameters>
                <asp:ControlParameter ControlID="GridView1" Name="AnimalInfo" PropertyName="SelectedValue" />
                <asp:Parameter Name="Age_loctation" />
                <asp:Parameter Name="Current_Stage_Of_Lactation" />
                <asp:Parameter Name="Health_Concerns_Illness_History" />
                <asp:Parameter Name="Overall_Health" />
                <asp:Parameter Name="Comments" />
                <asp:Parameter Name="id" />
            </UpdateParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
