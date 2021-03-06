<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSearchGridView.ascx.cs"
    Inherits="SIBINUtility.UserControl.SearchGridView.ucSearchGridView" %>
<link href="<%= ResolveClientUrl("~/UserControl/SearchGridView/ucGridView.css")%>"
    rel="stylesheet" type="text/css" />
<div style="position: relative; margin-left: auto; margin-right: auto; padding: auto;">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        &nbsp;
        <asp:Label ID="SearchLabel" runat="server"  Text="Search By Bussiness Name:" Font-Names="Times New Roman" Font-Bold="true" ></asp:Label>
            <asp:TextBox ID="SearchTextBox" runat="server" Width="150" ></asp:TextBox>
            <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" Enabled="true"/>
            <br />
            <br />
            <asp:GridView ID="refGridView" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" CssClass="ucGridView" PagerStyle-CssClass="pgr" RowStyle-CssClass="NormalRow"
                AlternatingRowStyle-CssClass="alt" SelectedRowStyle-CssClass="SelectRow" OnDataBound="refGridView_DataBound"
                OnRowDataBound="refGridView_RowDataBound" OnRowCreated="refGridView_RowCreated"
                OnRowDeleting="refGridView_RowDeleting" OnSelectedIndexChanging="refGridView_SelectedIndexChanging"
                OnPageIndexChanging="refGridView_PageIndexChanging" EmptyDataText="No Record Available!">
                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                <PagerStyle CssClass="pgr"></PagerStyle>
                <PagerTemplate>
                    <table style="width: 100%; border-spacing: 0;">
                        <tbody>
                            <tr>
                                <td class="ucGDPagerCell NextPrevAndNumeric" style="padding-top: 5px;">
                                    <div class="ucGDWrap ucGDArrPart1">
                                        <asp:Button ID="FirstButton" runat="server" Text=" " title="First Page" class="ucGDPageFirst"
                                            CommandArgument="First" CommandName="Page" Width="17" Height="17" OnClick="PageButton_Click" />
                                        <asp:Button ID="PreviousButton" runat="server" Text=" " title="Previous Page" class="ucGDPagePrev"
                                            CommandArgument="Prev" CommandName="Page" Width="17" Height="17" OnClick="PageButton_Click" />
                                    </div>
                                    <div class="ucGDWrap ucGDNumPart" style="vertical-align: top;">
                                        &nbsp;Page &nbsp;<asp:DropDownList ID="PageIndexDropDownList" runat="server" CssClass="styledDropDown"
                                            AutoPostBack="True" OnSelectedIndexChanged="PageIndexDropDownList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        &nbsp; of &nbsp;<asp:Literal ID="NoOfPagesLiteral" runat="server"></asp:Literal>
                                    </div>
                                    <div class="ucGDWrap ucGDArrPart2">
                                        <asp:Button ID="NextButton" runat="server" Text=" " title="Next Page" class="ucGDPageNext"
                                            CommandArgument="Next" CommandName="Page" Width="17" Height="17" OnClick="PageButton_Click" />
                                        <asp:Button ID="LastButton" runat="server" Text=" " title="Last Page" class="ucGDPageLast"
                                            CommandArgument="Last" CommandName="Page" Width="17" Height="17" OnClick="PageButton_Click" />
                                    </div>
                                    <div class="ucGDWrap ucGDAdvPart">
                                        <span id="ChangePageSizeLabel" class="ucGDPagerLabel" style="vertical-align: top;">Page
                                            size:</span>
                                        <asp:DropDownList ID="PageSizeDropDown" runat="server" CssClass="styledDropDown"
                                            AutoPostBack="true" OnSelectedIndexChanged="PageSizeDropDown_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>30</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="ucGDWrap ucGDInfoPart">
                                        <asp:Literal runat="server" ID="gridViewTotalRecordLiteral"></asp:Literal>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </PagerTemplate>
                <RowStyle CssClass="NormalRow"></RowStyle>
                <SelectedRowStyle CssClass="SelectRow"></SelectedRowStyle>
            </asp:GridView>
            <asp:HiddenField ID="TotalPageCountHiddenField" runat="server" />
            <asp:HiddenField ID="PageIndexHiddenField" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--    <asp:UpdateProgress ID="ucGridUpdateProgress" runat="server">
        <ProgressTemplate>
            <div style="z-index: 10000; position: absolute; height: 42px; width: 220px; top: 10%;
                left: 40%; background-color: White; border: 1px solid #000000; -moz-border-radius: 7px;
                -webkit-border-radius: 7px; -khtml-border-radius: 7px; border-radius: 7px;">
                <img style="padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loading.gif")%>" />
                <img style="padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loadingText.gif")%>" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
</div>
