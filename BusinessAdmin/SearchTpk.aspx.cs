using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using Sibin.Utilities.Web.ExceptionHandling;
using e_TravelBLL.TourPackage;
using SIBINUtility.ValidatorClass;
using Sibin.FrameworkExtensions.DotNet.Web;
using System.Data;

namespace e_Travel.BusinessAdmin
{
    public partial class SearchTpk : AdminBasePage
    {
        #region Private Variable
        
        TpkPackageMst objPackage;
        
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
            if (!IsPostBack)
            {
                 LoadDestination();
                 LoadAllPackageType();
                 DisplayResultByCriteria();
            }
        }
        #endregion

        #region Load Destination
        private void LoadDestination()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DestinationDropDown.Items.Clear();
                DestinationDropDown.Items.Add(new ListItem("--- Select Destination ---", "0"));
                DestinationDropDown.DataSource = objPackage.GetPackageDestinationByBusinessID(Session["BusDtl"].ToString().Split('|')[0].ToString());
                DestinationDropDown.DataTextField = "DestinationName";
                DestinationDropDown.DataValueField = "DestinationID";
                DestinationDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Destination", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }
        #endregion

        #region Load Package Type In Dropdown
        private void LoadAllPackageType()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = objPackage.GetPackageTypeByBusinessID(Session["BusDtl"].ToString().Split('|')[0].ToString());
                PackageTypeDropDown.Items.Clear();
                PackageTypeDropDown.Items.Add(new ListItem("----------Select Package Type----------", "0"));
                PackageTypeDropDown.DataTextField = "PackageTypeName";
                PackageTypeDropDown.DataValueField = "PackageTypeID";
                PackageTypeDropDown.DataSource = dt;
                PackageTypeDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Package Type From Server", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }

        }
        #endregion

        protected void SearchPackage_Click(object sender, EventArgs e)
        {
            HiddenField1.Value = "1";

            if (DestinationDropDown.SelectedValue == "0" && PackageTypeDropDown.SelectedValue == "0" && TourDurationDropDown.SelectedValue == "0")
            {
                WebMessenger.Show(this, "Select Atleast One Criteria", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                GridViewPackage.PageIndex = 0;
                DisplayResultByCriteria();
            }
        }

        #region Paging and Gridview Events
        protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow pagerRow = GridViewPackage.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            System.Web.UI.WebControls.DropDownList pageList = (System.Web.UI.WebControls.DropDownList)pagerRow.Cells[0].FindControl("ddlPages");

            // Set the PageIndex property to display that page selected by the user.
            GridViewPackage.PageIndex = pageList.SelectedIndex;
            PageIndexHiddenField.Value = (int.Parse(((DropDownList)sender).SelectedValue) - 1).ToString();
            DisplayResultByCriteria();
           
        }

        protected void GridViewPackage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {

                Response.Redirect("~/BusinessAdmin/ConfigureTpk.aspx?TPKID=" + e.CommandArgument.ToString());
            }
        }

        protected void GridViewPackage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = GridViewPackage.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            System.Web.UI.WebControls.DropDownList pageList = (System.Web.UI.WebControls.DropDownList)pagerRow.Cells[0].FindControl("ddlPages");
            pageList.SelectedIndex = int.Parse(PageIndexHiddenField.Value.ToString());
            DisplayResultByCriteria();
        }

        protected void GridViewPackage_DataBound(object sender, EventArgs e)
        {
            TotalPageCount.Value = ((int)Math.Ceiling((decimal)int.Parse(this.ViewState["TotalRecord"].ToString()) / (decimal)GridViewPackage.PageSize)).ToString();
            if (int.Parse(TotalPageCount.Value.ToString()) == 0)
            {
                TotalPageCount.Value = "1";
            }
            // Retrieve the pager row.
            GridViewRow pagerRow = GridViewPackage.BottomPagerRow;
            if (pagerRow != null)
            {
                // Retrieve the DropDownList and Label controls from the row.
                System.Web.UI.WebControls.DropDownList pageList = (System.Web.UI.WebControls.DropDownList)pagerRow.Cells[0].FindControl("ddlPages");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("lblPageCount");
                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < int.Parse(TotalPageCount.Value.ToString()); i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());
                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == int.Parse(PageIndexHiddenField.Value))
                        {
                            item.Selected = true;
                        }
                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);

                    }
                }

                if (pageLabel != null)
                {
                    // Update the Label control with the current page information.
                    if (ViewState["TotalRecord"] != null)
                    {
                        pageLabel.Text = TotalPageCount.Value;
                        lblRecordCount.Text = this.ViewState["TotalRecord"].ToString() + " Record(s) Found.";
                        if (ViewState["TotalRecord"].ToString() == "1")
                        {
                            lblRecordCount.Text = "Total 1 Record Found";
                        }
                    }
                }

                #region Manage Button States
                ImageButton btnPrev = (ImageButton)pagerRow.Cells[0].FindControl("btnPrev");
                ImageButton btnNext = (ImageButton)pagerRow.Cells[0].FindControl("btnNext");
                ImageButton btnFirst = (ImageButton)pagerRow.Cells[0].FindControl("btnFirst");
                ImageButton btnLast = (ImageButton)pagerRow.Cells[0].FindControl("btnLast");

                //now figure out what page we're on
                if (int.Parse(TotalPageCount.Value.ToString()) == 1)
                {
                    btnLast.Enabled = false;
                    btnNext.Enabled = false;
                }
                if (int.Parse(PageIndexHiddenField.Value) == 0)
                {
                    btnPrev.Enabled = false;
                    btnFirst.Enabled = false;
                }
                else if (int.Parse(PageIndexHiddenField.Value) + 1 == int.Parse(TotalPageCount.Value.ToString()))
                {
                    btnLast.Enabled = false;
                    btnNext.Enabled = false;
                }
                else
                {
                    btnLast.Enabled = true;
                    btnNext.Enabled = true;
                    btnPrev.Enabled = true;
                    btnFirst.Enabled = true;
                }
                #endregion

                #region Enable pager and make it visible
                pagerRow.Enabled = true;
                pagerRow.Visible = true;
                #endregion
            }
        }

        protected void ManagePaging(object sender, CommandEventArgs e)
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = GridViewPackage.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            System.Web.UI.WebControls.DropDownList pageList = (System.Web.UI.WebControls.DropDownList)pagerRow.Cells[0].FindControl("ddlPages");
            switch (e.CommandArgument.ToString())
            {
                case "Prev":
                    if (int.Parse(PageIndexHiddenField.Value.ToString()) > 0)
                    {
                        GridViewPackage.PageIndex = int.Parse(PageIndexHiddenField.Value.ToString()) - 1;
                        PageIndexHiddenField.Value = GridViewPackage.PageIndex.ToString();
                    }
                    break;
                case "Next":
                    if (int.Parse(PageIndexHiddenField.Value.ToString()) < int.Parse(TotalPageCount.Value.ToString()))
                    {
                        GridViewPackage.PageIndex = int.Parse(PageIndexHiddenField.Value.ToString()) + 1;
                        PageIndexHiddenField.Value = GridViewPackage.PageIndex.ToString();
                    }
                    break;
                case "First":
                    GridViewPackage.PageIndex = 0;
                    PageIndexHiddenField.Value = GridViewPackage.PageIndex.ToString();
                    break;
                case "Last":
                    GridViewPackage.PageIndex = int.Parse(TotalPageCount.Value.ToString()) - 1;
                    PageIndexHiddenField.Value = GridViewPackage.PageIndex.ToString();
                    break;
            }
        }
        #endregion

        #region Display Search Package By Criteria
        private void DisplayResultByCriteria()
        {
            objPackage = new TpkPackageMst();
            try
            {
                bool IndianNationality = true;
                int pageIndex = GridViewPackage.PageIndex * GridViewPackage.PageSize + 1;
                DataTable dt = new DataTable();
                Int64? DestinationID = null, PackageTypeID = null;
                int? MinimumDays = null;
                int? MaximumDays = null;
                if (DestinationDropDown.SelectedValue == "0")
                {
                    DestinationID = null;
                }
                else
                {
                    DestinationID = Int64.Parse(DestinationDropDown.SelectedValue.ToString());
                }
                if (PackageTypeDropDown.SelectedValue == "0")
                {
                    PackageTypeID = null;
                }
                else
                {
                    PackageTypeID = Int64.Parse(PackageTypeDropDown.SelectedValue.ToString());
                }
                if (TourDurationDropDown.SelectedValue == "0")
                {
                    MaximumDays = null;
                    MinimumDays = null;
                }
                else
                {
                    MinimumDays = int.Parse(TourDurationDropDown.SelectedValue.Substring(0, TourDurationDropDown.SelectedValue.ToString().IndexOf("-")));
                    MaximumDays = int.Parse(TourDurationDropDown.SelectedValue.Substring(TourDurationDropDown.SelectedValue.ToString().IndexOf("-") + 1));
                }
                if (ForeignerCheckBox.Checked == false)
                {
                    IndianNationality = true;
                }
                else
                {
                    IndianNationality = false;
                }
                dt = objPackage.GetPackageByCriteria(Session["BusDtl"].ToString().Split('|')[0].ToString(), DestinationID, PackageTypeID, MinimumDays, MaximumDays,IndianNationality, pageIndex, GridViewPackage.PageSize);
                if (dt.Rows.Count > 0)
                {
                    ViewState["TotalRecord"] = dt.Rows[0]["TotalRows"].ToString();
                }
                else
                {
                    ViewState["TotalRecord"] = "0";
                }
                GridViewPackage.DataSource = dt;
                GridViewPackage.DataBind();

            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Data From Server", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }
        
        #endregion

    }
}