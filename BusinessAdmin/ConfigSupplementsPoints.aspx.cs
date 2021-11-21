using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;

using SIBINUtility.ValidatorClass;
using Sibin.ExceptionHandling.ExceptionHandler;

using e_Travel.Class;
using e_TravelBLL.TourPackage;

namespace e_Travel.BusinessAdmin
{
    public partial class ConfigSupplementsPoints : AdminBasePage
    {
        SupplementPointsBLL objSupplementPointsBLL;
        ItineraryMstBLL objItineraryMstBLL;

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            BindEvents();
            if (Session["BusDtl"] == null)
            {
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
            if (!IsPostBack)
            {
                ItinerarySearchCustomGridView.AddGridViewColumns("Itinerary", "ItineraryHeading", "400", "ItineraryID|ItineraryDetail", true, false);
                ItinerarySearchCustomGridView.BindData();
                LoadAllSupplementPoints();
                ItinerarySearchCustomGridView.SearchLabelText = "Search By Itinerary";
                ETravelCustomGridView.AddGridViewColumns("Supplement Name|Available In Itinerary|Effective From|Effective Upto", "SupplementName|ItineraryCount|EffectiveFrom|EffectiveUpto", "200|150", "SupplementPointID|TransactionID|CostPrice|SellingPrice", true, true);
                ETravelCustomGridView.BindData();
                Clearall();
                InputEnableDisable(false);
            }
        }
        #endregion

        #region Page Function
        private void Clearall()
        {
            ViewState["Dt"] = null;
            BindSecondGrid();
            FromDateTextBox.Text = "";
            UptoDateTextBox.Text = "";
            CostPriceTextBox.Text = "";
            SellingPriceTextBox.Text = "";
            SuppPointsDropDownList.SelectedValue = "0";
        }
        private void InputEnableDisable(bool isEnable)
        {
            FromDateTextBox.Enabled = isEnable;
            UptoDateTextBox.Enabled = isEnable;
            CostPriceTextBox.Enabled = isEnable;
            SellingPriceTextBox.Enabled = isEnable;
            SuppPointsDropDownList.Enabled = isEnable;
            SelectedItineraryGridView.Enabled = isEnable;
            
           
        }
        #endregion



        private void BindEvents()
        {
            #region Bind All GridView events

            #region Bind  CustomGridView with search
            ItinerarySearchCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(ItinerarySearchCustomGridView_SelectedIndexChanging);
            //ItinerarySearchCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(ItinerarySearchCustomGridView_RowDeleting);
            ItinerarySearchCustomGridView.DataSource += new SIBINUtility.UserControl.SearchGridView.ucSearchGridView.GridViewDataSource(LoadItinerarySearchCustomGridView);
            ItinerarySearchCustomGridView.ucGridView_SearchButtonClick += new EventHandler(ItinerarySearchCustomGridView_ucGridView_SearchButtonClick);
            #endregion

            #region Bind Custom GridView without Search
            ETravelCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(ETravelCustomGridView_SelectedIndexChanging);
            // ETravelCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(ETravelCustomGridView_RowDeleting);
            ETravelCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadETravelGridView);
            #endregion

            #endregion
        }

        void ItinerarySearchCustomGridView_ucGridView_SearchButtonClick(object sender, EventArgs e)
        {
            Clearall();
            ItinerarySearchCustomGridView.BindData();
        }

        #region ETravelCustomGridView All Events operation Without Search and Load ETravelCustomGridView

