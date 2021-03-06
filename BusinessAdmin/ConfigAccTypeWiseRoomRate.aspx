<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master" AutoEventWireup="true" CodeBehind="ConfigAccTypeWiseRoomRate.aspx.cs" Inherits="e_Travel.BusinessAdmin.ConfigAccTypeWiseRoomRate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class = "FromBody">
<asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
          <div class = "BoxHeader">Accomodation Type Wise Room Configuration</div>
        <div class = "BoxBody">
        <div class = "BoxContent">
        <fieldset>
         <legend>Accomodation Type Wise Room Configuration</legend>
         <div class="row">
          <label for="AccTypeDropDown" style="width:200px;">Accomodation Type</label>
                            <asp:DropDownList ID="AccTypeDropDown" runat="server" ValidationGroup="ValidateData" Width = "665px" AppendDataBoundItems="True">
                            </asp:DropDownList>
         </div>
            <div class="row">
                 <label for="AccDestinationDropDown" style="width:200px;">Destination</label>
                            <asp:DropDownList ID="DestinationDropDown" runat="server" 
                     ValidationGroup="ValidateData" Width = "665px" AppendDataBoundItems="True">
                            </asp:DropDownList>
               </div>
                <div class="row">
                 <label for="RoomPlanDropDown" style="width:200px;">Room Plan Name</label>
                            <asp:DropDownList ID="RoomPlanDropDown" runat="server" ValidationGroup="ValidateData" Width = "665px" AppendDataBoundItems="True">
                            </asp:DropDownList>
               </div>
                    <div class="row" style="width: 450px; display: inline-block;">
                                <label for="SingleRoomCostPriceTextBox" style="width: 200px;">
                                    Single Room Cost Price</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="SingleRoomCostPriceTextBox" runat="server" MaxLength="20"  Width="200px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="width: 450px; display: inline-block;">
                                <label for="SingleRoomSellPriceTextBox" style="width: 200px;">
                                    Single Room Selling Price</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="SingleRoomSellPriceTextBox" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                             <div class="row" style="width: 450px; display: inline-block;">
                                <label for="DoubleRoomCostPriceTextBox" style="width: 200px;">
                                    Double Room Cost Price</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="DoubleRoomCostPriceTextBox" runat="server" MaxLength="20"  Width="200px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="width: 450px; display: inline-block;">
                                <label for="DoubleRoomSellPriceTextBox" style="width: 200px;">
                                    Double Room Selling Price</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="DoubleRoomSellPriceTextBox" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                              <div class="row" style="width: 450px; display: inline-block;">
                                <label for="ExtraBedCostPriceTextBox" style="width: 200px;">
                                    Extra Bed Cost Price</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="ExtraBedCostPriceTextBox" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                                <div class="row" style="width: 450px; display: inline-block;">
                                <label for="ExtraBedSellPriceTextBox" style="width: 200px;">
                                    Extra Bed Selling Price</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="ExtraBedSellPriceTextBox" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                                <div class="row" style="width: 450px; display: inline-block;">
                                <label for="ExtraChildCostPriceTextBox" style="width: 200px;">
                                    Room Cost Rate For Extra Child</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="ExtraChildCostPriceTextBox" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                </div>
                            </div>
                                    <div class="row" style="width: 450px; display: inline-block;">
                                <label for="ExtraChildSellPriceTextBox" style="width: 200px;">
                                    Room Sell Rate For Extra Child</label>
                                <div style="margin-top: 6px;">
                                    <asp:TextBox ID="ExtraChildSellPriceTextBox" runat="server" MaxLength="20" Width="200px"></asp:TextBox>
                                </div>
                            </div>
         </fieldset>
              <fieldset>
                <div class="FromBodyButtonContainer">
                  <asp:Button ID="AddButton" runat="server" Text=" Add " CssClass="FromBodyButton" 
                        Width="80px" onclick="AddButton_Click"/>
                   <asp:Button ID="EditButton" runat="server" Text=" Edit " 
                        CssClass="FromBodyButton" Width="80px" onclick="EditButton_Click"/>
                   <asp:Button ID="SaveButton" runat="server" Text=" Save " 
                        CssClass="FromBodyButton" ValidationGroup="ValidateData" Width="80px" 
                        onclick="SaveButton_Click"/>
                  <asp:Button ID="ClearButton" runat="server" Text=" Clear " 
                        CssClass="FromBodyButton" Width="80px" onclick="ClearButton_Click"/>
                 </div>
         </fieldset>
         <div id="ClearDIV" style="clear:both;"></div>
          <fieldset>
                    <gridUC:CustomGridView ID="ETravelCustomGridView" runat="server" Width="100%" />
                </fieldset>
        </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
</div>
</asp:Content>
