using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Sibin.Utilities.Web.Cryptography;

namespace SIBINUtility.UserControls.Account
{
    public partial class ucEncryptedRegisterUser : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
            if (!this.Page.IsPostBack)
            {
                ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserName")).Text = string.Empty;
            }

            ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserName")).Focus();

            Sibin.Utilities.Web.Cryptography.Encryption sec = new Encryption(512);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("SecurityScript"))
            {
                string str = sec.GetJavaScriptClientCode();
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "SecurityScript", str);
            }

            if (Session["d"] == null)
            {
                Session["d"] = sec.ExportParamaters(true).D.ToString(16);
                Session["e"] = sec.ExportParamaters(true).E.ToString(16);
                Session["n"] = sec.ExportParamaters(true).N.ToString(16);
            }
        }

        protected void RegisterUser_CreateUserError(object sender, CreateUserErrorEventArgs e)
        {
            // Reset up the context for security and clean up username text box

            ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserName")).Text = RegisterUser.UserName;
            ((TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("UserName")).Focus();

            /* Take here your custom login failed handler if any */
        }

        protected void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            try
            {
                BigInteger bi_d = new BigInteger(Convert.ToString(Session["d"]), 16);
                BigInteger bi_n = new BigInteger(Convert.ToString(Session["n"]), 16);

                // Decrypt userName
                BigInteger UNbi_encrypted = new BigInteger(RegisterUser.UserName.ToString(), 16);
                BigInteger UNbi_decrypted = UNbi_encrypted.modPow(bi_d, bi_n);

                // Decrypt Password
                BigInteger Pbi_encrypted = new BigInteger(RegisterUser.Password.ToString(), 16);
                BigInteger Pbi_decrypted = Pbi_encrypted.modPow(bi_d, bi_n);

                //if (Pbi_decrypted.ToString(95).IndexOf(UNbi_decrypted.ToString(95), StringComparison.OrdinalIgnoreCase) >= 0)
                //{
                //    // Show the error message
                //    ((Literal)RegisterUserWizardStep.ContentTemplateContainer.FindControl("ErrorMessage")).Text = "The username may not appear anywhere in the password.";
                //    // Cancel the create user workflow
                //    e.Cancel = true;

                //    return;
                //}

                RegisterUser.UserName = UNbi_decrypted.ToString(95);

                Membership.CreateUser(RegisterUser.UserName, Pbi_decrypted.ToString(95), RegisterUser.Email);

                Session.Remove("d");
                Session.Remove("e");
                Session.Remove("n");

                e.Cancel = true;
                //RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
            }
            catch(Exception ex)
            {
                e.Cancel = false;
                RegisterUser.ActiveStepIndex = 0;
                ErrorMessageLabel.Text = "User creation attempt failed. Please contact the Technical Administration.";
                RegisterUser.UnknownErrorMessage = "User creation attempt failed. Please contact the Technical Administration.";
            }
        }

        protected void RegisterUser_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            
            if (ErrorMessageLabel.Text == "User creation attempt failed. Please contact the Technical Administration.")
            {
                e.Cancel = true;
                RegisterUser.ActiveStepIndex = 0;
            }
            else
            {
                e.Cancel = false;
                RegisterUser.ActiveStepIndex = 1;
            }
        }

        protected void RegisterUser_ContinueButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }


    }
}