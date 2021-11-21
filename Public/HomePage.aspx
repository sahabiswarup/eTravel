<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PublicMaster.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="e_Travel.Public.HomePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
<style type="text/css">
.GridStyle
{
  border:0px solid none; 
}
.GridStyle th
{
    display:none;
}
.lightBackground
{
    background-color:#E8EDF0;
}
.GridStyle tr td{width:600px; border-bottom:20px solid white;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridViewPackage" runat="server" AutoGenerateColumns="False" 
        CssClass="GridStyle" BorderWidth="0px" DataKeyNames="TpkID">
        <AlternatingRowStyle BorderWidth="0px" />
    <Columns>
    <asp:TemplateField>
    <ItemTemplate>
    <div style="width:580px;float:left; height:25px; font-family:Tahoma,Arial; font-size:15px; font-weight:bold;border-radius:5px 5px 0px 0px;padding:10px;" class="lightBackground">
    <asp:Label ID="PackageNameLabel" runat="server" Text='<%# Eval("TpkName") %>'/>
    </div>
    <div class="lightBackground" style="padding:0px 10px 10px 10px;width:110px;float:left;width:580px;">
    <div style="float:left;height:50px;width:82px;">
        <asp:Image ID="Image1" runat="server" ImageUrl='<%#String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("PhotoThumb"))) %>' Height="50px" Width="80px" BorderWidth="1px" BorderColor="Beige"/>
        </div>
        <div style="float:left;height:50px; width:200px; overflow:hidden; margin-left:10px;padding:0px 10px 0px 0px;border-right:1px solid black;">
        <asp:Label ID="Label4" runat="server" Text='<%# Eval("TpkDesc") %>'/>
        </div>
        <div style="float:left;margin-left:5px;">
        <asp:Label ID="Label2" runat="server" Text='<%# Eval("TotalDays") %>'/> Days/<asp:Label ID="Label3" runat="server" Text='<%# Eval("TotalNights") %>'/> Nights
        </div>
    </div>
    <div class="lightBackground" style="float:left;width:600px;">
    
    </div>
    </ItemTemplate>
    </asp:TemplateField>
    </Columns>
    </asp:GridView>
</asp:Content>
