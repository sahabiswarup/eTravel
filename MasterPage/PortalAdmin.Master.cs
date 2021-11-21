using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using e_TravelBLL.CMS;
using System.Data;

namespace e_Travel.MasterPage
{
    public partial class PortalAdmin : System.Web.UI.MasterPage
    {
        #region Private Variables
        configBusMenuBLL objMenu;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            objMenu = new configBusMenuBLL();
            DataTable dt = new DataTable();
            if (!IsPostBack)
            {
                if (Session["BusDtl"] == null)
                {
                    if (this.Page.User.IsInRole("PortalAdmin"))
                    {
                        WebMessenger.Show(this.Page, "Session Expired... You will be redirected to Business search page...", "Session Expired...", DialogTypes.Information);
                        Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
                    }
                }
                else
                {
                    dt = objMenu.GetAllCMSMenuByBusID(Session["BusDtl"].ToString().Split('|')[0].ToString());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MenuItem mnu = new MenuItem(dt.Rows[i]["MenuName"].ToString(), dt.Rows[i]["MenuID"].ToString(), "", "~/PortalAdmin/" + dt.Rows[i]["PortalAdminUrl"].ToString() + "?ID=" + dt.Rows[i]["MenuID"].ToString() + "");
                        PortalAdminMenu.FindItem("CMS").ChildItems.Add(mnu);
                    }
                }
            }
        }
    }
}