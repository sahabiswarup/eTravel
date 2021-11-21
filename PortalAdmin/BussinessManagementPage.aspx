<%@ Page Title="Bussiness Management" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master"
    AutoEventWireup="true" CodeBehind="BussinessManagementPage.aspx.cs" Inherits="e_Travel.PortalAdmin.BussinessManagementPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mGrid
        {
            width: 100%;
            background-color: #fff;
            margin: 5px 0 10px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
        }
        .mGrid td
        {
            padding: 2px;
            border: solid 1px #c1c1c1;
            color: #717171;
        }
        .mGrid th
        {
            padding: 4px 2px;
            color: #fff;
            background: #424242 url(../Images/IconImage/grd_head.png) repeat-x top;
            border-left: solid 1px #525252;
            font-size: 0.9em;
        }
        .mGrid .alt
        {
            background: #fcfcfc url(../Images/IconImage/grd_alt.png) repeat-x top;
        }
        .status0
        {
            color: Blue;
            text-shadow: 0px 2px 3px #666;
            font-weight: bold;
        }
        .status1
        {
            color: Red;
            text-shadow: 0px 2px 3px #666;
            font-weight: bold;
        }
        .BussinessDtlDiv
        {
            float: left;
            width: 40%;
            border: 3px solid #12ACC8;
            border-radius: 10px 10px 2px 2px;
            background: rgba(0,0,0,0.25);
            box-shadow: 0 2px 6px rgba(0,0,0,0.5), inset 0 1px rgba(255,255,255,0.3), inset 0 10px rgba(255,255,255,0.2), inset 0 10px 20px rgba(255,255,255,0.25), inset 0 -15px 30px rgba(0,0,0,0.3);
            -o-box-shadow: 0 2px 6px rgba(0,0,0,0.5), inset 0 1px rgba(255,255,255,0.3), inset 0 10px rgba(255,255,255,0.2), inset 0 10px 20px rgba(255,255,255,0.25), inset 0 -15px 30px rgba(0,0,0,0.3);
            -webkit-box-shadow: 0 2px 6px rgba(0,0,0,0.5), inset 0 1px rgba(255,255,255,0.3), inset 0 10px rgba(255,255,255,0.2), inset 0 10px 20px rgba(255,255,255,0.25), inset 0 -15px 30px rgba(0,0,0,0.3);
            -moz-box-shadow: 0 2px 6px rgba(0,0,0,0.5), inset 0 1px rgba(255,255,255,0.3), inset 0 10px rgba(255,255,255,0.2), inset 0 10px 20px rgba(255,255,255,0.25), inset 0 -15px 30px rgba(0,0,0,0.3);
            -ms-box-shadow: 0 2px 6px rgba(0,0,0,0.5), inset 0 1px rgba(255,255,255,0.3), inset 0 10px rgba(255,255,255,0.2), inset 0 10px 20px rgba(255,255,255,0.25), inset 0 -15px 30px rgba(0,0,0,0.3);
            margin-top: 10px;
            height: 280px;
        }
        .CMSStatusFieldset
        {
            font-family: sans-serif;
            border: 3px solid #12ACC8;
            border-radius: 5px;
            background: #ddd;
        }
        .CMSStatusLegend
        {
            background: #12ACC8;
            color: #fff;
            padding: 5px 10px;
            font-size: 18px;
            border-radius: 5px;
            box-shadow: 0 0 0 5px #ddd;
            font-family: Times New Roman;
            margin-left: 160px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%;">
        <div class="BussinessDtlDiv">
            <%-- <fieldset style="border: 1px solid green; border-style: solid">--%>
            <%-- <legend style="padding: 0.1em 0.5em; border: 1px solid green; color: green; font-size: 90%;
                    text-align: right;">Business Details </legend>--%>
            <p style="line-height: 25px; text-align: center; text-shadow: 0px 2px 3px #666;padding:80px;">
                <asp:Label ID="BussinessNameTextBox" runat="server" Font-Bold="true" Font-Names="Times New Roman"
                    Font-Size="Large"></asp:Label>
                <br />
                <asp:Label ID="AddressTextBox" runat="server" Font-Names="Times New Roman" Font-Size="Medium"></asp:Label>
                <br />
                <asp:Label ID="ContactNoTextBox" runat="server" Font-Names="Times New Roman" Font-Size="Medium"></asp:Label>
                <br />
                <asp:Label ID="EmailIDTextBox" runat="server" Font-Names="Times New Roman" Font-Size="Medium"></asp:Label>
            </p>
            <%--</fieldset>--%>
        </div>
        <div style="float: left; width: 40%; padding-left: 10%;">
            <fieldset class="CMSStatusFieldset">
                <legend class="CMSStatusLegend">CMS Status.. </legend>
                <div style="padding: 10px">
                    <asp:GridView ID="CMSGridView" runat="server" AutoGenerateColumns="False" Width="400px"
                        Font-Bold="true" Font-Size="Medium" Font-Names="Times New Roman" CssClass="mGrid"
                        AlternatingRowStyle-CssClass="alt" >
                        <Columns>
                            <asp:BoundField HeaderText="CMS" ShowHeader="true" DataField="CMS">
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Request Status">
                                <ItemTemplate>
                                    <a href="<%# Page.ResolveClientUrl((string)Eval("RequestLink"))%>">
                                        <asp:Label ID="StatusLabel" runat="server" Text='<%#Eval("Request")%>' class='<%# "status"+ ((int.Parse(Eval("Request").ToString())>0)?"1":"0")%>'></asp:Label>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </fieldset>
        </div>
    </div>
</asp:Content>
