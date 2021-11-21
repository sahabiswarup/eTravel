using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using e_Travel.Class;
using Sibin.Utilities.Web.ExceptionHandling;
using e_TravelBLL.CMS;


namespace e_Travel.BusinessAdmin
{
    public partial class SocialNetworkDtls : AdminBasePage
    {
        CMSMstSocialNetworkBLL objCMSMstSocialNetworkBLL;
        protected void Page_Load(object sender, EventArgs e)
        {
            checkBusinessID();
            if (!IsPostBack)
            {
                LoadSocialLinks();
            }
        }

        protected void LoadSocialLinks()
        {
            objCMSMstSocialNetworkBLL = new CMSMstSocialNetworkBLL();
            try
            {
                SocialGridView.DataSource = objCMSMstSocialNetworkBLL.GetSocialNetworkByBusinessID(Session["BusinessID"].ToString());
                SocialGridView.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Socail Links At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objCMSMstSocialNetworkBLL = null;
            }

        }


        protected void checkBusinessID()
        {
            if (Session["BusinessID"] == null)
            {
                if (Request.Cookies["BusinessID"] != null)
                {
                    Session["BusinessID"] = Request.Cookies["BusinessID"].Value.ToString();
                }
                else
                {
                    if (Page.User.Identity.Name != "")
                    {
                        if (User.IsInRole("BusinessAdmin"))
                        {
                            Session["BusinessID"] = HttpContext.Current.Profile.GetPropertyValue("BusinessID").ToString();
                        }
                    }
                }
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objCMSMstSocialNetworkBLL = new CMSMstSocialNetworkBLL();
            string FollowIDs = null, Shows = null, Links = null;
            try
            {
                foreach (GridViewRow row in SocialGridView.Rows)
                {
                    TextBox LinkTextBox = ((TextBox)row.FindControl("LinkTextBox"));
                    if(((CheckBox)row.FindControl("ShowCheckBox")).Checked== true)
                    {
                        if (LinkTextBox.Text.Trim() == "")
                        {
                            WebMessenger.Show(this, "Link Url Is Required ", "Information", DialogTypes.Information);
                        }
                    }
                    FollowIDs = FollowIDs + ((Label)row.FindControl("FollowIDLabel")).Text+",";
                    Shows = Shows + ((CheckBox)row.FindControl("ShowCheckBox")).Checked+",";
                    Links = Links + ((TextBox)row.FindControl("LinkTextBox")).Text+",";
                }
                objCMSMstSocialNetworkBLL.UpdateSocialNetworkDtls(Session["BusinessID"].ToString(), FollowIDs, Shows, Links, Page.User.Identity.Name);
                WebMessenger.Show(this, "All Changes Has Been Saved Successfully", "Saved", DialogTypes.Information);
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable to Save Your Changes At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objCMSMstSocialNetworkBLL = null;
            }
        }
        protected bool Show(String Show)
        {
            if (Show == "")
            {
                return false;
            }
            else
            {
                if (Show == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}