using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Sibin.Utilities.Web.Cryptography;
using System.Web.Profile;

namespace SIBINUtility.UserControls.Account
{
    public partial class ucEncryptedLogin : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((TextBox)EncryptedLogin.FindControl("UserName")).Text = string.Empty;
            }

            ((TextBox)EncryptedLogin.FindControl("UserName")).Focus();

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

        protected void EncryptedLogin_LoggedIn(object sender, EventArgs e)
        {

            

        }

        protected void EncryptedLogin_LoginError(object sender, EventArgs e)
        {
            // Reset up the context for security and clean up username text box

            ((TextBox)EncryptedLogin.FindControl("UserName")).Text = EncryptedLogin.UserName;
            ((TextBox)EncryptedLogin.FindControl("UserName")).Focus();

            /* Take here your custom login failed handler if any */
        }

        protected void EncryptedLogin_Authenticate(object sender, AuthenticateEventArgs e)
        {

            MembershipUser user = Membership.GetUser(EncryptedLogin.UserName);

            if (user != null && (user.IsLockedOut == true))
            {
                EncryptedLogin.FailureText = "Your account has been locked out because of too many invalid login attempts. Please contact the administrator to have your account unlocked.";
                return;
            }


            /*--------------------------------------------------------------------------
             * Before Logging in Decrypt the values and re-assign the decrpted values
             */

            // Get Decryption Key and Salt
            BigInteger bi_d = new BigInteger(Convert.ToString(Session["d"]), 16);
            BigInteger bi_n = new BigInteger(Convert.ToString(Session["n"]), 16);

            //// Decrypt UserName
            //BigInteger Ubi_encrypted = new BigInteger(EncryptedLogin.UserName.ToString(), 16);
            //BigInteger Ubi_decrypted = Ubi_encrypted.modPow(bi_d, bi_n);

            // Decrypt Password
            BigInteger Pbi_encrypted = new BigInteger(EncryptedLogin.Password.ToString(), 16);
            BigInteger Pbi_decrypted = Pbi_encrypted.modPow(bi_d, bi_n);

            //EncryptedLogin.UserName = Ubi_decrypted.ToString(95);
            int UsersRoleCount = 0;
            try
            {
                if (Membership.ValidateUser(EncryptedLogin.UserName, Pbi_decrypted.ToString(95)))
                {
                    e.Authenticated = true;
                    FormsAuthentication.SetAuthCookie(EncryptedLogin.UserName, true);
                    Session.Remove("d");
                    Session.Remove("e");
                    Session.Remove("n");

                    string[] RoleList = Roles.GetAllRoles();

                    foreach (string RoleName in RoleList)
                    {
                        if (Roles.IsUserInRole(EncryptedLogin.UserName, RoleName))
                        {
                            UsersRoleCount++;
                        }
                    }
                    if (UsersRoleCount == 0)
                    {
                        EncryptedLogin.FailureText = "Your dont have any role assigned by the System Administrator. Please contact the Technical Administration.";
                    }

                    if (UsersRoleCount < 2)
                    {
                        if (Roles.IsUserInRole(EncryptedLogin.UserName, "BusinessAdmin"))
                        {
                            if (Request.Cookies["BusDtl"] == null)
                            {
                                HttpCookie loginCookie1 = new HttpCookie("BusDtl");
                                Response.Cookies["BusDtl"].Value = ProfileBase.Create(EncryptedLogin.UserName).GetPropertyValue("BusinessID").ToString() + "|" + ProfileBase.Create(EncryptedLogin.UserName).GetPropertyValue("BusinessName").ToString();
                            }
                            else
                            {
                                Response.Cookies["BusDtl"].Value = ProfileBase.Create(EncryptedLogin.UserName).GetPropertyValue("BusinessID").ToString() + "|" + ProfileBase.Create(EncryptedLogin.UserName).GetPropertyValue("BusinessName").ToString();
                            }
                            Session["BusDtl"] = ProfileBase.Create(EncryptedLogin.UserName).GetPropertyValue("BusinessID").ToString() + "|" + ProfileBase.Create(EncryptedLogin.UserName).GetPropertyValue("BusinessName").ToString();
                            Response.Redirect("~/BusinessAdmin/BusinessDetails.aspx");
                        }
                        else if (Roles.IsUserInRole(EncryptedLogin.UserName, "PortalAdmin"))
                        {
                            Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx",false);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Account/StartPage.aspx");
                    }

                }
                else
                {
                    EncryptedLogin.FailureText = "Invalid UserName or Password.";
                }

            }
            catch (Exception ex)
            {
                EncryptedLogin.FailureText = "Your login attempt failed. Please contact the Technical Administration.";
            }
        }
            
        protected void EncryptedLogin_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            BigInteger bi_d = new BigInteger(Convert.ToString(Session["d"]), 16);
            BigInteger bi_n = new BigInteger(Convert.ToString(Session["n"]), 16);

            // Decrypt userName
            BigInteger UNbi_encrypted = new BigInteger(EncryptedLogin.UserName.ToString(), 16);
            BigInteger UNbi_decrypted = UNbi_encrypted.modPow(bi_d, bi_n);

            EncryptedLogin.UserName = UNbi_decrypted.ToString(95);
        }
    }
}