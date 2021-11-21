using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using e_TravelBLL.TourPackage;
using System.Data;
using SIBINUtility.ValidatorClass;
using Sibin.ExceptionHandling.ExceptionHandler;
using System.Text;
using System.Data.SqlClient;

namespace e_Travel.BusinessAdmin
{
    public partial class ConfigDiscount : AdminBasePage
    {
        #region Private Variables
        ConfigDiscountBLL objBusAdmConfigDiscountBLL;
        //static DataTable dt;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
            try
            {
                BindEvents();
                if (Session["BusDtl"] == null)
                {
                    Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
                }
                if (!IsPostBack)
                {

                        tpkSeachCustomGridView.AddGridViewColumns("Tpk ID|Pkg. Name|Tot. Days|Tot. Nights", "TpkID|TpkName|TotalDays|TotalNights", "60|280|60|60", "TpkID|BusinessID", true, false);
                        ETravelCustomGridView.AddGridViewColumns("Transaction ID|Discount Description|Applicable From|Applicable Upto", "TransactionID|DiscountDesc|EffectiveFrom|EffectiveUpto", "90|450|90|90", "TransactionID", true, true);
                        tpkSeachCustomGridView.BindData();
                        ETravelCustomGridView.BindData();
                        Clearall();
                        InputEnableDisable(false);
                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error occured while processsing..", "Error", DialogTypes.Error);
            }
            finally
            {

            }

        }
        #endregion

        #region Page Function
        private void Clearall()
        {

            ViewState["Dt"] = null;
            BindSecondGrid();
            EffectiveDateTextBox.Text = string.Empty;
            EndDateDateTextBox.Text = string.Empty;
            //VehicleDiscountTextBox.Text = string.Empty;
            //AccTextBox.Text = string.Empty;
            DiscountTextBox.Text = string.Empty;
            DiscountDisTextBox.Text = string.Empty;
            Session["OperationType"] = null;
            //ETravelCustomGridView.BindData();

        }
        private void InputEnableDisable(bool isEnable)
        {
            //VehicleDiscountTextBox.Enabled = isEnable;
            //AccTextBox.Enabled = isEnable;
            DiscountTextBox.Enabled = isEnable;
            EffectiveDateTextBox.Enabled = isEnable;
            EndDateDateTextBox.Enabled = isEnable;
            DiscountDisTextBox.Enabled = isEnable;
        }
        #endregion

      

        private void BindEvents()
        {
            #region Bind All GridView events

            #region Bind  CustomGridView with search
            tpkSeachCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(SearchTpkCustomGridView_SelectedIndexChanging);
            tpkSeachCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(SearchTpkCustomGridView_RowDeleting);
            tpkSeachCustomGridView.DataSource += new SIBINUtility.UserControl.SearchGridView.ucSearchGridView.GridViewDataSource(LoadTpkSearchCustomGridView);
            tpkSeachCustomGridView.ucGridView_SearchButtonClick += new EventHandler(tpkSeachCustomGridView_ucGridView_SearchButtonClick);
            //tpkSeachCustomGridView.ucGridView_UserTextChanged += new EventHandler(SearchTpkCustomGridView_ucGridView_UserTextChanged);
            #endregion

            #region Bind Custom GridView without Search
            ETravelCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(ETravelCustomGridView_SelectedIndexChanging);
            ETravelCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(ETravelCustomGridView_RowDeleting);
            ETravelCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadETravelGridView);
            #endregion

            #endregion
        }

        protected void tpkSeachCustomGridView_ucGridView_SearchButtonClick(object sender, EventArgs e)
        {
            Clearall();
            tpkSeachCustomGridView.BindData();
        }

        #region ETravelCustomGridView All Events operation Without Search and Load ETravelCustomGridView

        protected void ETravelCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                Session["OperationType"] = null;
                //VehicleDiscountTextBox.Text = string.Empty;
                //AccTextBox.Text = string.Empty;
                DiscountTextBox.Text = string.Empty;
                objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();

