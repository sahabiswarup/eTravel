<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master" AutoEventWireup="true" CodeBehind="SearchTpk.aspx.cs" Inherits="e_Travel.BusinessAdmin.SearchTpk" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class = "FromBody">
        <div class = "BoxHeader">Search Packages</div>
        <div class = "BoxBody">
            <div class = "BoxContent">
                <div class="leftCol" style="min-height:300px;">
                    <div class="searchTourPackage">
                        <h3>Search Tour Package</h3>
                        <div class="searchform_innerTourPackage">
                            <div style="padding:5px; float:left; width:246px;">
                            <div style="width: 100%; padding: 0; margin: 0">
                          <asp:CheckBox ID="ForeignerCheckBox" runat="server" />
                                     <%--<asp:RadioButton ID="IndianNationalitydRadioButton" runat="server" Text="" GroupName="NationalityRequiredGroup" Checked="true"  Visible="false"/>--%>
                                                    <%--<asp:RadioButton ID="NonIndianNationalityRadioButton" runat="server" Text="" GroupName="NationalityRequiredGroup" />--%>
                                
                                    <label for="DestinationDropDown" style = "width:200px;margin-top:0px;float:right">NRI or Foreign National</label>                            
                                </div>
                                
                                <div style="width: 100%; padding: 0; margin: 0">
                                    <label for="DestinationDropDown" style = "width:246px;">Destination</label>                            
                                </div>
                                <div style="margin-bottom:10px; width: 100%;">
                                    <asp:DropDownList ID="DestinationDropDown" runat="server" ValidationGroup="ValidateData" Width = "246px" AppendDataBoundItems="True"></asp:DropDownList>
                                </div>
                                <div style="width: 100%; padding: 0; margin: 0">
                                    <label for="PackageTypeDropDown" style = "width: 246px">Holiday/Package Type</label>
                                </div>
                                <div style="margin-bottom:10px; width: 100%;"> 
                                    <asp:DropDownList ID="PackageTypeDropDown" runat="server"  AutoPostBack="false" ValidationGroup="ValidateData" Width = "246px" AppendDataBoundItems="True">
                                    </asp:DropDownList>
                                </div>
                                <div style="width: 100%; padding: 0; margin: 0">
                                    <label for="TourDurationDropDown" style = "width: 246px;">Tour Duration</label>                      
                                </div>
                                <div style="margin-bottom:10px; width: 100%;">
                                    <asp:DropDownList ID="TourDurationDropDown" runat="server" ValidationGroup="ValidateData" Width = "246px">
                                        <asp:ListItem Value="0">--- Select Tour Duration ---</asp:ListItem>
                                        <asp:ListItem Value="1-3">1 to 3 Days</asp:ListItem>
                                        <asp:ListItem Value="4-6">4 to 6 Days</asp:ListItem>
                                        <asp:ListItem Value="7-8">7 to 8 Days</asp:ListItem>
                                        <asp:ListItem Value="9-10">9 to 10 Days</asp:ListItem>
                                        <asp:ListItem Value="11-12">11 to 12 Days</asp:ListItem>
                                        <asp:ListItem Value="13-14">13 to 14 Days</asp:ListItem>
                                        <asp:ListItem Value="15-500">Above 15 Days</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <hr style = "width: 246px"/>
                                <div style="margin-bottom:10px; margin-top: 10px; text-align: right; padding-right: 5px; width: 100%">
                                    <asp:Button ID="SearchPackage" runat="server" Text="Search Package" CssClass="FromBodyButton" onclick="SearchPackage_Click" />
                                </div>
                            </div>
                            <div style = "clear: both"></div>
                        </div>
                    </div>
                </div>
                <div class="rightCol">
                    <fieldset style="padding: 0; margin-bottom: 12px; float:left; overflow: hidden; width: 648px;">
                        <asp:GridView ID="GridViewPackage" runat="server" AutoGenerateColumns="False" GridLines="None"
                            BorderWidth="0px" DataKeyNames="TpkID" 
                            onrowcommand="GridViewPackage_RowCommand" AllowPaging="True" 
                            onpageindexchanging="GridViewPackage_PageIndexChanging" 
                            ondatabound="GridViewPackage_DataBound" PageSize="5" 
                            AlternatingRowStyle-BackColor="#ECECEC" ShowHeader="False" >
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <table style = "margin: 10px; width: 628px;" cellpadding= "0" cellspacing = "0" >
                                            <tr>
                                                <td rowspan="4" style = "width: 130px;">
                                                    <div style= "float: left; height:100px; width:130px; border:5px solid #BCC7D8; border-radius:5px 5px 5px 5px; ">
                                                        <a href="#">
                                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%#String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("PhotoThumb"))) %>' Height="100px" Width="130px" BorderColor="Beige" />
                                                        </a>
                                                    </div>
                                                </td>
                                                <td style = "font-size: 12pt; font-weight: bold; color: #EE7B38; padding-bottom: 10px; padding-left: 20px; height: 22px;">
                                                    <asp:Label ID="PackageNameLabel" runat="server" Text='<%# Eval("TpkName") %>'/>
                                                    &nbsp;(<asp:Label ID="Label4" runat="server" Text='<%# Eval("TpkID") %>'/>)
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style = "padding-left: 20px; font-size: 10pt; font-weight: bold; color: #484848;">
                                                    Tour Duration: <asp:Label ID="Label2" runat="server" Text='<%# Eval("TotalDays") %>'/> Days/<asp:Label ID="Label3" runat="server" Text='<%# Eval("TotalNights") %>'/> Nights
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style = "padding-left: 20px; color: #484848;">
                                                 <b>   Destination Covered:</b> <asp:Label ID="Label1" runat="server" Text='<%# Eval("TpkDestCovered") %>'/>
                                                    <div style="clear:both;height:10px"></div>
                                                      <asp:Label ID="Label20" runat="server" Text='<%#Eval("TpkDesc") %>'></asp:Label>
                                                </td>
                                            </tr>
                                           
                                            <tr>
                                                <td style = "padding-left: 20px; text-align: right;">
                                                    <asp:Button ID="ViewPackage" runat="server" Text="View Details" CssClass="FromBodyButton"  CommandName="ViewDetails" CommandArgument='<%#Eval("TpkID")%>' />
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate> 
                                <div style = "text-align: center; margin-top: 20px; background-color: Gray;">
                                    <div style="float: left; margin-right: 10px;margin-left: 28%;">
                                        <asp:ImageButton ID="btnFirst" runat="server" CommandArgument="First" CommandName="Page"
                                            OnCommand="ManagePaging" AlternateText="First" Width="24px" Height="24px" ImageUrl="~/Images/paging_MoveFirst.png"
                                            ToolTip="Move First" />
                                        <asp:ImageButton ID="btnPrev" runat="server" CommandArgument="Prev" CommandName="Page"
                                            OnCommand="ManagePaging" AlternateText="Previous" Width="24px" Height="24px"
                                            ImageUrl="~/Images/paging_left.png" ToolTip="Move Previous" />
                                    </div>
                                    <div style="float: left; margin-right: 10px; margin-bottom: 10px; height: 20px; color:Black;">
                                        Page
                                        <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged" Height="32px"
                                            CssClass="pagerComboStyle">
                                        </asp:DropDownList>
                                        of
                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                    </div>
                                    <div style="float: left;">
                                        <asp:ImageButton ID="btnNext" runat="server" CommandArgument="Next" CommandName="Page"
                                            OnCommand="ManagePaging" AlternateText="Next" Width="24px" Height="24px" ImageUrl="~/Images/paging_right.png"
                                            ToolTip="Move Next" />
                                        <asp:ImageButton ID="btnLast" runat="server" CommandArgument="Last" CommandName="Page"
                                            OnCommand="ManagePaging" AlternateText="Last" Width="24px" Height="24px" ImageUrl="~/Images/paging_MoveLast.png"
                                            ToolTip="Move Last" />
                                    </div>
                                </div>
                            </PagerTemplate>   
                        </asp:GridView> 
                        <div style="height:20px;float:right; text-align: Right; margin-right: 10px; width:96%; color: #EE7B38;">
                            <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
                        </div>       
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
    <asp:HiddenField runat="server" id = "PageIndexHiddenField" Value="0"></asp:HiddenField>
    <asp:HiddenField runat="server" id = "TotalPageCount"></asp:HiddenField>
</asp:Content>