        protected void ETravelCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OperationType"] = null;
            SuppPointsDropDownList.SelectedValue = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["SupplementPointID"].ToString());
            FromDateTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text);
            UptoDateTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.Rows[e.NewSelectedIndex].Cells[4].Text);
            CostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["CostPrice"].ToString());
            SellingPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["SellingPrice"].ToString());
            ViewState["TransactionID"] = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["TransactionID"].ToString());
            GetItineraryListByTransactionID(ViewState["TransactionID"].ToString());
        }

       

        private DataTable LoadETravelGridView()
        {
            objSupplementPointsBLL = new SupplementPointsBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (ETravelCustomGridView.PageIndex * ETravelCustomGridView.PageSize) + 1;
                dt = objSupplementPointsBLL.GetAllItinerarySuppPointsPaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, ETravelCustomGridView.PageSize);
                if (dt.Rows.Count > 0)
                {
                    ETravelCustomGridView.TotalRecordCount = int.Parse(dt.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Record From Server", "Error", DialogTypes.Error);
            }
            finally
            {
                objSupplementPointsBLL = null;
            }
            return dt;
        }
        #endregion

        #region tpkSeachCustomGridView All Events operation with Search and Load ItinerarySearchCustomGridView

        protected void ItinerarySearchCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (SelectedItineraryGridView.Enabled == false)
            {
                WebMessenger.Show(this, "Select A Operation First ", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                AddRowToDataTable(ItinerarySearchCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItineraryID"].ToString(), ItinerarySearchCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text, ItinerarySearchCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItineraryDetail"].ToString());
                BindSecondGrid();
            }
           // InputEnableDisable(false);

        }

        //protected void SearchTpkCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
        //    try
        //    {

        //        objBusAdmConfigDiscountBLL.DeleteDiscountDetails(ETravelCustomGridView.DataKeys[e.RowIndex].Values["TransactionID"].ToString(), Page.User.Identity.Name);
        //        WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
        //        ETravelCustomGridView.BindData();

        //    }
        //    catch (Exception ex)
        //    {
        //        UserInterfaceExceptionHandler.HandleException(ref ex);
        //        Session["OperationType"] = null;
        //        WebMessenger.Show(this, "Record Could Not Delete", "Error", DialogTypes.Error);
        //    }
        //    finally
        //    {
        //        objBusAdmConfigDiscountBLL = null;
        //    }
        //}



        private DataTable LoadItinerarySearchCustomGridView()
        {
            string searchText = null;
            objItineraryMstBLL = new ItineraryMstBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (ItinerarySearchCustomGridView.PageIndex * ItinerarySearchCustomGridView.PageSize) + 1;
                if (ItinerarySearchCustomGridView.TextValue.Trim() == "")
                {
                    searchText = null;
                }
                else
                {
                    searchText = ItinerarySearchCustomGridView.TextValue.Trim();
                }
                dt = objItineraryMstBLL.GetAllItineraryPaged(startRowIndex, ItinerarySearchCustomGridView.PageSize, Session["BusDtl"].ToString().Split('|')[0].ToString(), "S", searchText);
                if (dt.Rows.Count > 0)
                {
                    ItinerarySearchCustomGridView.TotalRecordCount = int.Parse(dt.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Record From Server", "Error", DialogTypes.Error);
            }
            finally
            {
                objItineraryMstBLL = null;
            }
            return dt;

        }


        #endregion

        #region Move selected GridView row to another Gridview
        protected void BindSecondGrid()
        {
            DataTable dt = (DataTable)ViewState["Dt"];
            SelectedItineraryGridView.DataSource = dt;
            SelectedItineraryGridView.DataBind();
        }


        protected void AddRowToDataTable(string ItineraryID, string ItineraryName, string ItineraryDetail)
        {
            if (ViewState["Dt"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ItineraryID");
                dt.Columns.Add("ItineraryHeading");
                dt.Columns.Add("ItineraryDetail");

                DataRow dr = dt.NewRow();
                dr["ItineraryID"] = ItineraryID;
                dr["ItineraryHeading"] = ItineraryName;
                dr["ItineraryDetail"] = ItineraryDetail;
                dt.Rows.Add(dr);
                ViewState["Dt"] = dt;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["Dt"];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["ItineraryID"].ToString() == ItineraryID)
                    {
                        WebMessenger.Show(this.Page, "The selected Itinerary is already added to the  list...", "Information", DialogTypes.Information);
                        return;
                    }
                }
                DataRow dr = dt.NewRow();
                dr["ItineraryID"] = ItineraryID;
                dr["ItineraryHeading"] = ItineraryName;
                dr["ItineraryDetail"] = ItineraryDetail;
                dt.Rows.Add(dr);
                ViewState["Dt"] = dt;
            }
        }
        #endregion


        #region Button Click Events

        protected void AddButton_Click(object sender, EventArgs e)
        {
            Clearall();
            InputEnableDisable(true);
            Session["OperationType"] = "ADD";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (ETravelCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                InputEnableDisable(true);
                Session["OperationType"] = "UPDATE";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

            try
            {
                #region server side Validation

                DataTable dt = new DataTable();
                if (SelectedItineraryGridView.Rows.Count<=0)
                {
                    WebMessenger.Show(this.Page, "Please select the Itinerary  from list..", "Required", DialogTypes.Information);
                    return;
                }
                if (SuppPointsDropDownList.SelectedValue == "0")
                {
                    WebMessenger.Show(this, "Please Select The Supplement Points For Selected Itinerary.. ", "Required", DialogTypes.Information);
                    return;
                }
                if(FromDateTextBox.Text.Trim()=="")
                {
                    WebMessenger.Show(this,"Please Select The Date From The Supplement Is Available..  ","Required",DialogTypes.Information);
                    return;
                }
                if(UptoDateTextBox.Text.Trim()=="")
                {
                    WebMessenger.Show(this,"Please Select The Date Upto Which The Supplement Is Available... ","Required",DialogTypes.Information);
                    return;
                }
                if(CostPriceTextBox.Text.Trim()=="")
                {
                    WebMessenger.Show(this,"Please Provide Cost Price For The Supplement","Required",DialogTypes.Information);
                    return;
                }
                else if(ValidatorClass.IsNumeric(CostPriceTextBox.Text.Trim())== false)
                {
                    WebMessenger.Show(this,"Invalid Character In Cost Price ","Information",DialogTypes.Information);
                    return;
                }
                if(SellingPriceTextBox.Text.Trim()=="")
                {
                     WebMessenger.Show(this,"Please Provide Selling Price For The Supplement","Required",DialogTypes.Information);
                    return;
                }
                else if(ValidatorClass.IsNumeric(SellingPriceTextBox.Text.Trim())== false)
                {
                    WebMessenger.Show(this,"Invalid Character In Selling  Price ","Information",DialogTypes.Information);
                    return;
                }
              
                #endregion
                objSupplementPointsBLL = new SupplementPointsBLL();
                StringBuilder sb = new StringBuilder();
                if (SelectedItineraryGridView.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in SelectedItineraryGridView.Rows)
                    {
                        sb.Append(((Label)gvr.FindControl("ItineraryIDLabel")).Text + ",");
                    }
                }

                if (Session["OperationType"] != null)
                {
                    if (Session["OperationType"].ToString() == "ADD")
                    {
                        objSupplementPointsBLL.AddSupplementPointByItinerary(Session["BusDtl"].ToString().Split('|')[0].ToString(), sb.ToString(), SuppPointsDropDownList.SelectedValue, DateTime.Parse(FromDateTextBox.Text.Trim()), DateTime.Parse(UptoDateTextBox.Text.Trim()), decimal.Parse(CostPriceTextBox.Text.Trim()), decimal.Parse(SellingPriceTextBox.Text.Trim()), Page.User.Identity.Name);
                        ETravelCustomGridView.BindData();
                        WebMessenger.Show(this, "Supplement Point Successfully Configured For Selected Itinerary","Success" ,DialogTypes.Success);
                    }
                    else if (Session["OperationType"].ToString() == "UPDATE")
                    {
                        objSupplementPointsBLL.EditSupplementPointByItinerary(Session["BusDtl"].ToString().Split('|')[0].ToString(), ViewState["TransactionID"].ToString(), sb.ToString(), SuppPointsDropDownList.SelectedValue.ToString(), DateTime.Parse(FromDateTextBox.Text.Trim()), DateTime.Parse(UptoDateTextBox.Text.Trim()), decimal.Parse(CostPriceTextBox.Text.Trim()), decimal.Parse(SellingPriceTextBox.Text.Trim()), Page.User.Identity.Name);
                        ETravelCustomGridView.BindData();
                        WebMessenger.Show(this, "Supplement Point Successfully Configured For Selected Itinerary", "Success", DialogTypes.Success);
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Select A Operation First ", "Information", DialogTypes.Information);
                }
              

            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    WebMessenger.Show(this, "Itinerary  already exists in selected date range..", "Error", DialogTypes.Warning);
                }
            }
            catch (Exception ex)
            {

                WebMessenger.Show(this, "Error Occured..", "Error", DialogTypes.Error);
            }
            finally
            {
                objSupplementPointsBLL = null;
                Clearall();
                InputEnableDisable(false);
                ETravelCustomGridView.BindData();
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {

            Session["OperationType"] = null;
            Clearall();

        }
        #endregion

        protected void SelectedItineraryGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Dt"];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["ItineraryID"].ToString() == ((Label)SelectedItineraryGridView.Rows[e.RowIndex].FindControl("ItineraryIDLabel")).Text)
                {
                    dt.Rows.RemoveAt(i);
                    break;
                }
            }
            ViewState["Dt"] = dt;
            BindSecondGrid();
        }

        protected void GetItineraryListByTransactionID(string TransactionID)
        {
            objSupplementPointsBLL = new SupplementPointsBLL();
            try
            {
                DataTable dt = objSupplementPointsBLL.GetAllItineraryByTransactionID(TransactionID);
                if (ViewState["Dt"] != null)
                {
                    ViewState["Dt"] = null;
                }
                ViewState["Dt"] = dt;
                BindSecondGrid();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void LoadAllSupplementPoints()
        {
            objSupplementPointsBLL = new SupplementPointsBLL();
            try
            {
                DataTable dt = objSupplementPointsBLL.GetAllSupplmentPoints(Session["BusDtl"].ToString().Split('|')[0].ToString());
                SuppPointsDropDownList.Items.Clear();
                SuppPointsDropDownList.DataSource = dt;
                SuppPointsDropDownList.DataTextField = "SupplementName";
                SuppPointsDropDownList.DataValueField = "SupplementPointID";
                SuppPointsDropDownList.Items.Add(new ListItem("-------Select Points--------", "0"));
                SuppPointsDropDownList.DataBind();

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Supplement Points At This Website", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objSupplementPointsBLL = null;
            }
        }
       

    }



}