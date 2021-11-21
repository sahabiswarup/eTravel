<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/BusinessAdmin.Master"
    AutoEventWireup="true" CodeBehind="ReqMembership.aspx.cs" Inherits="e_Travel.BusinessAdmin.ReqMembership" %>

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
            url: '<%=ResolveClientUrl("~/WebHandler/AjaxFileUploader.ashx?file=file")%>',
            secureuri: false,
            fileElementId: 'photographToUpload',
            dataType: 'json',
            data: { name: 'logan', id: 'id' },
            success: function (data, status) {
                if (typeof (data.error) != 'undefined') {
                    if (data.error != '') {
                        alert(data.error);
                    } else {

                        alert("File Uploaded Successfully");

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
            var reg = /^[a-zA-Z0-9-_ \.]+\.(pdf|PDF)$/;
            if (uploadcontrol.length > 0) {
                //Checks with the control value.
                if (reg.test(uploadcontrol)) {
                    // Call Upload Handler
                    ajaxFileUpload();
                }
                else {
                    //If the condition not satisfied shows error message.
                    alert("Only .pdf or .PDF files are allowed!");
                    return false;
                }
            }
        } //End of function validate.
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="FromBody">
        <div class="BoxHeader">
            MemberShip Details
        </div>
        <div class="BoxBody">
            <div class="BoxContent">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Membership Details</legend>
                            <div class="row">
                                <label for="OrganisationDropDownList">
                                    Organisation Name</label>
                                <asp:DropDownList ID="OrganisationDropDownList" runat="server" AppendDataBoundItems="true"
                                    Width="300px">
                                </asp:DropDownList>
                            </div>
                            <div class="row">
                                <label for="MembershipDescTextBox">
                                    Description
                                </label>
                                <asp:TextBox ID="MembershipDescTextBox" runat="server" ValidationGroup="ValidateData"
                                    TextMode="MultiLine" EnableViewState="True" MaxLength="128" Width="660px"></asp:TextBox>
                            </div>
                            <div class="row">
                                <label for="MembershipDescTextBox">
                                    Membership Attachment</label>
                                <input id="photographToUpload" type="file" name="photographToUpload" onchange="return validatePhotographToUpload()"
                                    style="overflow: hidden; width: 92%" />
                                <img id="FileLoading" alt="Loading..." src="<%=ResolveClientUrl("~/images/loading.gif")%>"
                                    style="display: none;" />
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
                    <gridUC:CustomGridView ID="MemberShipCustomGridView" runat="server" Width="100%" />
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
