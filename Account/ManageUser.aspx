<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master"
    AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="e_Travel.Account.ManageUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .grid .NormalRow
        {
            background-color: #FFFFFF;
        }
        .grid .SelectRow
        {
            font-weight: bold;
            background: #fee2aa;
        }
        .grid .alt
        {
            background-color: #ececec;
        }
        .grid .NormalRow:hover, .grid .ucGridView tbody tr.alt:hover
        {
            background-color: #FEE2AA;
            color: black;
            font-weight: bold;
        }
        .grid .header
        {
            background-color: #A4ABB2;
            color: #3B3B3B;
            height: 35px;
            padding-left: 8px;
            text-align: left;
        }
        .grid td
        {
            border: 1px solid #A4ABB2;
        }
        .grid .header th
        {
            border: 1px solid #A4ABB2;
        }
        .clear
        {
            clear: both;
            height: 10px;
        }
        .backButton
        {
            float:right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <asp:UpdatePanel runat="server" ID="UpdatePanel2">
            <ContentTemplate>
                <div class="BoxHeader">
                    Manage User</div>
                <asp:Panel ID="UserListPanel" runat="server">
                    <div class="BoxBody">
                        <div class="BoxContent">
                            <fieldset>
                                <legend>Manage User </legend>
                                <div class="row">
                                    <label for="BusinessNameTextBox">
                                        Business Name
                                    </label>
                                    <asp:TextBox ID="BusinessNameTextBox" runat="server" Width="630px"></asp:TextBox>
                                </div>
                                <div class="row">
                                    <label for="UserRoleDropDownList">
                                        User Role
                                    </label>
                                    <asp:DropDownList ID="UserRoleDropDownList" runat="server" Width="640px" AppendDataBoundItems="True">
                                    </asp:DropDownList>
                                </div>
                                <div class="row">
                                    <label for="UserNameTextBox">
                                        User Name
                                    </label>
                                    <asp:TextBox ID="UserNameTextBox" runat="server" Width="630px"></asp:TextBox>
                                </div>
                                <div class="row">
                                    <label for="UserEmailTextBox">
                                        User Email
                                    </label>
                                    <asp:TextBox ID="UserEmailTextBox" runat="server" Width="630px"></asp:TextBox>
                                </div>
                                <div class="FromBodyButtonContainer">
                                    <asp:Button ID="SearchUserButton" runat="server" Text=" Search " CssClass="FromBodyButton"
                                        ValidationGroup="ValidateData" Width="80px" OnClick="SearchUserButton_Click" />
                                </div>
                            </fieldset>
                            <fieldset>
                                <gridUC:CustomGridView ID="UserListCustomGridView" runat="server" Width="100%" />
                                <div class="clear">
                                </div>
                                <div class="FromBodyButtonContainer">
                                    <asp:Button ID="ResetPasswordButton" runat="server" Text="Reset Password" OnClick="ResetPasswordButton_Click"
                                        CssClass="FromBodyButton" />
                                    <asp:Button ID="LockUserButton" runat="Server" Text="Enable User" OnClick="LockUserButton_Click"
                                        CssClass="FromBodyButton" />
                                    <asp:Button ID="AddUserInRoleButton" runat="server" Text="Assign Role" OnClick="AddUserInRoleButton_Click"
                                        CssClass="FromBodyButton" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="UserDetailsPanel" runat="server" Visible="false">
                    <div class="BoxBody">
                        <div class="BoxContent">
                            <fieldset>
                                <legend>User Details </legend>  
                                <div class="backButton"><asp:LinkButton ID="BackLinkButton" runat="server" Text="Back To User List" OnClick="BackLinkButton_Click"></asp:LinkButton></div>
                                
                                <div class="row">
                                    <label for="UserNameLabel">
                                        User Name
                                    </label>
                                    <asp:Label ID="UserNameLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="row">
                                    <label for="UserRoleLabel">
                                        Role
                                    </label>
                                    <asp:Label ID="UserRoleLabel" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="grid">
                                    <asp:GridView ID="RoleListGridView" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="alt"
                                        OnSelectedIndexChanging="RoleListGridView_SelectedIndexChanging" SelectedRowStyle-CssClass="SelectRow"
                                        EmptyDataText="No Record Available!" GridLines="None" HeaderStyle-CssClass="header">
                                        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <div style="height: 28px">
                                                        <asp:ImageButton ID="EditPhotoImageButton" runat="server" CommandName="Select" ImageUrl="~/UserControl/GridView/GridViewIcons.ashx?T=S"
                                                            ToolTip="Edit/Update Image Information." />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Role" DataField="RoleName">
                                                <ItemStyle Width="400px" />
                                            </asp:BoundField>
                                        </Columns>
                                        <RowStyle CssClass="NormalRow"></RowStyle>
                                        <SelectedRowStyle CssClass="SelectRow"></SelectedRowStyle>
                                    </asp:GridView>
                                </div>
                                <asp:Panel ID="ConfirmationPanel" runat="server" Visible="false">
                                    <div class="row">
                                        <label for="UserNameTextBox">
                                            Select The Business Name
                                        </label>
                                    </div>
                                    <gridUC:CustomGridView ID="BusinessCustomGridView" runat="server" Width="100%" />
                                </asp:Panel>
                            </fieldset>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
