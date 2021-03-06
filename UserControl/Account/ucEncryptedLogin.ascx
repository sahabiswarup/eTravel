<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEncryptedLogin.ascx.cs" Inherits="SIBINUtility.UserControls.Account.ucEncryptedLogin" %>
<script type="text/javascript">
    function noBack() {
        window.history.forward();
    }
    noBack();
    window.onload = noBack;
    window.onpageshow = function (evt) { if (evt.persisted) noBack(); }
    window.onunload = function () { void (0); }
</script>

<div id="LoginDiv" class="AccountLoginPanel">
    <div class="AccountHeader">
        <p>Welcome</p>
        <p>Please enter your credentials</p>
    </div>
    <div class="AccountBody">
    <asp:Login ID="EncryptedLogin" runat="server" 
               EnableViewState="false" 
               RenderOuterTable="false" 
               MembershipProvider="AspNetSqlMembershipProvider" 
               OnAuthenticate="EncryptedLogin_Authenticate"
               OnLoggedIn="EncryptedLogin_LoggedIn" 
               OnLoginError="EncryptedLogin_LoginError" 
            onloggingin="EncryptedLogin_LoggingIn">
        <LayoutTemplate>
            <span class="AccountfailureNotification">
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
            </span>
            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="AccountfailureNotification" 
                 ValidationGroup="LoginUserValidationGroup"/>

               
                    <div class="row">
                        <label for="UserName">User Name :</label>
                        <asp:TextBox ID="UserName" runat="server" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                             CssClass="AccountfailureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </div>
                    <div class="row">
                        <label for="Password">Password :</label>
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" autocomplete="off"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                             CssClass="AccountfailureNotification" ErrorMessage="Password is required." ToolTip="Password is required." 
                             ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                    </div>
               
                <p class="AccountSubmitButton">
                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Login" 
                        ValidationGroup="LoginUserValidationGroup" CssClass="FromBodyButton" OnClientClick="return encrypt();"/>
                    
                </p>

        </LayoutTemplate>
    </asp:Login>
    </div>
    </div>
    
    <div id="LoadingDiv" class="AccountLoginLoadingPanel">
    <p>&nbsp;</p>  
    <p>&nbsp;</p>
    <div style="float:left; width:35px; margin-left:15px;">
     <img style = "padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loading.gif")%>"/>
                <img style = "padding: 5px;" alt="Loading, Please Wait..." src="<%= ResolveClientUrl("~/Images/loadingText.gif")%>"/>
    </div>
    <div style="float: left; padding-top:8px; width:228px;">
        Please wait varifying login credentials...
    </div>
    </div>
    
    
    <script type="text/javascript">
    var gPasswordEncrypted = false;
    var gUserNameEncrypted = false;
    
    //Get the reference the client control
    var refUserName = document.getElementById('<% = ((TextBox)EncryptedLogin.FindControl("Username")).ClientID %>');
    var refPassword = document.getElementById('<% = ((TextBox)EncryptedLogin.FindControl("Password")).ClientID %>');

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

        if (gPasswordEncrypted && gUserNameEncrypted) {
            LoginDiv.style.display = "none";
            LoadingDiv.style.display = "block";
            return true;
        }
        else {

            return false;
        }

    }
</script>