using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using e_Travel.Class;
using e_TravelBLL.CMS;

namespace e_Travel.BusinessPublic
{
    public partial class PublicCMS : PublicBasePage
    {
        #region private variables
        CMSCommonContentBLL objCMS;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                objCMS = new CMSCommonContentBLL();
                try
                {
                    dt = objCMS.GetCMSCommonContentForPublic(Session["BusinessDtl"].ToString().Split('|')[0].ToString(), long.Parse(Request.QueryString["ID"].ToString()));
                    if (dt.Rows.Count == 1)
                    {
                        contentHeader.InnerText = dt.Rows[0]["MenuText"].ToString();
                        PageContentLabel.Text = Server.HtmlDecode(dt.Rows[0]["PublishedContent"].ToString());
                    }
                    else
                    {
                        contentHeader.InnerText = "Page Under Construction";
                        PageContentLabel.Text = "This page is under construction, inconvenience regretted...";
                    }
                }
                catch (Exception)
                {
                    WebMessenger.Show(this.Page, "Error occured in server, Redirecting to Default Page...", "Error", DialogTypes.Error);
                    Response.Redirect(ResolveClientUrl("~/BusinessPublic/default.aspx"));
                }
                finally
                {
                    objCMS = null;
                }
            }
        }
        #endregion
    }
}