                DataTable dt = objBusAdmConfigDiscountBLL.SelectDiscountDetailsByTID(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["TransactionID"].ToString(), Session["BusDtl"].ToString().Split('|')[0].ToString(), "D");
                if (ViewState["Dt"] != null)
                {
                    ViewState["Dt"] = null;
                }
                ViewState["Dt"] = dt;
                BindSecondGrid();
                dt = null;

                dt = objBusAdmConfigDiscountBLL.SelectDiscountDetailsByTID(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["TransactionID"].ToString(), Session["BusDtl"].ToString().Split('|')[0].ToString(), "A");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //if (!string.IsNullOrEmpty(dr["DiscountFor"].ToString()) && !string.IsNullOrWhiteSpace(dr["DiscountFor"].ToString()) && dr["DiscountFor"].ToString() != string.Empty && dr["DiscountFor"].ToString() == "VH")
                        //{
                        //    VehicleDiscountTextBox.Text = dr["DiscountPercent"].ToString().Trim();
                        //}
                        //if (!string.IsNullOrEmpty(dr["DiscountFor"].ToString()) && !string.IsNullOrWhiteSpace(dr["DiscountFor"].ToString()) && dr["DiscountFor"].ToString() != string.Empty && dr["DiscountFor"].ToString() == "AC")
                        //{
                        //    AccTextBox.Text = dr["DiscountPercent"].ToString().Trim();
                        //}

