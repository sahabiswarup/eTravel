<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessPublic.Master" AutoEventWireup="true" CodeBehind="PublicCMS.aspx.cs" Inherits="e_Travel.BusinessPublic.PublicCMS"  Theme="BusPublicDefault" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class = "FromBody">
        <div style = "width: 100%; float: left; min-height: 400px;">
            <div style = "width: 100%; float: left;">
                <h1 id="contentHeader" runat="server"></h1>
            </div>
            <div style = "width: 100%; float: left; text-align: justify;">
                <asp:Label ID="PageContentLabel" runat="server" Text=""></asp:Label>
            </div>
            <div style = "clear: both"></div>
        </div>
        <div style = "clear: both"></div>
    </div>
</asp:Content>
