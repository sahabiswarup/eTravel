<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master" AutoEventWireup="true" CodeBehind="ManageRole.aspx.cs" Inherits="e_Travel.Account.ManageRole"  %>
<%@ Register src="~/UserControl/Account/ucManageUsersInRole.ascx" tagname="ucManageUsersInRole" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:ucManageUsersInRole ID="ucManageUsersInRole1" runat="server" />
    
</asp:Content>
