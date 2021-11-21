<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucManageUsersInRole.ascx.cs" Inherits="SIBINUtility.UserControls.Account.ucManageUsersInRole" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<link href="<%= ResolveClientUrl("~/Styles/Account.css")%>" rel="stylesheet" type="text/css" />

<div id="CreateRoleDiv" class="AccountRegisterUserPanel" style="width:700px; height:auto">
    <span class="AccountfailureNotification">
          <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
    </span>

    <div class="AccountBody">
    <fieldset>
        <legend>Select Role</legend> 
        <div class="row">
            <label for="RoleListDropDownList">Role</label>
            <asp:DropDownList ID="RoleListDropDownList" runat="server" 
                ValidationGroup="CheckRole">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="RoleListDropDownList" ErrorMessage="*" 
                ValidationGroup="CheckRole"></asp:RequiredFieldValidator>
        </div>
        <div class="row" >
            <label for="UserTypeDropDownList">User Type</label>
            <asp:DropDownList ID="UserTypeDropDownList" runat="server" AutoPostBack="True" 
                onselectedindexchanged="UserTypeDropDownList_SelectedIndexChanged">
                <asp:ListItem Value="0">&lt;-- Select --&gt;</asp:ListItem>
                <asp:ListItem Value="1">Employee</asp:ListItem>
                <asp:ListItem Value="2">Non-Employee</asp:ListItem>
            </asp:DropDownList></div>
            
    </fieldset>
    <div class="pageContentWrapper">
         
        <div>
            <fieldset style="width:670px; float:left;">
                <legend>Search for Users</legend>
                <div class="row">
                    <label for="SearchByDropDownList">Search By:</label>
                    <asp:DropDownList ID="SearchByDropDownList" runat="server">
                        <asp:ListItem Value="0"> &lt;-- Select --&gt;</asp:ListItem>
                        <asp:ListItem Value="1">User name</asp:ListItem>
                        <asp:ListItem Value="2">E-Mail</asp:ListItem>
                    </asp:DropDownList>
                </div>
              <div class="row">
                <label for="SearchByTextBox">for:</label>
                <asp:TextBox ID="SearchByTextBox" runat="server"></asp:TextBox>
                <asp:Button ID="FindUserButton" runat="server" Text="Find User" CssClass="AccountButton" onclick="FindUserButton_Click" />
            </div>
                <%--<div class="row">
                <label for="FindUserButton"></label>
                    <asp:Button ID="FindUserButton" runat="server" Text="Find User" CssClass="AccountButton" onclick="FindUserButton_Click" />
                </div>--%>
            </fieldset>
            <div>
            <p>
                    <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" style="float:left; margin-top:30px;"
                     Width="94%" onrowdeleting="UsersGridView_RowDeleting" 
                        onrowcreated="UsersGridView_RowCreated" SelectedRowStyle-BackColor="#CCCCCC"  ForeColor="#333399">
                        <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                    <Columns>

                    </Columns>
                     <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" 
                            BorderStyle="None" />
                        <HeaderStyle Height="35px" BackColor="#507CD1" Font-Bold="True" 
                            ForeColor="White" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                            BorderStyle="None" VerticalAlign="Middle" />
                         <AlternatingRowStyle BackColor="White" BorderStyle="None" />
                    </asp:GridView>
          </p>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                 <asp:GridView ID="UserListGridView" runat="server"   
                        AutoGenerateColumns="False" style="float:left; margin-top:40px; "
                         Width = "94%"  onrowcreated="UserListGridView_RowCreated"  SelectedRowStyle-BackColor="#CCCCCC"  ForeColor="#333399">
                       <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                        <Columns>
                        </Columns>
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" 
                            BorderStyle="None" />
                        <HeaderStyle Height="35px" BackColor="#507CD1" Font-Bold="True" 
                            ForeColor="White" HorizontalAlign="Center" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" 
                            BorderStyle="None" VerticalAlign="Middle" />
                         <AlternatingRowStyle BackColor="White" BorderStyle="None" />
                    </asp:GridView>
                </ContentTemplate>
                 </asp:UpdatePanel>
          </div>
        </div>
    </div>
          
    </div>
</div>
