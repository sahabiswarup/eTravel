<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="SocialNetworkDtls.aspx.cs" Inherits="e_Travel.BusinessAdmin.SocialNetworkDtls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../UserControl/GridView/ucGridView.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            Social Links Details
        </div>
        <div class="BoxBody">
            <div class="BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Social Links Details</legend>
                            <div class="row">
                                <asp:GridView ID="SocialGridView" runat="server" AutoGenerateColumns="false" CssClass="ucGridView"
                                    RowStyle-CssClass="NormalRow" AlternatingRowStyle-CssClass="alt" SelectedRowStyle-CssClass="SelectRow"
                                    EmptyDataText="No Record Available!" GridLines="None" HeaderStyle-CssClass="header">
                                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Show">
                                            <ItemTemplate>
                                                <div style="display: none;">
                                                    <asp:Label ID="FollowIDLabel" runat="server" Text='<%#Eval("FollowID") %>'></asp:Label>
                                                </div>
                                                <asp:CheckBox ID="ShowCheckBox" runat="server" Checked='<%#this.Show(Convert.ToString(Eval("Show")))%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="SocialNetworkNameLabel" runat="server" Text='<%#Eval("Name") %>' ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Link">
                                            <ItemTemplate>
                                                <asp:TextBox ID="LinkTextBox" runat="server" Text='<%#Eval("Link") %>' Width="300px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="NormalRow"></RowStyle>
                                    <SelectedRowStyle CssClass="SelectRow"></SelectedRowStyle>
                                </asp:GridView>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="SaveButton" runat="server" Text=" Save Changes " CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="120px" OnClick="SaveButton_Click" />
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
