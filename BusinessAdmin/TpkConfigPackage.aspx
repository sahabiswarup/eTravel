<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="TpkConfigPackage.aspx.cs" Inherits="e_Travel.BusinessAdmin.TpkConfigPackage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <script src="../Scripts/JqueyUI/jquery-1.9.1.js" type="text/javascript"></script>--%>
    <link href="../Scripts/JqueyUI/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/JqueyUI/jquery-ui.js" type="text/javascript"></script>
    <script src="../Scripts/jQuery/ajaxfileupload.js" type="text/javascript"></script>
    <style type="text/css">
        .grid .NormalRow
        {
            background-color: #FFFFFF;
        }
        .grid .SelectRow
        {
            font-weight: bold;
            background: #fee2aa;
        }
        .grid .alt
        {
            background-color: #ececec;
        }
        .grid .NormalRow:hover, .grid .ucGridView tbody tr.alt:hover
        {
            background-color: #FEE2AA;
            color: black;
            font-weight: bold;
        }
        .grid .header
        {
            background-color: #A4ABB2;
            color: #3B3B3B;
            height: 35px;
            padding-left: 8px;
            text-align: left;
        }
        .grid td
        {
            border: 1px solid #A4ABB2;
        }
        .grid .header th
        {
            border: 1px solid #A4ABB2;
        }
        .grid
        {
            width: 100%;
        }
        .clear
        {
            height: 10px;
            clear: both;
        }
    </style>
    <script type="text/javascript">

    
         // Determine how much the visitor had scrolled brief popup
         function fireHotelBriefPopup() {
             var scrolledX, scrolledY;
             if (self.pageYOffset) {
                 scrolledX = self.pageXOffset;
                 scrolledY = self.pageYOffset;
             } else if (document.documentElement && document.documentElement.scrollTop) {
                 scrolledX = document.documentElement.scrollLeft;
                 scrolledY = document.documentElement.scrollTop;
             } else if (document.body) {
                 scrolledX = document.body.scrollLeft;
                 scrolledY = document.body.scrollTop;
             }

             // Determine the coordinates of the center of the page

             var centerX, centerY;
             if (self.innerHeight) {
                 centerX = self.innerWidth;
                 centerY = self.innerHeight;
             } else if (document.documentElement && document.documentElement.clientHeight) {
                 centerX = document.documentElement.clientWidth;
                 centerY = document.documentElement.clientHeight;
             } else if (document.body) {
                 centerX = document.body.clientWidth;
                 centerY = document.body.clientHeight;
             }
             var leftOffset = scrolledX + (centerX - 500) / 2;
             var topOffset = scrolledY + (centerY - 300) / 2;

             document.getElementById("ImageCropperPopup").style.top = topOffset + "px";
             document.getElementById("ImageCropperPopup").style.left = leftOffset + "px";
             document.getElementById("ImageCropperPopup").style.display = "block";
             //  document.getElementById("hotelBrief").style.overflow = "auto";

         }


         /// Dependant on ~/Scripts/ajaxfileupload.js

         function ajaxFileUpload() {
             $("#FileLoading").ajaxStart(function () {
                 $(this).show();
             }).ajaxComplete(function () {
                 $(this).hide();
             });

             $.ajaxFileUpload({
                 url: '<%=ResolveClientUrl("~/WebHandler/AjaxFileUploader.ashx")%>',
                 secureuri: false,
                 //  fileElementId: 'photographToUpload',
                 //fileElementId: $('[id$=fupActivityImage]'),
                 fileElementId: 'fupActivityImage',
                 dataType: 'json',
                 data: { name: 'logan', id: 'id' },
                 success: function (data, status) {
                     if (typeof (data.error) != 'undefined') {
                         if (data.error != '') {
                             alert(data.error);
                         } else {
                             //  alert(data.msg);
                             $('[id$=cropImageTarget]').attr('src', '<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx")%>' + '?T=' + new Date().getTime());
                             $('.jcrop-holder img').attr('src', '<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx")%>' + '?T=' + new Date().getTime());
                             //console.log($('[id$=cropImageTarget]').width() + ' ' + $('[id$=cropImageTarget]').width());
                             $('[id$=cropImageTarget]').Jcrop({
                                 onChange: setCoords,
                                 onSelect: setCoords,
                                 onRelease: clearCoords,
                                 setSelect: [0, 0, $('[id$=cropImageTarget]').width() - 40, ($('[id$=cropImageTarget]').width() * (3 / 4)) - 30],
                                 aspectRatio: 10 / 8
                             });

                             fireHotelBriefPopup();

                             //$('[id$=imgThumb]').attr('src', '<%=ResolveClientUrl("~/UserControls/PhotoGallery/DisplayCropedThumbImage.ashx?")%>cropImageX1=' + $('[id$=cropImageX1]').val() + ' &cropImageY1=' + $('[id$=cropImageY1]').val() + ' &cropImageW=' + $('[id$=cropImageW]').val() + ' &cropImageH=' + $('[id$=cropImageH]').val());
                             $("[id$=btnClearImage]").show();
                             $("#disablelayer").show();

                         }
                     }
                 },
                 error: function (data, status, e) {
                     alert(e);
                 }
             }
    )

             return false;
         }

         function validatePhotographToUpload() {
             var uploadcontrol = $('[id$=fupActivityImage]').val();
             uploadcontrol = uploadcontrol.substring(uploadcontrol.lastIndexOf('\\') + 1);
             //alert(uploadcontrol.lastIndexOf('\\'));
             //Regular Expression for fileupload control.
             var reg = /^[a-zA-Z0-9-_ \.]+\.(pjpg|PJPG|jpg|JPG|jpeg|JPEG|gif|GIF|png|PNG)$/;
             //  var uploadcontrol = $('[id$=fupActivityImage]').val();
             //  uploadcontrol = uploadcontrol.substring(uploadcontrol.lastIndexOf('\\') + 1);

             //Regular Expression for fileupload control.
             //   var reg = /^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.pjpg|.PJPG|.jpg|.JPG|.jpeg|.JPEG|.gif|.GIF|.png|.PNG)$/;
             if (uploadcontrol.length > 0) {
                 //Checks with the control value.
                 if (reg.test(uploadcontrol)) {
                     //            // Call Upload Handler
                     ajaxFileUpload();
                 }
                 else {
                     //            //If the condition not satisfied shows error message.
                     alert("Only .jpg or .JPG, .pjpg or .PJPG, .jpeg or .JPEG, .gif or .GIF and .png or .PNG files are allowed!");
                     return false;
                 }
             }
         } //End of function validate.
   

        function ShowThumbImage() {
            $('[id$=divImageThumb]').show();
            $('[id$=divImageUpload]').hide();
            $('[id$=imgSampleThumbImage]').attr('src', '<%=ResolveClientUrl("~/WebHandler/DisplayCropedThumbImage.ashx?")%>cropImageX1=' + $('[id$=cropImageX1]').val() + ' &cropImageY1=' + $('[id$=cropImageY1]').val() + ' &cropImageW=' + $('[id$=cropImageW]').val() + ' &cropImageH=' + $('[id$=cropImageH]').val());

        };
        function HideThumbImage() {
            $('[id$=divImageThumb]').hide();
            $('[id$=divImageUpload]').show();
        };
        function findIncompletePackage() {
            $(".ucGridView td:nth-child(7)").each(function (index, value) {
                var percent = $(value).text();
                if (percent != '100') {
                    $($(value).parent()).css('color', 'red');
                    $($(value).parent()).css('font-weight', 'bold');
                }
            });

        }
    </script>
    <script type="text/javascript">

     
        function pageLoad(sender, args) {
            findIncompletePackage();

            if ($('[id$=ItineraryListHiddenField]').val() != "") {
                var projects = $.parseJSON($('[id$=ItineraryListHiddenField]').val());
                if ($('[id$=TpkActivityForDayTextBox]').autocomplete({
                    minLength: 0,
                    source: projects,
                    focus: function (event, ui) {
                        $('[id$=TpkActivityForDayTextBox]').val(ui.item.label);
                        return false;
                    },
                    select: function (event, ui) {
                        $('[id$=TpkActivityForDayTextBox]').val(ui.item.label);
                        $('[id$=ItineraryDetailTextBox]').val(ui.item.ItineraryDetail);
                        $('[id$=SelectedItineraryHiddenField]').val(ui.item.value);

                        return false;
                    }
                })
                .data("ui-autocomplete") != null) {
                    $('[id$=TpkActivityForDayTextBox]').autocomplete({
                        minLength: 0,
                        source: projects,
                        focus: function (event, ui) {
                            $('[id$=TpkActivityForDayTextBox]').val(ui.item.label);
                            return false;
                        },
                        select: function (event, ui) {
                            $('[id$=TpkActivityForDayTextBox]').val(ui.item.label);
                            $('[id$=ItineraryDetailTextBox]').val(ui.item.ItineraryDetail);
                            $('[id$=SelectedItineraryHiddenField]').val(ui.item.value);
                            return false;
                        }
                    })._renderItem = function (ul, item) {
                        return $("<li>")
                .append("<a>" + item.label + "<br>" + "</a>")
                 .appendTo(ul);
                    };
                }



            }
        }
        $(function () {
            var projects = $.parseJSON($('[id$=ItineraryListHiddenField]').val());
            $('[id$=TpkActivityForDayTextBox]').autocomplete({
                minLength: 0,
                source: projects,
                focus: function (event, ui) {
                    $('[id$=TpkActivityForDayTextBox]').val(ui.item.label);
                    return false;
                },
                select: function (event, ui) {
                    $('[id$=TpkActivityForDayTextBox]').val(ui.item.label);
                    $('[id$=SelectedItineraryHiddenField]').val(ui.item.value);
                    return false;
                }
            })
            .data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
            .append("<a>" + item.label + "<br>" + "</a>")
            .appendTo(ul);
            };
        });
    </script>
    <style type="text/css">
        .ui-autocomplete
        {
            width: 670px !important;
            max-height: 200px;
            overflow-y: scroll;
        }
        .ui-autocomplete li:nth-child(even)
        {
            background-color: #e7ebf2;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            Tour Package Configuration</div>
        <asp:UpdatePanel ID="BoxBodyUpdatePanel" runat="server">
            <ContentTemplate>
                <div class="BoxBody">
                    <div class="BoxContent">
                        <asp:Panel ID="SearchPanel" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <fieldset>
                                        <legend>Search Tour Package</legend>
                                        <div style="width: 100%; float: left; margin-bottom: 5px;">
                                            <div style="width: 200px; float: left">
                                                Package Code
                                            </div>
                                            <div style="width: 330px; float: left">
                                                Package Name
                                            </div>
                                            <div style="width: 400px; float: left">
                                                Package Type
                                            </div>
                                        </div>
                                        <div style="width: 100%; float: left;">
                                            <div style="width: 200px; float: left">
                                                <asp:TextBox ID="TpkIDSearchTextBox" runat="server" ValidationGroup="ValidateData"
                                                    TextMode="SingleLine" EnableViewState="False" MaxLength="128" Width="180px"></asp:TextBox>
                                            </div>
                                            <div style="width: 330px; float: left">
                                                <asp:TextBox ID="TpkSearchNameTextBox" runat="server" ValidationGroup="ValidateData"
                                                    TextMode="SingleLine" EnableViewState="False" MaxLength="128" Width="310px"></asp:TextBox>
                                            </div>
                                            <div style="width: 300px; float: left">
                                                <asp:DropDownList ID="TpkTypeSearchDropDownList" runat="server" ValidationGroup="ValidateData"
                                                    AppendDataBoundItems="True" Width="280px">
                                                </asp:DropDownList>
                                            </div>
                                            <div style="width: 100px; float: left;">
                                                <asp:Button ID="SearchButton" runat="server" Text=" Search " CssClass="FromBodyButton"
                                                    ValidationGroup="ValidateData" Width="80px" OnClick="SearchButton_Click" />
                                            </div>
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <div class="row">
                                            <gridUC:CustomGridView ID="TpkGridView" runat="server" Width="100%" />
                                        </div>
                                    </fieldset>
                                    <fieldset>
                                        <div style="display: inline-block; width: 300px; font-weight: bold;">
                                            Complete Package
                                            <asp:LinkButton ID="CompletePackageLinkButton" runat="server" ForeColor="Red" OnClick="CompletePackageLinkButton_Click"></asp:LinkButton>
                                            InComplete Package
                                            <asp:LinkButton ID="InCompletePackageLinkButton" runat="server" ForeColor="Red" OnClick="InCompletePackageLinkButton_Click"></asp:LinkButton>
                                            <asp:LinkButton ID="ViewAllLinkButton" runat="server" Text="Remove Filter" OnClick="ViewAllLinkButton_Click"></asp:LinkButton>
                                        </div>
                                        <div class="FromBodyButtonContainer" style="display: inline-block; width: 380px">
                                            <asp:Button ID="AddButton" runat="server" Text=" Add New Package " CssClass="FromBodyButton"
                                                OnClick="AddButton_Click" />
                                            <asp:Button ID="EditButton" runat="server" Text=" Edit Selected package " CssClass="FromBodyButton"
                                                OnClick="EditButton_Click" />
                                        </div>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                        <asp:Panel ID="TpkWizardPanel" runat="server">
                            <asp:Wizard ID="Wizard1" runat="server" Width="100%" DisplaySideBar="False" OnNextButtonClick="Wizard1_NextButtonClick"
                                ActiveStepIndex="0" OnPreviousButtonClick="Wizard1_PreviousButtonClick" StartNextButtonStyle-CssClass="StepButtonStyle" OnFinishButtonClick="Wizard1_FinishButtonClick">
                                <WizardSteps>
                                    <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1" StepType="Auto">
                                        <div style="width: 100%; float: left; margin-top: 5px; margin-bottom: 5px; height: 30px;">
                                            <span style="font-size: 14px; font-weight: bold; color: Red;">Step 1 of 3</span>
                                            <span style="font-size: 11px; font-weight: bold; color: Red; float: right;">
                                                <asp:LinkButton ID="Step1BackLinkButton" runat="server" Text="Back To Packege List"
                                                    OnClick="BackLinkButton_Click"></asp:LinkButton></span>
                                        </div>
                                        <fieldset>
                                            <legend>Tour Package Details</legend>
                                            <asp:UpdatePanel ID="StepOneUpdatePanel" runat="server">
                                                <ContentTemplate>
                                                    <div class="ContentWrapper">
                                                        <div class="row">
                                                            <label for="TpkNameTextBox">
                                                                Tour Package Name</label>
                                                            <asp:TextBox ID="TpkNameTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"
                                                                MaxLength="128" Width="72%"></asp:TextBox>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkTypeDropDownList">
                                                                Tour Package Type</label>
                                                            <asp:DropDownList ID="TpkTypeDropDownList" runat="server" ValidationGroup="ValidateData"
                                                                Width="73%" AppendDataBoundItems="True">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkTypeDropDownList">
                                                                Travel Segment</label>
                                                            <asp:DropDownList ID="TravelSegmentDropDownList" runat="server" ValidationGroup="ValidateData"
                                                                Width="73%" AppendDataBoundItems="True">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkDescTextBox">
                                                                Tour Package Description</label>
                                                            <asp:TextBox ID="TpkDescTextBox" runat="server" ValidationGroup="ValidateData" TextMode="MultiLine"
                                                                Width="72%"></asp:TextBox>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkDestCoveredTextBox">
                                                                Destination Covered</label>
                                                            <asp:TextBox ID="TpkDestCoveredTextBox" runat="server" ValidationGroup="ValidateData"
                                                                TextMode="MultiLine" Width="72%" MaxLength="1024"></asp:TextBox>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkPickUpTextBox">
                                                                Pickup Point</label>
                                                            <asp:TextBox ID="TpkPickUpTextBox" runat="server" ValidationGroup="ValidateData"
                                                                TextMode="SingleLine" MaxLength="256" Width="72%"></asp:TextBox>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkDropTextBox">
                                                                Drop Point</label>
                                                            <asp:TextBox ID="TpkDropTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"
                                                                MaxLength="256" Width="72%"></asp:TextBox>
                                                        </div>
                                                        <div class="row">
                                                            <label for="TpkDaysTextBox">
                                                                Number of Days</label>
                                                            <asp:TextBox ID="TpkDaysTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"
                                                                MaxLength="3"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                TargetControlID="TpkDaysTextBox" FilterMode="ValidChars" FilterType="Numbers">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                        </div>
                                                        <div class="row" style="display: none;">
                                                            <label for="TpkNightsTextBox">
                                                                Number of Nights</label>
                                                            <asp:TextBox ID="TpkNightsTextBox" runat="server" ValidationGroup="ValidateData"
                                                                Enabled="false" TextMode="SingleLine" MaxLength="3"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                TargetControlID="TpkNightsTextBox" FilterMode="ValidChars" FilterType="Numbers">
                                                            </ajaxToolkit:FilteredTextBoxExtender>
                                                        </div>
                                                        
                                                        <div class="row">
                                                            <label for="TpkDaysTextBox">
                                                               Additional Cost Applicable</label>
                                                            <asp:CheckBox ID="AdditionalCostCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="AdditionalCostCheckBox_CheckedChanged" />
                                                        </div>
                                                         <asp:Panel ID ="AdditionalCostPanel" runat="server" Visible="false" >
                                                         <fieldset> 
                                                         <legend>Additional Cost </legend>
                                                          <div class="row">
                                                            <label for="TpkDaysTextBox">
                                                              Remarks</label>
                                                           <asp:TextBox ID="RemarksTextBox" runat="server"  ValidationGroup="ValidateData" TextMode="SingleLine" Width="650px"></asp:TextBox>
                                                     
                                                        </div>
                                                          <div class="row">
                                                            <label for="TpkDaysTextBox">
                                                               Additional Cost </label>
                                                           <asp:TextBox ID="AdditionalCostTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"></asp:TextBox>
                                                        </div>
                                                        </fieldset>


                                                        </asp:Panel>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                                        <div style="width: 100%; float: left; margin-top: 5px; margin-bottom: 5px; height: 30px;">
                                            <span style="font-size: 14px; font-weight: bold; color: Red;">Step 2 of 3</span>
                                            <span style="font-size: 11px; font-weight: bold; color: Red; float: right;">
                                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Back To Packege List" OnClick="BackLinkButton_Click"></asp:LinkButton></span>
                                        </div>
                                        <div style="width: 100%; float: left; margin-bottom: 5px;">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <fieldset>
                                                        <legend>Tour Package Itinerary</legend>
                                                        <div class="ContentWrapper">
                                                            <div class="row">
                                                                <label for="TpkItnCodeTextBox">
                                                                    Tour Package Code</label>
                                                                <asp:TextBox ID="TpkItnCodeTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    TextMode="SingleLine" MaxLength="128" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="row">
                                                                <label for="TpkItnNameTextBox">
                                                                    Tour Package Name</label>
                                                                <asp:TextBox ID="TpkItnNameTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    TextMode="SingleLine" MaxLength="128" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="row">
                                                                <label for="TpkDayNoDropDownList">
                                                                    Select Day No.</label>
                                                                <asp:DropDownList ID="TpkItnDayNoDropDownList" runat="server" Enabled="true" Width="210px"
                                                                    OnSelectedIndexChanged="TpkItnDayNoDropDownList_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="row">
                                                                <label for="TpkItnActivityTextBox">
                                                                    Activity for the Day</label>
                                                                <asp:TextBox ID="TpkActivityForDayTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    MaxLength="128" Width="670px"></asp:TextBox>
                                                            </div>
                                                            <div class="row">
                                                                <label for="TpkItnActivityTextBox">
                                                                    Activity Details</label>
                                                                <asp:TextBox ID="ItineraryDetailTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    TextMode="MultiLine" MaxLength="128" Width="670px" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                            <asp:UpdatePanel ID="OverNightUpdatePanel" runat="server">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <label for="DestDropDownList">
                                                                            Select Overnight Destination</label>
                                                                        <asp:DropDownList ID="DestDropDownList" runat="server" Enabled="true" Width="210px"
                                                                            AppendDataBoundItems="true">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="row">
                                                                        <label for="TpkItnOverNightTextBox">
                                                                            Over NightDest Note</label>
                                                                        <asp:TextBox ID="TpkItnOverNightTextBox" runat="server" ValidationGroup="ValidateData"
                                                                            TextMode="MultiLine" Width="72%" MaxLength="256"></asp:TextBox>
                                                                    </div>
                                                                    <div class="row" style="width: 450px; display: inline-block;">
                                                                        <label for="YesAccomadationRequiredRadioButton" style="width: 230px;">
                                                                            Is Accomadation Required??</label>
                                                                        <div style="margin-top: 6px;">
                                                                            <asp:RadioButton ID="YesAccomadationRequiredRadioButton" runat="server" Text="" GroupName="AccomadationRequiredGroup" />Yes
                                                                            <asp:RadioButton ID="NoAccomadationRequiredRadioButton" runat="server" Text="" GroupName="AccomadationRequiredGroup" />No
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="TpkItnDayNoDropDownList" EventName="SelectedIndexChanged" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                    </fieldset>
                                                    <div class="FromBodyButtonContainer">
                                                        <asp:Button ID="ItnAddButton" runat="server" Text=" Add " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="ItnAddButton_Click" />
                                                        <asp:Button ID="ItnEditButton" runat="server" Text=" Edit " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="ItnEditButton_Click" />
                                                        <asp:Button ID="ItnSaveButton" runat="server" Text=" Save " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="ItnSaveButton_Click" />
                                                        <asp:Button ID="ItnCancelButton" runat="server" Text=" Cancel " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="ItnCancelButton_Click" />
                                                    </div>
                                                    <div style="height: 10px;">
                                                    </div>
                                                    <fieldset>
                                                        <gridUC:CustomGridView ID="TpkPackagaeItineraryCustomGridView" runat="server" Width="100%" />
                                                    </fieldset>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                                        <div style="width: 100%; float: left; margin-top: 5px; margin-bottom: 5px; height: 30px;">
                                            <span style="font-size: 14px; font-weight: bold; color: Red;">Step 3 of 3</span>
                                            <span style="font-size: 11px; font-weight: bold; color: Red; float: right;">
                                                <asp:LinkButton ID="LinkButton2" runat="server" Text="Back To Packege List" OnClick="BackLinkButton_Click"></asp:LinkButton></span>
                                        </div>
                                        <div style="width: 100%; float: left; margin-bottom: 5px;">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <fieldset>
                                                        <legend>Tour packaage Photo</legend>
                                                        <div class="ContentWrapper">
                                                            <div class="row">
                                                                <label for="TpkPhotoCodeTextBox">
                                                                    Tour Package Code</label>
                                                                <asp:TextBox ID="TpkPhotoCodeTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    TextMode="SingleLine" MaxLength="128" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="row">
                                                                <label for="TpkPhotoNameTextBox">
                                                                    Tour Package Name</label>
                                                                <asp:TextBox ID="TpkPhotoNameTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    TextMode="SingleLine" MaxLength="128" Enabled="false" Width="72%"></asp:TextBox>
                                                            </div>
                                                            <div class="row">
                                                                <label for="TpkPhotoDescTextBox">
                                                                    Photo Description</label>
                                                                <asp:TextBox ID="TpkPhotoDescTextBox" runat="server" ValidationGroup="ValidateData"
                                                                    TextMode="MultiLine" MaxLength="512" Enabled="false" Width="72%"></asp:TextBox>
                                                            </div>
                                                            <div class="row">
                                                                <label for="IsDefaultCheckBox">
                                                                    Is Default Photo</label>
                                                                <asp:CheckBox ID="IsDefaultCheckBox" runat="server" ValidationGroup="ValidateData" />
                                                            </div>
                                                            
                                                              <div class="row">
                        
                        <div id="divImageUpload">
                            <%-- <asp:FileUpload ID="fupActivityImage" runat="server" onchange="return validatePhotographToUpload()" />--%>
                            <input id="fupActivityImage" type="file" size="45" name="photographToUpload" onchange="return validatePhotographToUpload();" />
                            <img id="FileLoading" alt="Loading..." src="<%=ResolveClientUrl("~/images/loading.gif")%>"
                                style="display: none;" />
                        </div>
                        <div id="divImageThumb">
                        <img id="imgSampleThumbImage" width="100" height="80" class="IntGalHLNoBrdr" alt='Sorry! No image found.'
                            src='' runat="server" />
                            <img id="imgThumb" alt="" />
                            <asp:Button ID="ClearImageButton" runat="server" Text="Clear Image" OnClick="ClearImageButton_Click" />
                       
                   </div> 
                                                            </div>
                                                    </fieldset>
                                                    <div class="FromBodyButtonContainer">
                                                        <asp:Button ID="PhotoAddButton" runat="server" Text=" Add " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="PhotoAddButton_Click" />
                                                        <asp:Button ID="PhotoEditButton" runat="server" Text=" Edit " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="PhotoEditButton_Click" />
                                                        <asp:Button ID="PhotoSaveButton" runat="server" Text=" Save " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="PhotoSaveButton_Click" />
                                                        <asp:Button ID="PhotoCancelButton" runat="server" Text=" Cancel " CssClass="FromBodyButton"
                                                            ValidationGroup="ValidateData" Width="80px" OnClick="PhotoCancelButton_Click" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <fieldset>
                                                        <div class="grid">
                                                            <asp:GridView ID="PhotoDtlsGridView" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanging="PhotoDtlsGridView_SelectedIndexChanging"
                                                                Width="100%" OnRowDeleting="PhotoDtlsGridView_RowDeleting" DataKeyNames="TpkPhotoID,PhotoNormal"
                                                                RowStyle-CssClass="NormalRow" AlternatingRowStyle-CssClass="alt" SelectedRowStyle-CssClass="SelectRow"
                                                                EmptyDataText="No Record Available!" GridLines="None" HeaderStyle-CssClass="header">
                                                                <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                                                                <Columns>
                                                                    <asp:TemplateField ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <div style="height: 28px">
                                                                            </div>
                                                                            <asp:ImageButton ID="EditPhotoImageButton" runat="server" CommandName="Select" ImageUrl="~/UserControl/GridView/GridViewIcons.ashx?T=S"
                                                                                ToolTip="Edit/Update Image Information." />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Image Description" DataField="PhotoDesc">
                                                                        <ItemStyle Width="700px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField HeaderText="IsDefault" DataField="IsDefaultPhoto">
                                                                        <ItemStyle Width="150px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Image">
                                                                        <ItemTemplate>
                                                                            <asp:Image ID="PhotoImage" runat="server" Height="62" Width="80" ImageUrl='<%#String.Format("data:image/jpg;base64,{0}", Convert.ToBase64String((byte[])Eval("PhotoThumb"))) %>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ShowHeader="False">
                                                                        <ItemTemplate>
                                                                            <div style="height: 28px">
                                                                            </div>
                                                                            <asp:ImageButton ID="DeletePhotoImageButton" runat="server" CausesValidation="False"
                                                                                CommandName="Delete" ImageUrl="~/UserControl/GridView/GridViewIcons.ashx?T=D"
                                                                                ToolTip="Delete Record." OnClientClick="return confirm('Are you sure you want to delete the selected image...?');" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="NormalRow"></RowStyle>
                                                                <SelectedRowStyle CssClass="SelectRow"></SelectedRowStyle>
                                                            </asp:GridView>
                                                        </div>
                                                    </fieldset>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="PhotoSaveButton" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </asp:WizardStep>
                                </WizardSteps>
                            </asp:Wizard>
                        </asp:Panel>
                    </div>
                </div>
                <asp:HiddenField ID="hfCustomerId" runat="server" />
                <asp:HiddenField ID="BusinessIDHiddenField" runat="server" Value="A001" />
                <asp:HiddenField ID="hdnStartPointID" runat="server" />
                <asp:HiddenField ID="ItineraryTypeHiddenField" runat="server" Value="1" />
                <asp:HiddenField ID="ItineraryListHiddenField" runat="server" />
                <asp:HiddenField ID="SelectedItineraryHiddenField" runat="server" />
               
            </ContentTemplate>
        </asp:UpdatePanel>
          <div id="disablelayer" style="position: fixed; left: 0px; top: 0px; z-index: 10000;
                width: 1349px; height: 1137px; display: none;">
            </div>
            <div id='ImageCropperPopup' style='position: absolute; z-index: 10005; padding: 10px;
                width: 600px; left: 300px; height: 500px; border-radius: 5px 5px 5px 5px; display: none;
                background: #ddd; right: 0px; top: 500px; overflow: hidden;'>
                <input id="btnAddImage" type="button" value="Add Image" onclick='$("#ImageCropperPopup").hide();$("#disablelayer").hide();ShowThumbImage();' />
               
                <input id="btnClearImage" type="button" value="Cancel" onclick='$("#ImageCropperPopup").hide();$("#disablelayer").hide();'  />
                <div id="uploadedImageDisplay">
                    <img id="cropImageTarget" alt="Please browse the image" name="cropImageTarget" src="<%= ResolveClientUrl("~/UserControls/PhotoGallery/DisplayUploadedImage.ashx?T=") + DateTime.Now.Second.ToString()%>"
                        style="border-style: none" />
                </div>
            </div>
            
               <asp:HiddenField ID="cropImageX1" runat="server" />
                <asp:HiddenField ID="cropImageY1" runat="server" />
                <asp:HiddenField ID="cropImageW" runat="server" />
                <asp:HiddenField ID="cropImageH" runat="server" />
            <asp:HiddenField ID="hdnUploadableImage" runat="server" />
            <asp:HiddenField ID="hidOperation" runat="server" />

    </div>
</asp:Content>
