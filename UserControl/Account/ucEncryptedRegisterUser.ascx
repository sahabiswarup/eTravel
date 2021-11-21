<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEncryptedRegisterUser.ascx.cs" Inherits="SIBINUtility.UserControls.Account.ucEncryptedRegisterUser" %>

<div class="AccountRegisterUserPanel">
 <asp:Label ID="ErrorMessageLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
<asp:CreateUserWizard ID="RegisterUser" runat="server" EnableViewState="false" 
        Width="100%" oncreateusererror="RegisterUser_CreateUserError" 
        oncreatinguser="RegisterUser_CreatingUser" LoginCreatedUser="False" 
        onnextbuttonclick="RegisterUser_NextButtonClick" 
        oncontinuebuttonclick="RegisterUser_ContinueButtonClick">
    <LayoutTemplate>
        <asp:PlaceHolder ID="wizardStepPlaceholder" runat="server"></asp:PlaceHolder>
        <asp:PlaceHolder ID="navigationPlaceholder" runat="server"></asp:PlaceHolder>
    </LayoutTemplate>
    <CompleteSuccessTextStyle HorizontalAlign="Center" />
    <WizardSteps>
        <asp:CreateUserWizardStep ID="RegisterUserWizardStep" runat="server">
            <ContentTemplate>
                <p style="color:Teal;">
                   
                <span style="font-size:15px; color:Red;"><blink>Note : </blink></span>
                  Passwords must be minimum of <span style=" font-size:12px;"><%= Membership.MinRequiredPasswordLength %> </span>characters in length and must consist of <span style=" font-size:12px;"> <%= Membership.MinRequiredNonAlphanumericCharacters %> </span> Non-alphanumeric/special character(s).
                </p>
                
                <span class="AccountfailureNotification">
                    <%--<asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>--%>
                  
                </span>
                <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="AccountfailureNotification" 
                        ValidationGroup="RegisterUserValidationGroup"/>
                <div class="AccountBody">
                    <fieldset>
                        <legend>Account Information</legend>
                        <div class="row">
                            <label for="UserName">User Name :</label>
                            <asp:TextBox ID="UserName" runat="server" autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                    CssClass="AccountfailureNotification cssclass1" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                                    ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="row">
                            <label for="Email">E-mail:</label>
                            <asp:TextBox ID="Email" runat="server" autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                                    CssClass="AccountfailureNotification cssclass2" ErrorMessage="E-mail is required." ToolTip="E-mail is required." 
                                    ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rgevEmail" runat="server" ControlToValidate="Email" 
                                    CssClass="AccountfailureNotification cssclass6" ErrorMessage="Invalid Email ID" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                    ValidationGroup="RegisterUserValidationGroup">*</asp:RegularExpressionValidator>
                        </div>
                        <div class="row">
                            <label for="Password">Password:</label>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password" autocomplete="off"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                    CssClass="AccountfailureNotification cssclass3" ErrorMessage="Password is required." ToolTip="Password is required." 
                                    ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                        </div>
                        <div class="row">
                            <label for="ConfirmPassword">Confirm Password:</label>
                            <asp:TextBox ID="ConfirmPassword" runat="server" autocomplete="off" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="AccountfailureNotification cssclass4" Display="Dynamic" 
                                    ErrorMessage="Confirm Password is required." ID="ConfirmPasswordRequired" runat="server" 
                                    ToolTip="Confirm Password is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                    CssClass="AccountfailureNotification cssclass5" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                    ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                        </div>
                    </fieldset>
                    <div class="FromBodyButtonContainer">
                        <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Create User" 
                                ValidationGroup="RegisterUserValidationGroup" CssClass="FromBodyButton"  />
                                 
                              
                    </div>
                </div>
            </ContentTemplate>
            <CustomNavigationTemplate>
            </CustomNavigationTemplate>
        </asp:CreateUserWizardStep>
        <asp:CompleteWizardStep ID="RegisterUserWizardCompleteStep" runat="server"></asp:CompleteWizardStep>
    </WizardSteps>
</asp:CreateUserWizard>
</div>
<script type="text/javascript">
    var gPasswordEncrypted = false;
    var gConfirmPasswordEncrypted = false;
    var gUserNameEncrypted = false;


    //Get the reference the client control
    var refUserName = document.getElementById('<% = ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserName")).ClientID %>');
    var refPassword = document.getElementById('<% = ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("Password")).ClientID %>');
    var refConfirmPassword = document.getElementById('<% = ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("ConfirmPassword")).ClientID %>');

    function ClientEncrypt(refSource) {
        //Now encrypt refbackend
        if (refSource.value.toString().length > 0) {
            refSource.value = Encrypt(refSource.value, '<%=Session["n"]%>', '<%=Session["e"]%>');
            return true;
        }
        else {
            return false;
        }
    }

    function encrypt() {
      
        gUserNameEncrypted = ClientEncrypt(refUserName);
        gPasswordEncrypted = ClientEncrypt(refPassword);
        gConfirmPasswordEncrypted = ClientEncrypt(refConfirmPassword);

        if (gPasswordEncrypted && gUserNameEncrypted && gConfirmPasswordEncrypted) {
            return true;
        }
        else {

            return false;
        }

    }
    $(document).ready(function () {
        $(".FromBodyButton").live("click", function () {

            if ($('.cssclass1').css('visibility') == 'hidden' && $('.cssclass2').css('visibility') == 'hidden' && $('.cssclass3').css('visibility') == 'hidden' && $('.cssclass4').css('display') == 'none' && $('.cssclass5').css('display') == 'none' && $('.cssclass6').css('visibility') == 'hidden') {
               
                encrypt();
            }
        });
    });
</script>

