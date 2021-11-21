using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using e_TravelBLL.CMS;
using e_Travel.Class;

namespace e_Travel.BusinessAdmin
{
    public partial class AdminCMS : AdminBasePage
    {
        #region private variables
        CMSCommonContentBLL objCMS;
        MstBusinessCMSMenuBLL objMenu;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/Account/Login.aspx");
            }

            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                objMenu = new MstBusinessCMSMenuBLL();
                try
                {
                    dt = objMenu.GetCMSMenuByMenuID(long.Parse(Request.QueryString["ID"].ToString()));
                    if (dt.Rows.Count == 1)
                    {
                        CMSHeaderLabel.Text = Session["BusDtl"].ToString().Split('|')[1].ToString() + " (" + dt.Rows[0]["MenuName"].ToString() + ")";
                        ContentLevel.Text = dt.Rows[0]["MenuName"].ToString();
                        ExistingContentSpan.InnerText = "Existing " + dt.Rows[0]["MenuName"].ToString();
                    }
                    else
                    {
                        WebMessenger.Show(this.Page, "Invalid Url, Redirecting to Default Page...", "Error", DialogTypes.Error);
                        Response.Redirect(ResolveClientUrl("~/BusinessAdmin/BusAdminDefault.aspx"));
                        return;
                    }
                    LoadCMSContent();
                }
                catch (Exception)
                {
                    WebMessenger.Show(this.Page, "Error occured in server, some content may not load properly. Please close the page and try again...", "Error", DialogTypes.Error);
                }
                finally
                {
                    objMenu = null;
                }
            }
        }
        #endregion

        #region Load CMS Content
        private void LoadCMSContent()
        {
            objCMS = new CMSCommonContentBLL();
            DataTable dt = new DataTable();
            try
            {
                dt = objCMS.GetCMSCommonContentByID(Session["BusDtl"].ToString().Split('|')[0].ToString(), long.Parse(Request.QueryString["ID"].ToString()));
                if (dt.Rows.Count == 1)
                {
                    if (bool.Parse(dt.Rows[0]["IsRequested"].ToString()))
                    {
                        ExistingContentFieldSet.Visible = true;
                        ContentCKEditor.Text = dt.Rows[0]["UnPublishedContent"].ToString();
                        ExistingContentLabel.Text = dt.Rows[0]["PublishedContent"].ToString();
                    }
                    else
                    {
                        ExistingContentFieldSet.Visible = false;
                        ContentCKEditor.Text = dt.Rows[0]["PublishedContent"].ToString();
                    }
                }
                else
                {
                    ExistingContentFieldSet.Visible = false;
                    ContentCKEditor.Text = "No content configured yet...";
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Unable to load CMS Content... Pleae try again...", "Error", DialogTypes.Error);
            }
            finally
            {
                objCMS = null;
                dt.Dispose();
            }
        }
        #endregion

        #region button click events
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Server.HtmlDecode(ContentCKEditor.Text.Trim())))
            {
                WebMessenger.Show(this.Page, "Please enter CMS Content...", "Information", DialogTypes.Information);
                return;
            }

            objCMS = new CMSCommonContentBLL();
            try
            {
                objCMS.RequestCommonCMSChange(Session["BusDtl"].ToString().Split('|')[0].ToString(), long.Parse(Request.QueryString["ID"]), ContentCKEditor.Text.Trim(), Page.User.Identity.Name);
                WebMessenger.Show(this.Page, "Content change request has been submitted succesfully...", "Information", DialogTypes.Success);
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Unable to save to Content, Please try again...", "Error", DialogTypes.Error);
            }
            finally
            {
                objCMS = null;
            }
        }
        #endregion
    }
}