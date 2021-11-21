<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master"
    AutoEventWireup="true" CodeBehind="configBusCMSMenu.aspx.cs" Inherits="e_Travel.PortalAdmin.configBusCMSMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .mGrid
        {
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
            margin-left: 350px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="upanel" runat="server">
<ContentTemplate>
    <div style="float: left; width: 40%; padding-left: 10%;">
        <fieldset class="CMSStatusFieldset">
            <legend class="CMSStatusLegend">CMS Menu Status.. </legend>
            <div style="padding: 10px; width: 827px;">
                <asp:GridView ID="ConfigCMSMenuGridView" runat="server" AutoGenerateColumns="False" datakeynames="MenuID"
                    Width="826px" Font-Bold="True" Font-Size="Medium" Font-Names="Times New Roman"
                    CssClass="mGrid" AlternatingRowStyle-CssClass="alt" EmptyDataText="No Data Available yet..">
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="" ShowHeader="true" DataField="MenuID" visible="false">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CMS Menu" ShowHeader="true" DataField="MenuName">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="CMS Menu Description" ShowHeader="true" DataField="MenuDesc">
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="CMS Menu Status">
                            <ItemTemplate>
                                <asp:CheckBox ID="configCMSCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="configCMSCheckBox_CheckedChanged" />
                                <asp:HiddenField ID="HiddenMenuID" runat="server" Value='<%#Eval("MenuID")%>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </fieldset>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdateProgress ID="GridUpdateProgress" runat="server" AssociatedUpdatePanelID="upanel">
        <ProgressTemplate>
            <div style="z-index: 10000; position: absolute; height: 42px; width: 220px; top: 10%;
                left: 40%; background-color: White; border: 1px solid #000000; -moz-border-radius: 7px;
                -webkit-border-radius: 7px; -khtml-border-radius: 7px; border-radius: 7px;">
                <img style="padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loading.gif")%>" />
                <img style="padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loadingText.gif")%>" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
