using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

using e_TravelBLL;
using e_TravelBLL.CMS;

namespace e_Travel.MasterPage
{
    public partial class BusinessPublic : System.Web.UI.MasterPage
    {
        #region Private Variables
        BusinessMstBLL objBusMst;
        configBusMenuBLL objMenu;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            objMenu = new configBusMenuBLL();
            objBusMst = new BusinessMstBLL();
            DataTable dt = new DataTable();
            StringBuilder sbBottomLink = new StringBuilder();
            sbBottomLink.Append("<div id='siteinfoMapLink'>");
            if (!IsPostBack)
            {
                if (Session["BusinessDtl"] != null)
                {
                    BusLogoImg.Src = ResolveClientUrl("~/WebHandler/BusinessImageHandler.ashx?BID=" + Session["BusinessDtl"].ToString().Split('|')[0] + "&IMG=BL");
                    MenuItem HomeMnu = new MenuItem("Home", "Home", "", "~/BusinessPublic/default.aspx");
                    BusPublicMenu.Items.Add(HomeMnu);

                    dt = objMenu.GetAllCMSMenuByBusID(Session["BusinessDtl"].ToString().Split('|')[0].ToString());
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["PublicMenuPosition"].ToString() == "Top")
                        {
                            MenuItem mnu = new MenuItem(dt.Rows[i]["MenuName"].ToString(), dt.Rows[i]["MenuID"].ToString(), "", "~/BusinessPublic/" + dt.Rows[i]["BusPublicUrl"].ToString() + "?ID=" + dt.Rows[i]["MenuID"].ToString() + "");
                            BusPublicMenu.Items.Add(mnu);
                        }
                        if (dt.Rows[i]["PublicMenuPosition"].ToString() == "Bottom Left")
                        {
                            sbBottomLink.Append("<a href=" + ResolveClientUrl("~/BusinessPublic/" + dt.Rows[i]["BusPublicUrl"].ToString() + "?ID=" + dt.Rows[i]["MenuID"].ToString()) + ">" + dt.Rows[i]["MenuName"].ToString() + "</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                        }
                    }
                    sbBottomLink.Append("</div>");
                    BottomLeftLinkLiteral.Text = sbBottomLink.ToString();

                    dt = objBusMst.GetBusinessDtlByID(Session["BusinessDtl"].ToString().Split('|')[0].ToString());
                    if (dt.Rows.Count == 1)
                    {
                        BusAddSpan.InnerText = dt.Rows[0]["BusAddress"].ToString();
                        BusCitySpan.InnerText = dt.Rows[0]["BusCity"].ToString();
                        BusZipSpan.InnerText = dt.Rows[0]["BusZipCode"].ToString();
                        BusNameDiv.InnerText = dt.Rows[0]["BusName"].ToString();
                    }
                }
            }
        }
        #endregion
    }
}