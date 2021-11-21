<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessPublic.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="e_Travel.BusinessPublic._default" Theme="BusPublicDefault" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="../Scripts/jQuery/jquery.mCustomScrollbar/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery/jquery.mCustomScrollbar/jquery.mCustomScrollbar.concat.min.js" type="text/javascript"></script>
    <link href="../Scripts/jQuery/FractionSlider/fractionslider.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Scripts/jQuery/FractionSlider/reset.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Scripts/jQuery/FractionSlider/style.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jQuery/FractionSlider/jquery.fractionslider.min.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/FractionSlider/main.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".HomePageBoxHeight").mCustomScrollbar({
                scrollButtons: {
                    enable: true
                },
                theme: "dark-thick"
            });
        });
    </script>
    
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class = "FromBody">
        <div id = "HomePageSearchWrap">
            <fieldset style = "height: 310px">
                <div style = "width: 48%; float: left;">
                    <div class = "HomePageHeadingText">
                        Search your tour package
                    </div>
                    <div class = "rowSpacer"></div>
                    <div class = "rowBold">
                        <label for="DestinationDropDownList">Where do you want to go?</label>
                        <asp:DropDownList ID="DestinationDropDownList" runat="server" AppendDataBoundItems="true" Width="97%">
                            <asp:ListItem Value="0">--- Select Destination ---</asp:ListItem>
                            <asp:ListItem Value="1-3">Gangtok</asp:ListItem>
                            <asp:ListItem Value="4-6">Darjeeling</asp:ListItem>
                            <asp:ListItem Value="7-8">Pelling</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class = "rowSpacer"></div>
                    <div class = "rowBold">
                        <label for="TourDurationDropDownList">Your Tour Duration (Optional)</label>
                        <asp:DropDownList ID="TourDurationDropDownList" runat="server" AppendDataBoundItems="true" Width="97%">
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
                    <div class = "rowSpacer"></div>
                    <div class = "rowBold">
                        <label for="TourTypeDropDownList">Type of Tour (Optional)</label>
                        <asp:DropDownList ID="TourTypeDropDownList" runat="server" AppendDataBoundItems="true" Width="97%">
                            <asp:ListItem Value="0">--- Select Tour Type ---</asp:ListItem>
                            <asp:ListItem Value="1-3">Advanture Tour</asp:ListItem>
                            <asp:ListItem Value="4-6">Religious Tour</asp:ListItem>
                            <asp:ListItem Value="7-8">leisure Tour</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style = "width: 100%; float: left; height: 32px; margin-top: 20px; margin-bottom: 10px; text-align: right;">
                        <div style = "width: 150px; float: right; margin-right: 5px;">
                            <asp:ImageButton id = "SearchTpkImageButton" runat="server" value = "" CssClass="HomePageSearchButton"></asp:ImageButton>
                        </div>
                    </div>
                </div>
                <div class = "HomePageVerticalLine"></div>
                <div style = "width: 50%; float: left;">
                    <div class = "HomePageHeadingText" style = "text-align: center">
                        Plan Your Tour
                    </div>
                    <div class = "HomePageSubHeadingText">
                        (in 3 easy steps)
                    </div>
                    <div class = "rowSpacer"></div>
                    <div class = "TourSteps">
                        <div id = "TourStep1"></div>
                        <div class = "TourStepText">
                            <span class = "TourStepHeading">Search</span><br />
                            Use Search Option to find a tour package
                        </div>
                    </div>
                    <div class = "TourSteps">
                        <div id = "TourStep2"></div>
                        <div class = "TourStepText">
                            <span class = "TourStepHeading">Configure & Book</span><br />
                            Congigure & book tour as per your need
                        </div>
                    </div>
                    <div class = "TourSteps">
                        <div id = "TourStep3"></div>
                        <div class = "TourStepText">
                            <span class = "TourStepHeading">Print & Go</span><br />
                            Print receipt & enjoy your tour
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div id = "HomePageDealWrap">
            <fieldset style = "height: 310px">
                <div class = "HomePageHeadingText" style = "text-align: center; width: 300px; margin-bottom: 15px;">
                    Offers & Deals
                </div>
                <div class="HomePageBoxHeight">
                    <asp:ListView ID="DiscountListView" runat="server" DataKeyNames="TpkID">
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td style = "width: 220px; height: 70px;">
                                        <span style = "width: 220px; font-weight: bold; color: #1FA7F9; font-size: 10pt;">
                                            <%#Eval("TpkName")%>&nbsp;(<%#Eval("DiscountPercent")%>%)
                                        </span>
                                        <span style = "width: 210px; padding-right: 10px; text-align: justify;">
                                            <%#Eval("DiscountDesc")%>
                                        </span>
                                    </td>
                                    <td style = "width: 90px; height: 70px;">
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%#String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("PhotoThumb"))) %>' Width="90px" height="72px" BorderColor="Beige" />
                                    </td>
                                </tr>
                            </table>
                            <hr style = "float: left; color: #FFFFFF; width:100%" />
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </fieldset>
        </div>
        <div id = "HomePageDestinationWrap">
            <div style = "width: 605px; height: 395px; float: left;">
                <div class="slider-wrapper">
			        <div class="responisve-container">
                        <div style = "width: 590px; padding-left: 10px; height: 40px; line-height: 40px; color: #FFFFFF; background-color: #000000; font-size: 12pt; font-weight: bold;">
                            Destinations
                        </div>
				        <div class="slider">
					        <div class="fs_loader"></div>
                            <asp:Literal ID="destLiteral" runat="server"></asp:Literal>
                        </div>
                        <div style = "width: 600px; height: 40px; line-height: 40px; color: #FFFFFF; background-color: #000000; font-size: 12pt; font-weight: bold;">
                            <div style = "width: 520px; height: 40px; line-height: 40px; float: left;">
                            </div>
                            <div style = "width: 80px; height: 40px; float: left; background-color: #1FA7F9; color: #FFFFFF; font-weight: bold; font-size: 11pt; text-align: center;">
                                View All
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id = "HomePageHelpWrap">
            <img src="../Images/TempImages/help.jpg" />
        </div>
        <div style = "clear: both"></div>
    </div>
</asp:Content>
