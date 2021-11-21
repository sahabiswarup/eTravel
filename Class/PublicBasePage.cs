using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using e_TravelBLL;

namespace e_Travel.Class
{
    public class PublicBasePage : Page
    {
        public PublicBasePage()
        {
            this.PreInit += new EventHandler(PublicBasePage_PreInit);
        }

        protected void PublicBasePage_PreInit(object sender, EventArgs e)
        {
            string hostName = "brotherstours.etravel.com";
            string BusID;
            BusinessMstBLL objBusMst = new BusinessMstBLL();
            if (Session["BusinessDtl"] == null)
            {
                if (Request.Cookies["BusinessDtl"] != null)
                {
                    Session["BusinessDtl"] = Request.Cookies["BusinessDtl"].Value.ToString();
                }
            }
            if (Session["BusinessDtl"] != null)
            {
                if (hostName != Session["BusinessDtl"].ToString().Split('|')[1])
                {
                    try
                    {
                        BusID = objBusMst.GetBusinessIDByDomainName(hostName);
                        if (BusID != "")
                        {
                            Session["BusinessDtl"] = BusID + "|" + hostName;
                        }
                        else
                        {
                            WebMessenger.Show(this.Page, "Oops an error occured... Please close the page and try again...", "Error...", DialogTypes.Error);
                        }
                    }
                    catch (Exception)
                    {
                        WebMessenger.Show(this.Page, "Oops an error occured... Please close the page and try again...", "Error...", DialogTypes.Error);
                    }
                }
            }
            else
            {
                BusID = objBusMst.GetBusinessIDByDomainName(hostName);
                if (BusID != "")
                {
                    Session["BusinessDtl"] = BusID + "|" + hostName;
                }
            }
        }
    }
}