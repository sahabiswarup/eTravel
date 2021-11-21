using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using e_Travel.Class;
using e_TravelBLL.BussinessManagement;
using System.Web.UI.WebControls;
using System.Data;

namespace e_Travel.PortalAdmin
{
    public partial class BussinessManagementPage : AdminBasePage
    {
        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to Business search page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadAllBussinessDetail();
                    LoadAllCMSRequestStatus();
                }
            }

        }
        #endregion

        #region Load all Bussiness details by BussinessID
        private void LoadAllBussinessDetail()
        {
            BussinessManagementBLL bussinessManagementObj;
            DataTable bussinessManagementDt = null;
            try
            {
                bussinessManagementObj = new BussinessManagementBLL();
                bussinessManagementDt = bussinessManagementObj.GetAllBussinessDetailsByID(Session["BusDtl"].ToString().Split('|')[0].ToString());
                if (bussinessManagementDt.Rows.Count > 0)
                {
                    BussinessNameTextBox.Text = bussinessManagementDt.Rows[0]["BusName"].ToString();
                    AddressTextBox.Text = bussinessManagementDt.Rows[0]["BusAddress"].ToString();
                    ContactNoTextBox.Text = bussinessManagementDt.Rows[0]["ContactMobileNo"].ToString();
                    EmailIDTextBox.Text = bussinessManagementDt.Rows[0]["ContactEmailID"].ToString();
                }
                else
                {
                    WebMessenger.Show(this, "Loading failed...", "Information", DialogTypes.Information);
                    Response.Redirect("~/PortalAdmin/BussinessManagementPage");
                }

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error occured..", "Error", DialogTypes.Error);
            }
            finally
            {
                bussinessManagementObj = null;
                bussinessManagementDt.Dispose();

            }

        }
        #endregion

        #region Load All CMS Request status
        private void LoadAllCMSRequestStatus()
        {
            BussinessManagementBLL bussinessManagementObj;
            DataTable bussinessManagementDt = null;
            try
            {
                bussinessManagementObj = new BussinessManagementBLL();
                bussinessManagementDt = bussinessManagementObj.GetAllCMSRequestStatus(Session["BusDtl"].ToString().Split('|')[0].ToString());
                if (bussinessManagementDt.Rows.Count > 0)
                {
                    CMSGridView.DataSource = bussinessManagementDt;
                    CMSGridView.DataBind();
                }
                else
                {
                    CMSGridView.DataSource = null;
                    WebMessenger.Show(this, "Loading failed...", "Information", DialogTypes.Information);
                    Response.Redirect("~/PortalAdmin/BussinessManagementPage");
                }

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error occured..", "Error", DialogTypes.Error);
            }
            finally
            {
                bussinessManagementObj = null;
                bussinessManagementDt.Dispose();

            }

        }

        #endregion
    }
}