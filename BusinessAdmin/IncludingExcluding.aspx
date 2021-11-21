<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master" AutoEventWireup="true" CodeBehind="IncludingExcluding.aspx.cs" Inherits="e_Travel.BusinessAdmin.IncludingExcluding" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="FromBody">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <div class="BoxHeader">
                   Including And Excluding
                 </div>
                <div class="BoxBody">
                    <div class="BoxContent">
                        <fieldset>
                            <legend>Including And Excluding</legend>
                          <div class="row">
                                <label for="ItemNameTextBox">Item Name</label>
                                <asp:TextBox ID="ItemNameTextBox" runat="server" TextMode="SingleLine" EnableViewState="True" MaxLength = "128" width = "660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="ItemDescTextBox">Item Desc</label>
                                <asp:TextBox ID="ItemDescTextBox" runat="server" ValidationGroup="ValidateData" TextMode="MultiLine" EnableViewState="True" MaxLength = "128" width = "660px"></asp:TextBox>
                            </div>
                             <div class="row">
                                <label for="ItemDescTextBox">Item </label>
                                <asp:RadioButton ID="IncludingRadioButton" runat="server" GroupName="Type" /> Including 
                                <asp:RadioButton ID="ExcludingRadioButton" runat="server" GroupName="Type" /> Excluding
                            </div>
                        </fieldset>
                        
                        <div id="CommandControls" style="width: 100%; float: left;">
                            <fieldset>
                                <div class="FromBodyButtonContainer">
                                    <asp:Button ID="AddButton" runat="server" Text=" Add " CssClass="FromBodyButton"
                                        Width="80px" OnClick="AddButton_Click" />
                                    <asp:Button ID="EditButton" runat="server" Text=" Edit " CssClass="FromBodyButton"
                                        Width="80px" OnClick="EditButton_Click" />
                                    <asp:Button ID="SaveButton" runat="server" Text=" Save " CssClass="FromBodyButton"
                                        ValidationGroup="ValidateData" Width="80px" OnClick="SaveButton_Click" />
                                    <asp:Button ID="ClearButton" runat="server" Text=" Clear " CssClass="FromBodyButton"
                                        Width="80px" OnClick="ClearButton_Click" />
                                </div>
                            </fieldset>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <fieldset id="Gridview">
                            <gridUC:CustomGridView ID="ItemCustomGridView" runat="server" Width="100%" />
                        </fieldset>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
