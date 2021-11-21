<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="DestinationDtlMaster.aspx.cs" Inherits="e_Travel.BusinessAdmin.DestinationDtlMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jQuery/ajaxfileupload.js" type="text/javascript"></script>
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
                                 aspectRatio: 2 / 1
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
    </script>
    <script type="text/javascript">

        function ShowThumbImage() {
            $('[id$=divImageThumb]').show();
            $('[id$=divImageUpload]').hide();
            $('[id$=imgSampleThumbImage]').attr('src', '<%=ResolveClientUrl("~/WebHandler/DisplayCropedThumbImage.ashx?")%>cropImageX1=' + $('[id$=cropImageX1]').val() + ' &cropImageY1=' + $('[id$=cropImageY1]').val() + ' &cropImageW=' + $('[id$=cropImageW]').val() + ' &cropImageH=' + $('[id$=cropImageH]').val());

        };
        function HideThumbImage() {
            $('[id$=divImageThumb]').hide();
            $('[id$=divImageUpload]').show();
        };


    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
       
                <div class="BoxHeader">
                    <asp:Label ID="BusinessNameLabel" runat="server" Text=""></asp:Label></div>
                <div class="BoxBody">
                    <div class="BoxContent">
                     <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                        <fieldset>
                            <legend>Destination Information</legend>
                            <div class="row">
                                <label for="TaglineTextBox">
                                    TagLine
                                </label>
                                <asp:TextBox ID="TaglineTextBox" runat="server" ValidationGroup="ValidateData" MaxLength="512"
                                    Width="670px" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="HirarchyTextBox">
                                    Hirarchy</label>
                                <asp:TextBox ID="HirarchyTextBox" runat="server" ValidationGroup="ValidateData" Width="670px"
                                    Enabled="false"  ></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="DescTextBox">
                                    Description</label>
                                <asp:TextBox ID="DescTextBox" runat="server" ValidationGroup="ValidateData" TextMode="MultiLine"
                                    Width="670px" Enabled="false"></asp:TextBox>
                            </div>
                        </fieldset>
                        <fieldset>
                            <legend>Upload Photo For Destination</legend>
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
                        <fieldset>
                            <div class="FromBodyButtonContainer">
                                <asp:Button ID="AddButton" runat="server" Text=" Add " CssClass="FromBodyButton"
                                    Width="80px" onclick="AddButton_Click" />
                                <asp:Button ID="EditButton" runat="server" Text=" Edit " CssClass="FromBodyButton"
                                    Width="80px" onclick="EditButton_Click" />
                                <asp:Button ID="SaveButton" runat="server" Text="Save" CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="80px" onclick="SaveButton_Click" />
                                <asp:Button ID="ClearButton" runat="server" Text=" Clear " CssClass="FromBodyButton"
                                    Width="80px" onclick="ClearButton_Click" />
                            </div>
                        </fieldset>
                         </ContentTemplate>
            <Triggers>
             <asp:AsyncPostBackTrigger ControlID="HirarchyTextBox" EventName="textchanged" />
            </Triggers>
        </asp:UpdatePanel>
                      
                       
           
                        <fieldset>
            <gridUC:CustomGridView ID="DestinationDtlCustomGridView" runat="server" Width="100%" />
        </fieldset>
        </div>
                      </div>
               
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
