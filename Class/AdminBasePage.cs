using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace e_Travel.Class
{
    public class AdminBasePage : Page
    {
        public AdminBasePage()
        {
            this.PreInit += new EventHandler(AdminBasePage_PreInit);
            this.Load += new EventHandler(AdminBasePage_Load);
        }

        protected void AdminBasePage_Load(object sender, EventArgs e)
        {
            #region Session variable Management
            if (Session["BusDtl"] == null)
            {
                if (Request.Cookies["BusDtl"] != null)
                {
                    Session["BusDtl"] = Request.Cookies["BusDtl"].Value.ToString();
                }
            }
            #endregion
        }

        protected void AdminBasePage_PreInit(object sender, EventArgs e)
        {

            if (Request.Browser.Browser == "IE")
            {
                if (Request.Browser.Version == "6.0" || Request.Browser.Version == "7.0")
                {
                    Page.Theme = "BusinessAdminDefault";
                }
                else
                {
                    Page.Theme = "BusinessAdminDefault";
                }
            }
            else
            {
                Page.Theme = "BusinessAdminDefault";
            }
        }

        public bool IsDate(TextBox inputValue)
        {

            try
            {
                string ss = DateTime.Parse(inputValue.Text.Trim()).ToString("dd-MMM-yyyy");
                string[] inputarray = ss.Trim().Split('-');
                if (inputarray.Length == 3)
                {
                    DateTime dt = DateTime.Parse(inputValue.Text);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}