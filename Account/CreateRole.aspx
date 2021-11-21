<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master" AutoEventWireup="true" CodeBehind="CreateRole.aspx.cs" Inherits="e_Travel.Account.CreateRole" %>
<%@ Register src="~/UserControl/Account/ucCreateRole.ascx" tagname="ucCreateRole" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class = "FromBody">
        <div class = "BoxHeader">Register User </div>
        <div class = "BoxBody">
        <div class = "BoxContent">

<uc1:ucCreateRole ID="ucCreateRole1" runat="server" />
</div></div>
</div>
</asp:Content>