                        if (!string.IsNullOrEmpty(dr["DiscountFor"].ToString()) && !string.IsNullOrWhiteSpace(dr["DiscountFor"].ToString()) && dr["DiscountFor"].ToString() != string.Empty && dr["DiscountFor"].ToString() == "PK")
                        {
                            DiscountTextBox.Text = dr["DiscountPercent"].ToString().Trim();
                        }

                    }
                }
                EffectiveDateTextBox.Text = ETravelCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text;
                EndDateDateTextBox.Text = ETravelCustomGridView.Rows[e.NewSelectedIndex].Cells[4].Text;
                DiscountDisTextBox.Text = ETravelCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text.Replace("&nbsp;", "");
                EditButton.Enabled = true;
                InputEnableDisable(false);
                //if (!string.IsNullOrEmpty(VehicleDiscountTextBox.Text) || !string.IsNullOrEmpty(AccTextBox.Text))
                //{
                //    DiscountDisTextBox.Enabled = false;
                //}
                //if (!string.IsNullOrEmpty(DiscountDisTextBox.Text))
                //{
                //    VehicleDiscountTextBox.Enabled = false;
                //    AccTextBox.Enabled = false;
                //}


            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                WebMessenger.Show(this, "Error in Record Navigating", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmConfigDiscountBLL = null;
            }
        }

        protected void ETravelCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
            try
            {

                objBusAdmConfigDiscountBLL.DeleteDiscountDetails(ETravelCustomGridView.DataKeys[e.RowIndex].Values["TransactionID"].ToString(), Page.User.Identity.Name);
                WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
                ETravelCustomGridView.BindData();

            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OperationType"] = null;
                WebMessenger.Show(this, "Record Could Not Delete", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmConfigDiscountBLL = null;
            }
        }

        private DataTable LoadETravelGridView()
        {
            objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (ETravelCustomGridView.PageIndex * ETravelCustomGridView.PageSize) + 1;
                dt = objBusAdmConfigDiscountBLL.GetDiscountDetails(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, ETravelCustomGridView.PageSize, "", "D");
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
                objBusAdmConfigDiscountBLL = null;
            }
            return dt;
        }
        #endregion

        #region tpkSeachCustomGridView All Events operation with Search and Load tpkSeachCustomGridView

        protected void SearchTpkCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            AddRowToDataTable(tpkSeachCustomGridView.DataKeys[e.NewSelectedIndex].Values["TpkID"].ToString(), tpkSeachCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
            BindSecondGrid();
            InputEnableDisable(false);
        }

        protected void SearchTpkCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
            try
            {

                objBusAdmConfigDiscountBLL.DeleteDiscountDetails(ETravelCustomGridView.DataKeys[e.RowIndex].Values["TransactionID"].ToString(), Page.User.Identity.Name);
                WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
                ETravelCustomGridView.BindData();

            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OperationType"] = null;
                WebMessenger.Show(this, "Record Could Not Delete", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmConfigDiscountBLL = null;
            }
        }
   
        private DataTable LoadTpkSearchCustomGridView()
        {
            objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (tpkSeachCustomGridView.PageIndex * tpkSeachCustomGridView.PageSize) + 1;
                dt = objBusAdmConfigDiscountBLL.GetDiscountDetails(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, tpkSeachCustomGridView.PageSize, tpkSeachCustomGridView.TextValue, "A");
                if (dt.Rows.Count > 0)
                {
                    tpkSeachCustomGridView.TotalRecordCount = int.Parse(dt.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Record From Server", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmConfigDiscountBLL = null;
            }
            return dt;

        }

        #endregion

        #region Move selected GridView row to another Gridview
        protected void BindSecondGrid()
        {
            DataTable dt = (DataTable)ViewState["Dt"];
            selectedtpkGridView.DataSource = dt;
            selectedtpkGridView.DataBind();
        }


        protected void AddRowToDataTable(string TpkId, string PackageName)
        {
            if (ViewState["Dt"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("TpkID");
                dt.Columns.Add("TpkName");

                DataRow dr = dt.NewRow();
                dr["TpkID"] = TpkId;
                dr["TpkName"] = PackageName;
                dt.Rows.Add(dr);
                ViewState["Dt"] = dt;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["Dt"];
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["TpkID"].ToString() == TpkId)
                    {
                        WebMessenger.Show(this.Page, "The selected tour package is already added to the discount list...", "Information", DialogTypes.Information);
                        return;
                    }
                }
                DataRow dr = dt.NewRow();
                dr["TpkID"] = TpkId;
                dr["TpkName"] = PackageName;
                dt.Rows.Add(dr);
                ViewState["Dt"] = dt;
            }
        }
        #endregion

        #region All Text Changed Events
        //protected void VehicleDiscountTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(VehicleDiscountTextBox.Text) || !string.IsNullOrEmpty(VehicleDiscountTextBox.Text))
        //    {
        //        //if (!System.Text.RegularExpressions.Regex.IsMatch(VehicleDiscountTextBox.Text, @"^(\d{1,2}(\.\d{1,4})?|(100))$"))
        //        //{
        //        //    WebMessenger.Show(this,"Invalid input..", "Inforamtion", DialogTypes.Information);
        //        //    VehicleDiscountTextBox.Text = string.Empty;
        //        //    return;
        //        //}
        //        DiscountTextBox.Enabled = false;
        //        DiscountTextBox.Text = string.Empty;

        //    }
        //    else
        //    {
        //        DiscountTextBox.Enabled = true;
        //        DiscountTextBox.Text = string.Empty;
        //    }
        //}

        //protected void AccTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(AccTextBox.Text) || !string.IsNullOrEmpty(VehicleDiscountTextBox.Text))
        //    {

        //        DiscountTextBox.Enabled = false;
        //        DiscountTextBox.Text = string.Empty;

        //    }
        //    else
        //    {
        //        DiscountTextBox.Enabled = true;
        //        DiscountTextBox.Text = string.Empty;

        //    }
        //}

        //protected void DiscountTextBox_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(DiscountTextBox.Text))
        //    {

        //        AccTextBox.Enabled = false;
        //        VehicleDiscountTextBox.Enabled = false;
        //        AccTextBox.Text = string.Empty;
        //        VehicleDiscountTextBox.Text = string.Empty;

        //    }
        //    else
        //    {
        //        AccTextBox.Enabled = true;
        //        VehicleDiscountTextBox.Enabled = true;
        //    }
        //}
        #endregion

        #region Button Click Events

        protected void AddButton_Click(object sender, EventArgs e)
        {
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
                if (selectedtpkGridView.Rows.Count <= 0)
                {
                    WebMessenger.Show(this.Page, "Please select the tour package from list..", "Required", DialogTypes.Information);
                    return;
                }
                if (string.IsNullOrEmpty(EffectiveDateTextBox.Text))
                {
                    WebMessenger.Show(this.Page, "Please enter the effective date..", "Required", DialogTypes.Information);
                    EffectiveDateTextBox.Focus();
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(EffectiveDateTextBox.Text, @"^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"))
                {
                    WebMessenger.Show(this, "Invalid effective date..", "Inforamtion", DialogTypes.Information);
                    EffectiveDateTextBox.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(EndDateDateTextBox.Text))
                {
                    WebMessenger.Show(this.Page, "Please enter the End date..", "Required", DialogTypes.Information);
                    EndDateDateTextBox.Focus();
                    return;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(EndDateDateTextBox.Text, @"^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"))
                {
                    WebMessenger.Show(this, "Invalid end date..", "Inforamtion", DialogTypes.Information);
                    EndDateDateTextBox.Focus();
                    return;
                }
                if (!string.IsNullOrEmpty(EffectiveDateTextBox.Text) && !string.IsNullOrEmpty(EndDateDateTextBox.Text))
                {
                    if (DateTime.Parse(EffectiveDateTextBox.Text.Trim()) > (DateTime.Parse(EndDateDateTextBox.Text.Trim())))
                    {
                        WebMessenger.Show(this.Page, "Effective Date Cannot Be Greater Than Effective End Date", "Warning, Set The Date Properly", DialogTypes.Warning);
                        return;
                    }

                }

                if (string.IsNullOrEmpty(DiscountTextBox.Text))
                {
                    WebMessenger.Show(this.Page, "Please enter % of Discount Percent...", "Warning, Set The Date Properly", DialogTypes.Warning);
                    return;
                }
                //if (!string.IsNullOrEmpty(VehicleDiscountTextBox.Text) || !string.IsNullOrEmpty(AccTextBox.Text) || !string.IsNullOrEmpty(DiscountTextBox.Text))
                //{
                //    if (!string.IsNullOrEmpty(VehicleDiscountTextBox.Text))
                //    {
                //        if (!System.Text.RegularExpressions.Regex.IsMatch(VehicleDiscountTextBox.Text, @"^(\d{1,2}(\.\d{1,4})?|(100))$"))
                //        {
                //            WebMessenger.Show(this, "Invalid input..", "Inforamtion", DialogTypes.Information);
                //            VehicleDiscountTextBox.Focus();
                //            return;
                //        }
                //    }
                //    if (!string.IsNullOrEmpty(AccTextBox.Text))
                //    {
                //        if (!System.Text.RegularExpressions.Regex.IsMatch(AccTextBox.Text, @"^(\d{1,2}(\.\d{1,4})?|(100))$"))
                //        {
                //            WebMessenger.Show(this, "Invalid input..", "Inforamtion", DialogTypes.Information);
                //            AccTextBox.Focus();
                //            return;
                //        }
                //    }
                //    if (!string.IsNullOrEmpty(DiscountTextBox.Text))
                //    {
                //        if (!System.Text.RegularExpressions.Regex.IsMatch(DiscountTextBox.Text, @"^(\d{1,2}(\.\d{1,4})?|(100))$"))
                //        {
                //            WebMessenger.Show(this, "Invalid input..", "Inforamtion", DialogTypes.Information);
                //            DiscountTextBox.Focus();
                //            return;
                //        }
                //    }

                //}
                //if ((!string.IsNullOrEmpty(VehicleDiscountTextBox.Text) || !string.IsNullOrEmpty(AccTextBox.Text)) && !string.IsNullOrEmpty(DiscountTextBox.Text))
                //{
                //    WebMessenger.Show(this.Page, "Please enter valid discount option", "Warning, Set The Date Properly", DialogTypes.Warning);
                //    return;
                //}
       

                #endregion
                objBusAdmConfigDiscountBLL = new ConfigDiscountBLL();
                StringBuilder sb = new StringBuilder();
                if (selectedtpkGridView.Rows.Count > 0)
                {
                    foreach (GridViewRow gvr in selectedtpkGridView.Rows)
                    {
                        sb.Append(gvr.Cells[0].Text.Trim() + ",");
                    }
                }
                decimal? VHDis = null;
                decimal? AccDis = null;
                decimal? PKDis = null;

                //if (!string.IsNullOrEmpty(VehicleDiscountTextBox.Text))
                //{
                //    VHDis = decimal.Parse(VehicleDiscountTextBox.Text);
                //}
                //if (!string.IsNullOrEmpty(AccTextBox.Text))
                //{
                //    AccDis = decimal.Parse(AccTextBox.Text);
                //}
                if (!string.IsNullOrEmpty(DiscountTextBox.Text))
                {
                    PKDis = decimal.Parse(DiscountTextBox.Text);
                }
                if (Session["OperationType"] != null)
                {
                    if (Session["OperationType"] == null || Session["OperationType"].ToString() == "ADD")
                    {

                        objBusAdmConfigDiscountBLL.AddDiscountDetails(Session["BusDtl"].ToString().Split('|')[0].ToString(), sb.ToString().Substring(0, sb.Length - 1), VHDis, AccDis, PKDis, DateTime.Parse(EffectiveDateTextBox.Text.Trim()), DateTime.Parse(EndDateDateTextBox.Text.Trim()), DiscountDisTextBox.Text, Page.User.Identity.Name);
                        WebMessenger.Show(this, "Record saved successfully", "Success", DialogTypes.Success);

                    }
                    else if (Session["OperationType"] != null || Session["OperationType"].ToString() == "UPDATE")
                    {

                        objBusAdmConfigDiscountBLL.UpdatediscountDetails(Session["BusDtl"].ToString().Split('|')[0].ToString(), ETravelCustomGridView.SelectedDataKey["TransactionID"].ToString(), sb.ToString().Substring(0, sb.Length - 1), VHDis, AccDis, PKDis, DateTime.Parse(EffectiveDateTextBox.Text.Trim()), DateTime.Parse(EndDateDateTextBox.Text.Trim()), DiscountDisTextBox.Text, Page.User.Identity.Name);
                        WebMessenger.Show(this, "Record updated successfully", "Success", DialogTypes.Success);


                    }
                    else
                    {
                        WebMessenger.Show(this.Page, "Invalid Operation..", "Warning", DialogTypes.Warning);
                        return;
                    }
                    Clearall();
                    InputEnableDisable(false);
                    ETravelCustomGridView.BindData();
                }
                else
                {
                    WebMessenger.Show(this.Page, "Invalid Operation..", "Warning", DialogTypes.Warning);
                }
              

            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    WebMessenger.Show(this, "Tour package already exists in selected date range..", "Error", DialogTypes.Warning);
                }
            }
            catch (Exception ex)
            {

                WebMessenger.Show(this, "Error Occured..", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmConfigDiscountBLL = null;
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {

            Session["OperationType"] = null;
            Clearall();
        
        }
        #endregion

        protected void selectedtpkGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["Dt"];
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (dt.Rows[i]["TpkID"].ToString() == selectedtpkGridView.Rows[e.RowIndex].Cells[0].Text)
                {
                    dt.Rows.RemoveAt(i);
                    break;
                }
            }
            ViewState["Dt"] = dt;
            BindSecondGrid();
        }




    }
}