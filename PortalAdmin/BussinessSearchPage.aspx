<%@ Page Title="Bussiness Search" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master"
    AutoEventWireup="true" CodeBehind="BussinessSearchPage.aspx.cs" Inherits="e_Travel.PortalAdmin.BussinessSearchPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .BussinessInfoFieldset
        {
            font-family: sans-serif;
            border: 3px solid #12ACC8;
            border-radius: 5px;
            background: #ddd;
        }
        .BussinessInfoLegend
        {
            background: #12ACC8;
            color: #fff;
            padding: 5px 10px;
            font-size: 18px;
            border-radius: 5px;
            box-shadow: 0 0 0 5px #ddd;
            font-family: Times New Roman;
            margin-left: 420px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="BoxBody">
        <div class="BoxContent">
            <fieldset class="BussinessInfoFieldset">
                <legend class="BussinessInfoLegend">Bussiness Information..</legend>
                <div class="row">
                    <searchGridUC:SearchCustomGridView ID="BussinessSearchPageCustomGridView" runat="server"
                        Width="100%" />
                </div>
            </fieldset>
            <div style="float: right; padding: 10px;">
                <asp:Button ID="ManageBussinessButton" runat="server" Text="Manage Selected Bussiness"
                    CssClass="FromBodyButton" OnClick="ManageBussinessButton_Click" />
            </div>
            <asp:UpdatePanel ID="BSPUpdatePanel" runat="server">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
