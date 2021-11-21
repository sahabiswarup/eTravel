<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="SupplementPoints.aspx.cs" Inherits="e_Travel.BusinessAdmin.SupplementPoints" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            Supplement Points</div>
        <div class="BoxBody">
            <div class="BoxContent">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                        <fieldset>
                            <legend>Supplement Details</legend>
                            <div class="row">
                                <label for="SupplementNameTextBox">
                                    Supplement Point Name
                                </label>
                                <asp:TextBox ID="SupplementNameTextBox" runat="server" ValidationGroup="ValidateData"
                                    TextMode="SingleLine" EnableViewState="True" MaxLength="256" Width="660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="SupplementDescTextBox">
                                    Supplement Point Description
                                </label>
                                <asp:TextBox ID="SupplementDescTextBox" runat="server" ValidationGroup="ValidateData"
                                    TextMode="MultiLine" EnableViewState="True" MaxLength="1024" Width="660px"></asp:TextBox>
                            </div>
                            <div style="display: inline-block;">
                                <div style="width: 245px; display: inline-block;">
                                    Capacity
                                </div>
                                <div style="width: 100px; display: inline-block;">
                                    <asp:CheckBox ID="PerPaxCheckBox" runat="server" Checked="true" AutoPostBack="true"  />
                                    Is Per Pax &nbsp;&nbsp;&nbsp;
                                </div>
                                <%--<div style="width: 570px; display: inline-block;">
                                    <label for="MaximumCapacityTextBox">
                                        Maximum Capacity
                                    </label>
                                    <asp:TextBox ID="MaximumCapacityTextBox" runat="server" EnableViewState="True" MaxLength="128"
                                        Width="400px"></asp:TextBox>
                                </div>--%>
                            </div>
                           
                        </fieldset>
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
                       </ContentTemplate>
            
            </asp:UpdatePanel>
                 
                 <fieldset>
                    <gridUC:CustomGridView ID="SuppPointsCustomGridView" runat="server" Width="100%"  />
                </fieldset>
                
            </div>
       </div>
    </div>
</asp:Content>
