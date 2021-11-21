<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="e_Travel.Account.CreateUser" %>
<%@ Register src="~/UserControl/Account/ucEncryptedRegisterUser.ascx" tagname="ucEncryptedRegisterUser" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <link href="../App_Themes/AdminThemeDefault/Account.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class = "FromBody">
        <div class = "BoxHeader">Register User </div>
        <div class = "BoxBody">
        <div class = "BoxContent">
    <uc1:ucEncryptedRegisterUser ID="ucEncryptedRegisterUser1" runat="server" />
    </div>
    </div>
    </div>
</asp:Content>
