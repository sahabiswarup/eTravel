<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEncryptedChangePassword.ascx.cs" Inherits="SIBINUtility.UserControls.Account.ucEncryptedChangePassword" %>
<link href="<%= ResolveClientUrl("~/Styles/Account.css")%>" rel="stylesheet" type="text/css" />

<div class="AccountRegisterUserPanel">
<p>
   <span style="color:Red;"><blink>Note :</blink></span> Passwords are required to be a minimum of <%= Membership.MinRequiredPasswordLength %> characters in length and must consist with <%= Membership.MinRequiredNonAlphanumericCharacters %> Non-alphanumeric/special character(s).
</p>
<span class="AccountfailureNotification">
    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
</span>
 <asp:ChangePassword ID="ChangeUserPassword" runat="server" EnableViewState="false" 
        RenderOuterTable="false" 
        onchangepassworderror="ChangeUserPassword_ChangePasswordError" 
        onchangingpassword="ChangeUserPassword_ChangingPassword">
        <ChangePasswordTemplate>
            
            <asp:ValidationSummary ID="ChangeUserPasswordValidationSummary" runat="server" CssClass="AccountfailureNotification" 
                 ValidationGroup="ChangeUserPasswordValidationGroup"/>
            <div class="AccountBody">
                <fieldset>
                    <legend>Account Information</legend>
                    <div class="row">
                        <label for="CurrentPassword">Old Password:</label>
                        <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword" 
                             CssClass="AccountfailureNotification" ErrorMessage="Password is required." ToolTip="Old Password is required." 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="row">
                        <label for="NewPassword">New Password:</label>
                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword" 
                             CssClass="AccountfailureNotification" ErrorMessage="New Password is required." ToolTip="New Password is required." 
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="row">
                        <label for="ConfirmNewPassword">Confirm New Password:</label>
                        <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword" 
                             CssClass="AccountfailureNotification" Display="Dynamic" ErrorMessage="Confirm New Password is required."
                             ToolTip="Confirm New Password is required." ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                             CssClass="AccountfailureNotification" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                             ValidationGroup="ChangeUserPasswordValidationGroup">*</asp:CompareValidator>
                    </div>
                </fieldset>
                <p class="AccountSubmitButton">
                    <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword" Text="Change Password" ValidationGroup="ChangeUserPasswordValidationGroup" CssClass="AccountButton" OnClientClick="return encrypt();" />
                    <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" CssClass="AccountButton" />
                </p>
            </div>
        </ChangePasswordTemplate>
 </asp:ChangePassword>
</div>



<script type="text/javascript">
    var gOldPasswordEncrypted = false;
    var gNewPasswordEncrypted = false;
    var gConfirmPasswordEncrypted = false;
    


    //Get the reference the client control
    var refOldPassword = document.getElementById('<% = ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("CurrentPassword")).ClientID %>');
    var refNewPassword = document.getElementById('<% = ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("NewPassword")).ClientID %>');
    var refConfirmPassword = document.getElementById('<% = ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("ConfirmNewPassword")).ClientID %>');

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
        gOldPasswordEncrypted = ClientEncrypt(refOldPassword);
        gNewPasswordEncrypted = ClientEncrypt(refNewPassword);
        gConfirmPasswordEncrypted = ClientEncrypt(refConfirmPassword);

        if (gOldPasswordEncrypted && gNewPasswordEncrypted && gConfirmPasswordEncrypted) {
            return true;
        }
        else {

            return false;
        }

    }
</script>