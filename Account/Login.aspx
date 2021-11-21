<%@ Page Title="Log In" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="e_Travel.Account.Login" %>
     <%@ Register src="~/UserControl/Account/ucEncryptedLogin.ascx" tagname="ucEncryptedLogin" tagprefix="uc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="head">
    <link href="../App_Themes/AdminThemeDefault/Account.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="FromBody">
        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
            <ContentTemplate>
                <div class="BoxHeader">Login</div>

                <div class="BoxBody">
                        <div class="BoxContent">
                         <uc1:ucEncryptedLogin ID="ucEncryptedLogin1" runat="server" />
                        </div>
                        </div>
                </ContentTemplate>
                </asp:UpdatePanel>



                </div>
</asp:Content>
