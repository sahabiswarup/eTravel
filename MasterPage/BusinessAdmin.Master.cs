using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using e_TravelBLL;
using e_TravelBLL.CMS;

namespace e_Travel.MasterPage
{
    public partial class BusinessAdmin : System.Web.UI.MasterPage
    {
        #region Private Variables
        configBusMenuBLL objMenu;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            objMenu = new configBusMenuBLL();
            DataTable dt = new DataTable();
            if (!IsPostBack)
            {
                if (Session["BusDtl"] == null)
                {
                    if (this.Page.User.IsInRole("BusinessAdmin"))
                    {
                        WebMessenger.Show(this.Page, "Session Expired... You will be redirected to Business search page...", "Session Expired...", DialogTypes.Information);
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
                else
                {
                    dt = objMenu.GetAllCMSMenuByBusID(Session["BusDtl"].ToString().Split('|')[0].ToString());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        MenuItem mnu = new MenuItem(dt.Rows[i]["MenuName"].ToString(), dt.Rows[i]["MenuID"].ToString(), "", "~/BusinessAdmin/" + dt.Rows[i]["BusAdminUrl"].ToString() + "?ID=" + dt.Rows[i]["MenuID"].ToString() + "");
                        BusAdminMenu.FindItem("PublicWebsiteMenu").ChildItems.Add(mnu);
                    }
                }
            }
        }
    }
}