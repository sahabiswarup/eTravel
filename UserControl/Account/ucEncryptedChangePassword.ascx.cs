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
    public partial class ucEncryptedChangePassword : System.Web.UI.UserControl
    {
        private string _ChangePasswordSuccessURL;
        private string _ChangePasswordCancelURL;

        public string CancelDestinationPageUrl
        {
            get { return _ChangePasswordCancelURL; }
            set { _ChangePasswordCancelURL = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                ChangeUserPassword.CancelDestinationPageUrl = CancelDestinationPageUrl;

                ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("CurrentPassword")).Text = string.Empty;
            }

            ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("CurrentPassword")).Focus();

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

        protected void ChangeUserPassword_ChangePasswordError(object sender, EventArgs e)
        {
            // Reset up the context for security and clean up username text box

            ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("CurrentPassword")).Text = string.Empty;
            ((TextBox)ChangeUserPassword.ChangePasswordTemplateContainer.FindControl("CurrentPassword")).Focus();

            /* Take here your custom login failed handler if any */
        }

        protected void ChangeUserPassword_ChangingPassword(object sender, LoginCancelEventArgs e)
        {
            try
            {
                BigInteger bi_d = new BigInteger(Convert.ToString(Session["d"]), 16);
                BigInteger bi_n = new BigInteger(Convert.ToString(Session["n"]), 16);

                // Decrypt Current Password
                BigInteger CPbi_encrypted = new BigInteger(ChangeUserPassword.CurrentPassword.ToString(), 16);
                BigInteger CPbi_decrypted = CPbi_encrypted.modPow(bi_d, bi_n);

                // Decrypt New Password
                BigInteger NPbi_encrypted = new BigInteger(ChangeUserPassword.NewPassword.ToString(), 16);
                BigInteger NPbi_decrypted = NPbi_encrypted.modPow(bi_d, bi_n);

                MembershipUser mu = Membership.GetUser(this.Page.User.Identity.Name);
                bool IsUpdated = mu.ChangePassword(CPbi_decrypted.ToString(95), NPbi_decrypted.ToString(95));
                Membership.UpdateUser(mu);

                Session.Remove("d");
                Session.Remove("e");
                Session.Remove("n");

                if (IsUpdated)
                {
                    
                    FailureText.Text = "Your password has been changed successfully.";
                }
                else
                {
                    FailureText.Text = "Update password attempt failed. Check old password.";
                }

                e.Cancel = true;
            }
            catch (Exception ex)
            {
                FailureText.Text = "Update password attempt failed. Please contact the Technical Administration.";
            }
        }


    }
}