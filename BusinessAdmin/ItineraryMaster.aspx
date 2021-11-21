<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="ItineraryMaster.aspx.cs" Inherits="e_Travel.BusinessAdmin.ItineraryMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #VehicleListDIV, #OtherCostDiv
        {
            border: 1px solid #CCCCCC;
            border-radius: 5px 5px 5px 5px;
            float: left;
            width: 685px;
        }
        .GridStyle
        {
            border-width: 1px;
        }
        
        .GridStyle tr td
        {
            border-bottom: 20px solid none;
        }
        .gridHeader
        {
            margin-bottom: 20px; /*height: 30px;*/
        }
        .headerApplicable
        {
            float: left;
        }
        .accordion
        {
            width: 100%;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: #D3DEEF;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            Itinerary Master
        </div>
        <div class="BoxBody">
            <div class="BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <ajaxToolkit:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
                            HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                            runat="server">
                            <Panes>
                                <ajaxToolkit:AccordionPane runat="server">
                                    <Header>
                                        Itineary Basic Details
                                    </Header>
                                    <Content>
                                        <fieldset>
                                            <div class="row">
                                                <label for="ItineraryHeadingTextBox">
                                                    Itinerary Heading
                                                </label>
                                                <asp:TextBox ID="ItineraryHeadingTextBox" runat="server" TextMode="SingleLine" MaxLength="256"
                                                    Width="668px"></asp:TextBox>
                                            </div>
                                            <div class="row">
                                                <label for="ItineraryDetailTextBox">
                                                    Itinerary Detail
                                                </label>
                                                <asp:TextBox ID="ItineraryDetailTextBox" runat="server" TextMode="MultiLine" MaxLength="2048"
                                                    Width="668px" Height="150px"></asp:TextBox>
                                            </div>
                                            <div style="height: 7px;">
                                            </div>
                                            <div class="row" style="width: 450px; display: inline-block;">
                                                <label for="YesRAPApplicaleRadioButton" style="width: 230px;">
                                                    Is RAP Applicable?</label>
                                                <div style="margin-top: 6px;">
                                                    <asp:RadioButton ID="YesRAPApplicableRadioButton" runat="server" Text="" GroupName="RAPApplicableGroup"
                                                        AutoPostBack="true" OnCheckedChanged="YesRAPApplicableRadioButton_CheckedChanged" />Yes
                                                    <asp:RadioButton ID="NoRAPApplicableRadioButton" runat="server" GroupName="RAPApplicableGroup"
                                                        AutoPostBack="true" OnCheckedChanged="NoRAPApplicableRadioButton_CheckedChanged" />No
                                                </div>
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <asp:Panel ID="RAPPanel" runat="server" Visible="false">
                                                <div style="margin-left: 26%; border: 1px solid #CCCCCC; border-radius: 5px; padding: 10px;">
                                                    <asp:CheckBox ID="RAPPerPaxCheckBox" runat="server" Checked="true" />
                                                    Is Per Pax &nbsp;&nbsp;&nbsp; Cost Price
                                                    <asp:TextBox ID="RAPCostPriceTextBox" runat="server" TextMode="SingleLine" MaxLength="256"
                                                        Width="170px"></asp:TextBox>
                                                    &nbsp;&nbsp;&nbsp; Sell Price
                                                    <asp:TextBox ID="RAPSellPriceTextBox" runat="server" TextMode="SingleLine" MaxLength="256"
                                                        Width="170px"></asp:TextBox>
                                                </div>
                                            </asp:Panel>
                                            <div style="clear: both;">
                                            </div>
                                            <div class="row" style="width: 450px; display: inline-block; clear: both;">
                                                <label for="YesForeignerAllowedRadioButton" style="width: 230px;">
                                                    Is Foreigner Allowed?</label>
                                                <div style="margin-top: 6px;">
                                                    <asp:RadioButton ID="YesForeignerAllowedRadioButton" runat="server" Text="" GroupName="FoeignerAllowedRequiredGroup" />Yes
                                                    <asp:RadioButton ID="NoForeignerAllowedRadioButton" runat="server" Text="" GroupName="FoeignerAllowedRequiredGroup" />No
                                                </div>
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                            <div class="row" style="width: 450px; display: inline-block;">
                                                <label for="YesBothRadioButton" style="width: 350px;">
                                                    Does Package Include Transfer As Well As Sight Seen</label>
                                                <div style="margin-top: 6px;">
                                                    <asp:RadioButton ID="YesBothRadioButton" runat="server" Text="" GroupName="BothRequiredGroup"
                                                        AutoPostBack="true" OnCheckedChanged="YesBothRadioButton_CheckedChanged" />Yes
                                                    <asp:RadioButton ID="NoBothRadioButton" runat="server" Text="" GroupName="BothRequiredGroup"
                                                        OnCheckedChanged="NoBothRadioButton_CheckedChanged" AutoPostBack="true" />No
                                                </div>
                                            </div>
                                        </fieldset>
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                                <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                                    <Header>
                                         Itinerary Vehicle Cost</Header>
                                    <Content>
                                        <fieldset>
                                            <asp:Panel ID="VehicleDtlsPanel" runat="server" Visible="True">
                                                <span style = "font-size: 14pt; font-weight: bold;">Vehicle Type Wise Itinerary Cost</span>
                                                <hr style = "width: 920px; color: #FFFFFF; margin-top: 10px; margin-bottom: 15px" />
                                                <div id="VehicleDtlsDiv" class="row" style="float: left; width: 920px">
                                                    <asp:GridView ID="VehicleDtlsGridView" runat="server" AutoGenerateColumns="False"
                                                        GridLines="None" HeaderStyle-CssClass="gridHeader">
                                                        <RowStyle Height = "30px" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 200px; border-bottom: 1px dotted #797a7b">
                                                                        Vehicle Type
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 200px;">
                                                                        <div style="display: none;">
                                                                            <asp:Label ID="VehicleTypeIDLabel" runat="server" Text='<%#Eval("VehicleTypeID")%>'></asp:Label>
                                                                        </div>
                                                                        <div style="text-align: left; line-height: 25px;">
                                                                            <asp:CheckBox ID="IsApplicableCheckBox" runat="server" Enabled="true" Checked='<%#this.IsApplicable(Convert.ToString(Eval("IsApplicable")))%>'
                                                                                AutoPostBack="true" OnCheckedChanged = "IsApplicableCheckBox_CheckedChanged" style = "margin-top: 3px" />
                                                                            <asp:Label ID="VehicleNameLabel" runat="server" Text='<%#Eval("VehicleTypeName") %>'
                                                                                Width="200px"></asp:Label>
                                                                        </div>    
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 100px; border-bottom: 1px dotted #797a7b">
                                                                        Cost Price
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 100px;">
                                                                        <asp:TextBox ID="CostPriceTextBox" runat="server" Width="80px" Text='<%#Eval("ItineraryCostPrice") %>'></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 100px; border-bottom: 1px dotted #797a7b">
                                                                        Selling Price
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 100px;">
                                                                        <asp:TextBox ID="SellPriceTextBox" runat="server" Width="80px" Style="margin-right: 10px;"
                                                                            Text='<%#Eval("ItinerarySellPrice") %>'></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 520px; border-bottom: 1px dotted #797a7b">
                                                                        Remark/Note
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 520px;">
                                                                        <asp:TextBox ID="NoteTextBox" runat="server" Text='<%#Eval("VehicleNote") %>' width = "510px"></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="VehicleDtlsTransferPanel" runat="server" Visible="True">
                                                <span style = "font-size: 13pt; font-weight: bold;">Itinerary Vehicle Cost Breakup (Transfer Cost)</span>
                                                <hr style = "width: 920px; color: #FFFFFF; margin-top: 10px; margin-bottom: 15px" />
                                                <div id="VehicleTransferDIV" class="row" style="float: left; width: 920px">
                                                    <asp:GridView ID="VehicleTransferGridView" runat="server" AutoGenerateColumns="False"
                                                        CssClass="GridStyle" GridLines="None" HeaderStyle-CssClass="gridHeader">
                                                        <RowStyle Height = "30px" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 200px; border-bottom: 1px dotted #797a7b">
                                                                        Vehicle Type
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 200px;">
                                                                        <div style="display: none;">
                                                                            <asp:Label ID="VehicleTypeIDLabel" runat="server" Text='<%#Eval("VehicleTypeID")%>'></asp:Label>
                                                                        </div>
                                                                        <div style="text-align: left; line-height: 25px;">
                                                                            <asp:CheckBox ID="IsTransferApplicableCheckBox" runat="server" Enabled="true" Checked='<%#this.IsApplicable(Convert.ToString(Eval("IsApplicable")))%>'
                                                                                AutoPostBack="true" OnCheckedChanged="IsTransferApplicableCheckBox_CheckedChanged" style = "margin-top: 3px"/>
                                                                            <asp:Label ID="VehicleNameLabel" runat="server" Text='<%#Eval("VehicleTypeName") %>'
                                                                                Width="200px"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 100px; border-bottom: 1px dotted #797a7b">
                                                                        Cost Price
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 100px;">
                                                                        <asp:TextBox ID="CostPriceTextBox" runat="server" Width="80px" Style="margin-right: 10px;"
                                                                            Text='<%#Eval("ItineraryCostPrice") %>'></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 100px; border-bottom: 1px dotted #797a7b">
                                                                        Selling Price
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 100px;">
                                                                        <asp:TextBox ID="SellPriceTextBox" runat="server" Width="80px" Style="margin-right: 10px;"
                                                                            Text='<%#Eval("ItinerarySellPrice") %>'></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 520px; border-bottom: 1px dotted #797a7b">
                                                                        Remark/Note
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 520px;">
                                                                        <asp:TextBox ID="NoteTextBox" runat="server" Text='<%#Eval("VehicleNote") %>' width = "510px"></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="headerApplicable" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </asp:Panel>
                                            <div style="clear: both;">
                                            </div>
                                            <asp:Panel ID="VehicleDtlsSightSeenPanel" runat="server" Visible="True">
                                                <hr style = "width: 920px; color: #FFFFFF; margin-top: 20px; margin-bottom: 5px" />
                                                <span style = "font-size: 13pt; font-weight: bold;">Itinerary Vehicle Cost Breakup (Sightseen Cost)</span>
                                                <hr style = "width: 920px; color: #FFFFFF; margin-top: 5px; margin-bottom: 15px" />
                                                <div id="VehicleSightSeenDIV" class="row" style="float: left; width: 920px">
                                                    <asp:GridView ID="VehicleSightSeenGridView" runat="server" AutoGenerateColumns="False"
                                                        CssClass="GridStyle" GridLines="None" HeaderStyle-CssClass="gridHeader">
                                                        <RowStyle Height = "30px" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 200px; border-bottom: 1px dotted #797a7b">
                                                                        Vehicle Type
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 200px;">
                                                                        <div style="display: none;">
                                                                            <asp:Label ID="VehicleTypeIDLabel" runat="server" Text='<%#Eval("VehicleTypeID")%>'></asp:Label>
                                                                        </div>
                                                                        <div style="text-align: left; line-height: 25px;">
                                                                            <asp:CheckBox ID="IsSightSeenApplicableCheckBox" runat="server" Enabled="true" Checked="true"
                                                                                AutoPostBack="true" OnCheckedChanged="IsSightSeenApplicableCheckBox_CheckedChanged" style = "margin-top: 3px" />
                                                                            <asp:Label ID="VehicleNameLabel" runat="server" Text='<%#Eval("VehicleTypeName") %>'
                                                                                Width="200px"></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 100px; border-bottom: 1px dotted #797a7b">
                                                                        Cost Price
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 100px;">
                                                                        <asp:TextBox ID="CostPriceTextBox" runat="server" Width="80px" Text='<%#Eval("ItineraryCostPrice") %>'></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 100px; border-bottom: 1px dotted #797a7b">
                                                                        Selling Price
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 100px;">
                                                                        <asp:TextBox ID="SellPriceTextBox" runat="server" Width="80px" Text='<%#Eval("ItinerarySellPrice") %>'></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <div style="text-align: left; line-height: 20px; float: left; width: 520px; border-bottom: 1px dotted #797a7b">
                                                                        Remark/Note
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div style="text-align: left; line-height: 35px; margin-top: 10px; float: left; width: 520px;">
                                                                        <asp:TextBox ID="NoteTextBox" runat="server" Text='<%#Eval("VehicleNote") %>' width = "510px"></asp:TextBox>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </asp:Panel>
                                        </fieldset>
                                    </Content>
                                </ajaxToolkit:AccordionPane>
                            </Panes>
                        </ajaxToolkit:Accordion>
                        <div style="clear: both; height: 10px;">
                        </div>
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
                    <gridUC:CustomGridView ID="ItineraryCustomGridView" runat="server" Width="100%" />
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
