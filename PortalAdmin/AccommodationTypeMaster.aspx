<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master" AutoEventWireup="true" CodeBehind="AccommodationTypeMaster.aspx.cs" Inherits="e_Travel.PortalAdmin.AccommodationTypeMaster" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class = "FromBody">
        <div class = "BoxHeader"> Accommodation Type Master</div>
        <div class = "BoxBody">
            <div class = "BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Accommodation Type Master</legend>
                            <div class="row">
                                <label for="AccommodationTypeNameTextBox">Accommodation Type Name</label>
                                <asp:TextBox ID="AccommodationTypeNameTextBox" runat="server" TextMode="SingleLine" EnableViewState="True" MaxLength = "128" width = "660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="AccTypeDescTextBox">Acc Type Desc</label>
                                <asp:TextBox ID="AccTypeDescTextBox" runat="server" ValidationGroup="ValidateData" TextMode="MultiLine" EnableViewState="True" MaxLength = "128" width = "660px"></asp:TextBox>
                            </div>
                        </fieldset>
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="AddButton" runat="server" Text=" Add " 
                                    CssClass="FromBodyButton"  Width = "80px" 
                                    onclick="AddButton_Click" />
                                <asp:Button ID="EditButton" runat="server" Text=" Edit " 
                                    CssClass="FromBodyButton"  Width = "80px" 
                                    onclick="EditButton_Click" />
                                <asp:Button ID="SaveButton" runat="server" Text=" Save " 
                                    CssClass="FromBodyButton"  ValidationGroup="ValidateData" Width = "80px" 
                                    onclick="SaveButton_Click" />
                                <asp:Button ID="ClearButton" runat="server" Text=" Clear " 
                                    CssClass="FromBodyButton"  Width = "80px" 
                                    onclick="ClearButton_Click" />
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <fieldset>
                    <gridUC:CustomGridView ID="AccommodationTypeMasterCustomGridView" runat="server" Width = "100%"/>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
