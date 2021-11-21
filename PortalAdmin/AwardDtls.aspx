<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/PortalAdmin.Master"
    AutoEventWireup="true" CodeBehind="AwardDtls.aspx.cs" Inherits="e_Travel.PortalAdmin.AwardDtls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jQuery/ajaxfileupload.js" type="text/javascript"></script>
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
                        $('.jcrop-holder img').attr('src', '<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx?T=")%>' + (new Date()).getTime());
                        $('[id$=cropImageTarget]').Jcrop({
                            onChange: setCoords,
                            onSelect: setCoords,
                            onRelease: clearCoords
                        });
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
        function CropButton_onclick() {
            $.ajax({
                type: "POST",
                url: '<%= ResolveClientUrl("~/WebHandler/CropImage.ashx?X1=")%>' + $('[id$=cropImageX1]').val() + "&Y1=" + $('[id$=cropImageY1]').val() + "&X2=" + $('[id$=cropImageW]').val() + "&Y2=" + $('[id$=cropImageH]').val(),
                dataType: 'json',
                contentType: "application/json; charset=utf-8",
                success: function () {
                    $('[id$=cropImageTarget]').attr('src', '<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx?T=")%>' + (new Date()).getTime());
                    $('.jcrop-holder img').attr('src', '<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx?T=")%>' + (new Date()).getTime());
                    $('[id$=cropImageTarget]').Jcrop({
                        onChange: setCoords,
                        onSelect: setCoords,
                        onRelease: clearCoords
                    });
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            <asp:Label ID="CMSHeaderLabel" runat="server"></asp:Label>
        </div>
        <div class="BoxBody">
            <div class="BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>
                                <asp:Label ID="BusinessNameLabel" runat="server"></asp:Label>
                            </legend>
                            <div class="row">
                                <label for="AwardNameTextBox">
                                    Award Name</label>
                                <asp:TextBox ID="AwardNameTextBox" runat="server" TextMode="SingleLine" EnableViewState="True"
                                    MaxLength="128" Width="660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="AwardDescTextBox">
                                    Service Desc</label>
                                <asp:TextBox ID="AwardDescTextBox" runat="server" ValidationGroup="ValidateData"
                                    TextMode="MultiLine" EnableViewState="True" MaxLength="128" Width="660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="photographToUpload">
                                    Award Image</label>
                                <input id="photographToUpload" type="file" name="photographToUpload" onchange="return validatePhotographToUpload()"
                                    style="overflow: hidden; width: 71%" />
                                <img id="FileLoading" alt="Loading..." src="<%=ResolveClientUrl("~/images/loading.gif")%>"
                                    style="display: none;" />
                                <div id="uploadedImageDisplay">
                                    <img id="cropImageTarget" alt="Please browse the image" name="cropImageTarget" src="<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx")%>"
                                        style="border-style: solid; border-width: 2px; border-color: Gray; margin-top: 10px;" />
                                    <input id="CropButton" type="button" value="Crop Selected Area" onclick="return CropButton_onclick()" />
                                </div>
                            </div>
                            <div class="row">
                                <label for="AwardAttachmentFileUpload">
                                    Award Attachment</label>
                                <asp:FileUpload ID="AwardAttachmentFileUpload" runat="server" />
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
                                <asp:Button ID="ApproveButton" runat="server" Text="Approved" CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="80px" OnClick="ApproveButton_Click" />
                                <asp:Button ID="RejectButton" runat="server" Text=" Reject " CssClass="FromBodyButton"
                                    ValidationGroup="ValidateData" Width="95px" OnClick="RejectButton_Click" />
                                <asp:Button ID="ClearButton" runat="server" Text=" Clear " CssClass="FromBodyButton"
                                    Width="80px" OnClick="ClearButton_Click" />
                            </div>
                        </fieldset>
                        <fieldset>
                            <div style="display: inline-block; width: 300px; font-weight: bold; margin-bottom: 10px;">
                                <asp:LinkButton ID="AllLinkButton" runat="server" ForeColor="Red" CssClass="FromBodyButton"
                                    Text="All" OnClick="AllLinkButton_Click"></asp:LinkButton>
                                <asp:LinkButton ID="ReqLinkButton" runat="server" ForeColor="Red" CssClass="FromBodyButton"
                                    Text="Requested" OnClick="ReqLinkButton_Click"></asp:LinkButton>
                                <asp:LinkButton ID="AppLinkButton" runat="server" Text="Approved" CssClass="FromBodyButton"
                                    ForeColor="Red" OnClick="AppLinkButton_Click"></asp:LinkButton>
                                <asp:LinkButton ID="RejLinkButton" runat="server" Text="Rejected" CssClass="FromBodyButton"
                                    ForeColor="Red" OnClick="RejLinkButton_Click"></asp:LinkButton>
                            </div>
                            <gridUC:CustomGridView ID="AwardDtlsCustomGridView" runat="server" Width="100%" />
                        </fieldset>
                        <asp:HiddenField ID="LinkStatusHiddenField" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div id="disablelayer" style="position: fixed; left: 0px; top: 0px; z-index: 10000;
        width: 1349px; height: 1137px; display: none;">
    </div>
    <div id='HotelBriefPopup' style='position: absolute; z-index: 10005; padding: 10px;
        width: 600px; left: 300px; border-radius: 5px 5px 5px 5px; display: none; background: #ddd;
        right: 0px; top: 500px; overflow: hidden;'>
        <input id="btnAddImage" type="button" value="Add Image" onclick='$("#HotelBriefPopup").hide();$("#disablelayer").hide();ShowThumbImage();' />
        <%--   <asp:Button ID="btnAddImage" runat="server" Text="Add Image" OnClick="btnClearImage_Click" />--%>
        <asp:Button ID="btnClearImage" runat="server" Text="Cancel" />
        <div id="uploadedImageDisplay">
            <img id="cropImageTarget" alt="Please browse the image" name="cropImageTarget" src="<%= ResolveClientUrl("~/WebHandler/DisplayUploadedImage.ashx")%>"
                style="border-style: none; width: 590px;" />
        </div>
    </div>
    <div id="disablePhotolayer" style="position: fixed; left: 0px; top: 0px; z-index: 10000;
        width: 1349px; height: 1137px; display: none;">
    </div>
    <div id='HotelBriefPhotoPopup' style='position: absolute; z-index: 10005; padding: 10px;
        width: 600px; left: 300px; border-radius: 5px 5px 5px 5px; display: none; background: #ddd;
        right: 0px; top: 500px; overflow: hidden;'>
        <input id="btnAddPhoto" type="button" value="Add Photo" onclick='$("#HotelBriefPhotoPopup").hide();$("#disablePhotolayer").hide();ShowThumbPhoto();' />
        <asp:Button ID="btnCancelPhoto" runat="server" Text="Cancel" />
        <div id="uploadedPhotoDisplay">
            <img id="cropPhotoTarget" alt="Please browse the image" name="cropPhotoTarget" src="<%= ResolveClientUrl("~/Handlers/DisplayUploadedImage.ashx")%>"
                style="border-style: none; width: 590px;" />
        </div>
    </div>
    <asp:HiddenField ID="cropImageX1" runat="server" />
    <asp:HiddenField ID="cropImageY1" runat="server" />
    <asp:HiddenField ID="cropImageW" runat="server" />
    <asp:HiddenField ID="cropImageH" runat="server" />
    <asp:HiddenField ID="cropImagePX1" runat="server" />
    <asp:HiddenField ID="cropImagePY1" runat="server" />
    <asp:HiddenField ID="cropImagePW" runat="server" />
    <asp:HiddenField ID="cropImagePH" runat="server" />
    <asp:HiddenField ID="hdnUploadableImage" runat="server" />
    <asp:HiddenField ID="hdnOperation" runat="server" />
</asp:Content>
