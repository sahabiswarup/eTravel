<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master" AutoEventWireup="true" CodeBehind="BusinessDetails.aspx.cs" Inherits="e_Travel.BusinessAdmin.BusinessDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jQuery/ajaxfileupload.js" type="text/javascript"></script>
     <link href="../Scripts/FancyBox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/FancyBox/jquery.fancybox.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FancyBox/jquery.fancybox.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.fancybox.pack.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.mousewheel-3.0.6.pack.js" type="text/javascript"></script>
    <link href="../Scripts/FancyBox/jquery.fancybox-buttons.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FancyBox/jquery.fancybox-buttons.js" type="text/javascript"></script>
    <script src="../Scripts/FancyBox/jquery.fancybox-media.js" type="text/javascript"></script>
    <link href="../Scripts/FancyBox/jquery.fancybox-thumbs.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/FancyBox/jquery.fancybox-thumbs.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ajaxFileUpload() {
            $("#FileLoading")
            .ajaxStart(function () {
                $(this).show();
            })
        .ajaxComplete(function () {
            $(this).hide();
        });
            $.ajaxFileUpload
        ({
            url: '<%=ResolveClientUrl("~/WebHandler/AjaxFileUploader.ashx")%>',
            secureuri: false,
            fileElementId: 'photographToUpload',
            dataType: 'json',
            data: { name: 'logan', id: 'id' },
            success: function (data, status) {
                if (typeof (data.error) != 'undefined') {
                    if (data.error != '') {
                        alert(data.error);
                    } else {

                        $('[id$=cropImageTarget]').attr('src', '<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx?T=")%>' + (new Date()).getTime());
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
            var uploadcontrol = document.getElementById('photographToUpload').value;
            uploadcontrol = uploadcontrol.substring(uploadcontrol.lastIndexOf('\\') + 1);
            //alert(uploadcontrol.lastIndexOf('\\'));
            //Regular Expression for fileupload control.
            var reg = /^[a-zA-Z0-9-_ \.]+\.(pjpg|PJPG|jpg|JPG|jpeg|JPEG|gif|GIF|png|PNG)$/;
            if (uploadcontrol.length > 0) {
                //Checks with the control value.
                if (reg.test(uploadcontrol)) {
                    // Call Upload Handler
                    ajaxFileUpload();
                }
                else {
                    //If the condition not satisfied shows error message.
                    alert("Only .jpg or .JPG, .pjpg or .PJPG, .jpeg or .JPEG, .gif or .GIF and .png or .PNG files are allowed!");
                    return false;
                }
            }
        } //End of function validate.

        function loadImage() {

            $('[id$=cropImageTarget]').attr('src', '<%=ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx?T=")%>' + (new Date()).getTime());
        }
        function pageLoad(sender, args) {
          $("#BusinessImage").fancybox({
            'titleShow'     : true,
            'transitionIn'  : 'elastic',
            'transitionOut': 'elastic',
        }); 
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">   
        <div class = "FromBody">
         <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div class = "BoxHeader"><asp:Label ID="BusinessNameLabel" runat="server" Text=""></asp:Label></div>
            <div class = "BoxBody">
                    <div class = "BoxContent">
                    
                    <div style = "width:472px; float: left; margin-right: 20px;">
                    <fieldset>
                       <legend>Business Information</legend>                       
                        <div class="row">
                           <label for="BusNameTextBox">Business Name</label>
                            <asp:TextBox ID="BusNameTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "128" width = "305px" Enabled="false"></asp:TextBox>
                 </div>
                    <div class="row">
                        <label for="BusAddTextBox">Business Address</label>
                        <asp:TextBox ID="BusAddTextBox" runat="server" ValidationGroup="ValidateData" TextMode="MultiLine"  MaxLength = "1024" width = "305px"  Enabled="false"></asp:TextBox>
                    </div>
                    <div class="row">
                        <label for="CountryDropDownList">Country</label>
                        <asp:DropDownList ID="CountryDropDownList" runat="server" 
                            ValidationGroup="ValidateData" Width = "315px" AppendDataBoundItems="True" 
                            AutoPostBack="True" 
                            onselectedindexchanged="CountryDropDownList_SelectedIndexChanged"  Enabled="false">
                                
                        </asp:DropDownList>
                    </div>
                        <div class="row">
                            <label for="DestinationDropDownList" >Destination</label>
                            <asp:DropDownList ID="DestinationDropDownList" runat="server" ValidationGroup="ValidateData" Width = "315px" AppendDataBoundItems="True"  Enabled="false">
                            </asp:DropDownList>
                        </div>
                        <div class="row">
                            <label for="BusCityTextBox">City/Town</label>
                            <asp:TextBox ID="BusCityTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"  Enabled="false"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="ZipCodeTextBox">Zip Code</label> 
                            <asp:TextBox ID="ZipCodeTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine" MaxLength = "1024" width = "305px"  Enabled="false"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="WebsiteTextBox">Website URL</label>
                            <asp:TextBox ID="WebsiteTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"  Enabled="false"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="BusTypeDropDownList">Business Type</label>
                            <asp:DropDownList ID="BusTypeDropDownList" runat="server" ValidationGroup="ValidateData" Width = "315px" AppendDataBoundItems="True"  Enabled="false">
                                <asp:ListItem Value="0">--- Select Business Type ---</asp:ListItem>
                                <asp:ListItem Value="1">Travel Agent</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="row">
                            <label for="TimeZoneDropDownList">Time Zone</label>
                            <asp:DropDownList ID="TimeZoneDropDownList" runat="server" ValidationGroup="ValidateData" Width = "315px" AppendDataBoundItems="True"  Enabled="false">
                             <asp:ListItem Value="0">--- Select TimeZone ---</asp:ListItem>
                                <asp:ListItem Value="+5.30">Chennai+India:+5.30</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="row">
                            <label for="LatitudeTextBox">Latitude</label>
                            <asp:TextBox ID="LatitudeTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"  Enabled="false"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="LongitudeTextBox">Longitude</label>
                            <asp:TextBox ID="LongitudeTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"  Enabled="false"></asp:TextBox>
                        </div>
                    </fieldset>
            </div>
            <div style = "width:472px; float: left; margin: 0px; padding: 0px;">
                    <fieldset>
                        <legend>Business Contact Details</legend>
                        <div class="row">
                            <label for="ContactNameTextBox">Contact Name</label>
                            <asp:TextBox ID="ContactNameTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine" MaxLength = "128" width = "305px"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="PhoneNoTextBox">Phone No.</label>
                            <asp:TextBox ID="PhoneNoTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="MobileNoTextBox">Mobile No.</label>
                            <asp:TextBox ID="MobileNoTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="AltMobileNoTextBox">Alt. Mobile No.</label>
                            <asp:TextBox ID="AltMobileNoTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="EmailIDTextBox">Email ID</label>
                            <asp:TextBox ID="EmailIDTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="AltEmailIDTextBox">Alt. Email ID</label>
                            <asp:TextBox ID="AltEmailIDTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"></asp:TextBox>
                        </div>
                        <div class="row">
                            <label for="FaxNoTextBox">Fax No.</label>
                            <asp:TextBox ID="FaxNoTextBox" runat="server" ValidationGroup="ValidateData" TextMode="SingleLine"  MaxLength = "1024" width = "305px"></asp:TextBox>
                        </div>
                    </fieldset>
                    
                    <fieldset>
                        <legend>Business Logo (Max 4 MB)</legend>
                        <div class="row">
                            <%--<input id="photographToUpload" type="file" name="photographToUpload" onchange="return validatePhotographToUpload()" style = "overflow: hidden; width: 90%" />
                            <img id="FileLoading" alt="Loading..." src="<%=ResolveClientUrl("~/images/loading.gif")%>" style="display:none;" />--%>
                            <div id="uploadedImageDisplay">
                                <img id="cropImageTarget" alt="Please browse the image" name="cropImageTarget" src="<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx")%>" style="border-style: solid; border-width: 2px; border-color: Gray; max-width:430px; max-height: 120px; margin-top: 10px;"/>
                            </div>
                        </div>
                    </fieldset>
                </div>
                <div id="CommandControls"style = "width:100%; float: left;">
                          <fieldset>
                            <div class="FromBodyButtonContainer">
                            <asp:Button ID="UpdateButton" runat="server" Text=" Update " 
                                    CssClass="FromBodyButton"  ValidationGroup="ValidateData" Width = "80px" 
                                    onclick="UpdateButton_Click"/>                               
                                <asp:Button ID="CancelButton" runat="server" Text=" Cancel " 
                                    CssClass="FromBodyButton"  Width = "80px" onclick="CancelButton_Click"/>
                            </div>
                        </fieldset>
                </div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div style = "clear: both"></div>
<div style="display:none">
<div id="imageDialog"> 

 <img id="Img1" alt="Please browse the image" name="Img1" src="<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx")%>" style="border-style: solid; border-width: 2px; border-color: Gray; width: 600px; height:500px; margin-top: 10px;"/>
</div>
</div>
</asp:Content>
