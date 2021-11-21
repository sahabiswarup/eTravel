using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using e_Travel.Class;
using e_TravelBLL.BussinessManagement;
using System.Data;
namespace e_Travel.PortalAdmin
{
    public partial class BussinessSearchPage : AdminBasePage
    {
        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            BindEvents();
            BussinessSearchPageCustomGridView.SearchLabelText = "Search By Bussiness Name:";
            if (!IsPostBack)
            {

                BussinessSearchPageCustomGridView.AddGridViewColumns("Business ID|Business Name|Contact Person|Phone No.|Mobile No.|Email ID", "BusinessID|BusName|ContactPersonName|ContactPhoneNo|ContactMobileNo|ContactEmailID", "100|200|200|100|100|200", "BusinessID", true, false);

                BussinessSearchPageCustomGridView.BindData();

                Menu PortalMnu;
                PortalMnu = (Menu)Master.FindControl("PortalAdminMenu");
                if (PortalMnu != null)
                {
                    PortalMnu.Visible = false;
                }
                HtmlContainerControl menuWrap = (HtmlContainerControl)Master.FindControl("MenuWrap");
                menuWrap.InnerText = "Welcome to the Portal Administration Section... Please select a business and click on 'Manage Selected Business' button to proceed...";
            }
        }
        #endregion

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            BussinessSearchPageCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(BussinessSearchPageCustomGridView_SelectedIndexChanging);
            BussinessSearchPageCustomGridView.DataSource += new SIBINUtility.UserControl.SearchGridView.ucSearchGridView.GridViewDataSource(LoadBusinessCustomGridView);
            BussinessSearchPageCustomGridView.ucGridView_SearchButtonClick += new EventHandler(BussinessSearchPageCustomGridView_ucGridView_SearchButtonClick);

        }

        protected void BussinessSearchPageCustomGridView_ucGridView_SearchButtonClick(object sender, EventArgs e)
        {
            BussinessSearchPageCustomGridView.BindData();
        }

        protected void BussinessSearchPageCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (Request.Cookies["BusDtl"] == null)
            {
                HttpCookie loginCookie1 = new HttpCookie("BusDtl");
                Response.Cookies["BusDtl"].Value = BussinessSearchPageCustomGridView.DataKeys[e.NewSelectedIndex].Values["BusinessID"].ToString() + "|" + BussinessSearchPageCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text;
            }
            else
            {
                Response.Cookies["BusDtl"].Value = BussinessSearchPageCustomGridView.DataKeys[e.NewSelectedIndex].Values["BusinessID"].ToString() + "|" + BussinessSearchPageCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text;
            }
            Session["BusDtl"] = BussinessSearchPageCustomGridView.DataKeys[e.NewSelectedIndex].Values["BusinessID"].ToString() + "|" + BussinessSearchPageCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text;
        }
 

        private DataTable LoadBusinessCustomGridView()
        {
            BussinessManagementBLL bussinessManagementObj;
            DataTable dtRec = null;
            try
            {
                bussinessManagementObj = new BussinessManagementBLL();
                dtRec = new DataTable();
                int startRowIndex = (BussinessSearchPageCustomGridView.PageIndex * BussinessSearchPageCustomGridView.PageSize) + 1;
                dtRec = bussinessManagementObj.GetAllBusinessWiseDtlPaged(startRowIndex, BussinessSearchPageCustomGridView.PageSize, BussinessSearchPageCustomGridView.TextValue == string.Empty ? null : BussinessSearchPageCustomGridView.TextValue);
                if (dtRec.Rows.Count > 0)
                {
                    BussinessSearchPageCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable to load Business information list..", "Error", DialogTypes.Error);
            }
            finally
            {
                bussinessManagementObj = null;
                //dtRec = null;
            }
            return dtRec;
        }

        #endregion

        protected void ManageBussinessButton_Click(object sender, EventArgs e)
        {
            if (BussinessSearchPageCustomGridView.SelectedDataKey != null)
            {
                Response.Redirect("~/PortalAdmin/BussinessManagementPage.aspx");
            }
            else
            {
                WebMessenger.Show(this, "Select a Bussiness First..", "Information", DialogTypes.Information);
                return;
            }
        }
    }
}