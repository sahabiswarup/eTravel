<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PublicMaster.Master" AutoEventWireup="true" CodeBehind="SearchPage.aspx.cs" Inherits="e_Travel.Public.SearchPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:GridView ID="PackageListGridView" runat="server" AutoGenerateColumns="false" >
<Columns>
<asp:TemplateField >
<ItemTemplate>
<div>



</div>

</ItemTemplate>
</asp:TemplateField>
</Columns>

</asp:GridView>

</asp:Content>
