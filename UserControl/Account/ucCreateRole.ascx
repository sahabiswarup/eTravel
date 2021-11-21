<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCreateRole.ascx.cs" Inherits="SIBINUtility.UserControls.Account.ucCreateRole" %>
<link href="<%= ResolveClientUrl("~/Styles/Account.css")%>" rel="stylesheet" type="text/css" />

<div id="CreateRoleDiv" class="AccountRegisterUserPanel">
    <p>Create / Manage user roles </p>
    <span class="AccountfailureNotification">
          <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>
    <asp:ValidationSummary ID="CreateRoleValidationSummary" runat="server" CssClass="AccountfailureNotification" 
                        ValidationGroup="CreateRoleValidationGroup"/>

     <div class="AccountBody">
        <fieldset>
            <legend>Role Information</legend>
            <div class="row">
                <label for="RoleName">Role Name :</label>
                <asp:TextBox ID="RoleName" runat="server" autocomplete="off"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RoleNameRequired" runat="server" ControlToValidate="RoleName" 
                        CssClass="AccountfailureNotification" ErrorMessage="Role Name is required." ToolTip="Role Name is required." 
                        ValidationGroup="CreateRoleValidationGroup">*</asp:RequiredFieldValidator>
            </div>
        </fieldset>
        <p class="FromBodyButtonContainer">
            <asp:Button ID="CreateRoleButton" runat="server" Text="Create Role" ValidationGroup="CreateRoleValidationGroup" 
                CssClass="FromBodyButton" onclick="CreateRoleButton_Click" />
        </p>
        <asp:GridView ID="RolesGridView" runat="server" AutoGenerateColumns="False" 
             Width="98%" onrowdeleting="RolesGridView_RowDeleting">
            <Columns>
                <asp:BoundField DataField="RoleName" HeaderText="Role Name" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" 
                            CommandName="Delete" Text="Delete" OnClientClick="return confirm('Do you really want to delete this role ?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</div>