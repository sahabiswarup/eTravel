using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using e_TravelBLL.TourPackage;
using System.Data;
using Sibin.Utilities.Web.ExceptionHandling;
using SIBINUtility.ValidatorClass;
using System.Data.SqlClient;

namespace e_Travel.BusinessAdmin
{
    public partial class ConfigAccTypeWiseRoomRate : AdminBasePage
    {
        #region Private Variable

        AccommodationMstBLL objBusAdmAccommodationMstBLL;//For Accomodation Type
        DestinationMasterBLL objPtlAdmDestinationMasterBLL;//For Destination
        RoomPlanMstBLL objPtlAdmRoomPlanMstBLL;//For Room Plan Name
        AccommodationTypeMstBLL objAccommodationTypeMstBLL;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
            BindEvents();
         
            if (!IsPostBack)                
            {                
                    ETravelCustomGridView.AddGridViewColumns("Accomodation Type Name|Room Plan Name|Destination Name", "AccTypeName|RoomPlanName|DestinationName", "200|200|200", "AccRateID|AccTypeID|DestinationID|RoomPlanID|SingleRoomSellRate|DoubleRoomSellRate|ExtraBedSellRate|ExtraChildCost|ExtraChildSellRate|SingleRoomCost|DoubleRoomCost|ExtraBedCost", true, true);
                    ETravelCustomGridView.BindData();
                    LoadAccomodationType();
                    Clearall();
                    LoadDestination(null);
                    LoadRoomPlanName();
                    InputEnableDisable(false);
                    Session["OperationType"] = null;
            }
        }
        #endregion

        #region Gridview Events and Load Gridview
        private DataTable LoadGridView()
        {
            objAccommodationTypeMstBLL = new AccommodationTypeMstBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (ETravelCustomGridView.PageIndex * ETravelCustomGridView.PageSize) + 1;
                dt = objAccommodationTypeMstBLL.GetAllAccTypeWiseRoomRate(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, ETravelCustomGridView.PageSize);                
                if (dt.Rows.Count > 0)
                {
                    ETravelCustomGridView.TotalRecordCount = int.Parse(dt.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Accomodation Type Room Rate Details", "Error", DialogTypes.Error);
            }
            finally
            {
                objAccommodationTypeMstBLL = null;
            }
            return dt;
        }

        private void BindEvents()
        {
            ETravelCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(ETravelCustomGridView_SelectedIndexChanging);
            ETravelCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(ETravelCustomGridView_RowDeleting);
            ETravelCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void ETravelCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
           
            try
            {
                    Session["OperationType"] = null;
                    ViewState["AccRateID"] = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccRateID"].ToString());
                    AccTypeDropDown.SelectedValue = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccTypeID"].ToString());
                    DestinationDropDown.SelectedValue = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["DestinationID"].ToString());
                    RoomPlanDropDown.SelectedValue = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["RoomPlanID"].ToString());
                    SingleRoomCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["SingleRoomCost"].ToString());
                    SingleRoomSellPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["SingleRoomSellRate"].ToString());
                    DoubleRoomCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["DoubleRoomCost"].ToString());
                    DoubleRoomSellPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["DoubleRoomSellRate"].ToString());
                    ExtraBedCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraBedCost"].ToString());
                    ExtraBedSellPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraBedSellRate"].ToString());
                    ExtraChildCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraChildCost"].ToString());
                    ExtraChildSellPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraChildSellRate"].ToString());
                    EditButton.Enabled = true;
                    InputEnableDisable(false);


            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                WebMessenger.Show(this, "Error in Record Navigating", "Error", DialogTypes.Error);
            }
            
        }

        protected void ETravelCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objAccommodationTypeMstBLL = new AccommodationTypeMstBLL();
            try
            {
                objAccommodationTypeMstBLL.DeleteAccTypeWiseRoomRate(ETravelCustomGridView.DataKeys[e.RowIndex].Values["AccRateID"].ToString(), Page.User.Identity.Name);
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
                objAccommodationTypeMstBLL = null;
            }
        }
        #endregion

        #region Page Function
        private void Clearall()
        {
            SingleRoomCostPriceTextBox.Text = "";
            SingleRoomSellPriceTextBox.Text = "";
            DoubleRoomCostPriceTextBox.Text = "";
            DoubleRoomSellPriceTextBox.Text = "";
            ExtraBedCostPriceTextBox.Text = "";
            ExtraBedSellPriceTextBox.Text = "";
            ExtraChildCostPriceTextBox.Text = "";
            ExtraChildSellPriceTextBox.Text = "";
            AccTypeDropDown.SelectedValue = "0";
            DestinationDropDown.SelectedValue = "0";
            RoomPlanDropDown.SelectedValue = "0";
        }
        private void InputEnableDisable(bool isEnabled)
        {
            AccTypeDropDown.Enabled = isEnabled;
            DestinationDropDown.Enabled = isEnabled;
            RoomPlanDropDown.Enabled = isEnabled;
            SingleRoomCostPriceTextBox.Enabled = isEnabled;
            SingleRoomSellPriceTextBox.Enabled = isEnabled;
            DoubleRoomCostPriceTextBox.Enabled = isEnabled;
            DoubleRoomSellPriceTextBox.Enabled = isEnabled;
            ExtraBedCostPriceTextBox.Enabled = isEnabled;
            ExtraBedSellPriceTextBox.Enabled = isEnabled;
            ExtraChildCostPriceTextBox.Enabled = isEnabled;
            ExtraChildSellPriceTextBox.Enabled = isEnabled;
        }        
        #endregion

        #region Load Accomodation Type in Dropdown

        private void LoadAccomodationType()
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                AccTypeDropDown.Items.Clear();
                AccTypeDropDown.Items.Add(new ListItem("--------------------------------------------------------Select Accomodation----------------------------------------------------------------------------", "0"));
                AccTypeDropDown.DataSource = objBusAdmAccommodationMstBLL.GetAllAccTypeList();
                AccTypeDropDown.DataTextField = "AccTypeName";
                AccTypeDropDown.DataValueField = "AccTypeID";
                AccTypeDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error While Loading Accomodation Type", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusAdmAccommodationMstBLL = null;
            }

        }

        #endregion

        #region Load Destination
        private void LoadDestination(Int64? countryID)
        {
            objPtlAdmDestinationMasterBLL = new DestinationMasterBLL();
            try
            {
                DestinationDropDown.Items.Clear();
                DestinationDropDown.Items.Add(new ListItem("--------------------------------------------------------Select Destination----------------------------------------------------------------------------", "0"));
                DestinationDropDown.DataSource = objPtlAdmDestinationMasterBLL.GetDestination(countryID);
                DestinationDropDown.DataTextField = "DestinationName";
                DestinationDropDown.DataValueField = "DestinationID";
                DestinationDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error While Loading Destination", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmDestinationMasterBLL = null;
            }
        }
        #endregion

        #region Load Room Plan Name
        private void LoadRoomPlanName()
        {
            objPtlAdmRoomPlanMstBLL = new RoomPlanMstBLL();
            try
            {
                RoomPlanDropDown.Items.Clear();
                RoomPlanDropDown.Items.Add(new ListItem("--------------------------------------------------------Select Room Plan----------------------------------------------------------------------------", "0"));
                RoomPlanDropDown.DataSource = objPtlAdmRoomPlanMstBLL.GetRoomPlan();
                RoomPlanDropDown.DataValueField = "RoomPlanID";
                RoomPlanDropDown.DataTextField = "RoomPlanName";
                RoomPlanDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading Room Plan Name", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmRoomPlanMstBLL = null;
            }
        }
        #endregion

        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            Session["OperationType"] = "ADD";
            Clearall();
            InputEnableDisable(true);
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
                Session["OperationType"] = "UPDATE";
                InputEnableDisable(true);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objAccommodationTypeMstBLL = new AccommodationTypeMstBLL();
            try
            {
                Decimal? SingleRoomCostPrice = null;
                Decimal? DoubleRoomCostPrice = null;
                Decimal? ExtraChildRoomCharge = null;
                Decimal? ExtraBedCostPrice = null;

                #region Validation
                if (AccTypeDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Accomodation Type", "Required", DialogTypes.Information);
                    return;
                }

                if (DestinationDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Destination", "Required", DialogTypes.Information);
                    return;
                }
                if (RoomPlanDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Room Plan", "Required", DialogTypes.Information);
                    return;
                }
                if (SingleRoomCostPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(SingleRoomCostPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Single Room Cost Price", "Invalid", DialogTypes.Information);
                        return;
                    }
                    else
                    {
                        SingleRoomCostPrice = decimal.Parse(SingleRoomCostPriceTextBox.Text.Trim());
                    }
                }

                else
                {
                    SingleRoomCostPrice = null;
                }
                
               
              
                if (SingleRoomSellPriceTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Mention The Single Room Selling Price", "Required", DialogTypes.Information);
                    SingleRoomSellPriceTextBox.Focus();
                    return;
                }
                else if (SingleRoomSellPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(SingleRoomSellPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Single Room Selling Price Field", "Invalid", DialogTypes.Information);
                        return;
                    }
                }
                if (DoubleRoomCostPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(DoubleRoomCostPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Double Room Cost Price", "Invalid", DialogTypes.Information);
                        return;
                    }
                    else
                    {
                        DoubleRoomCostPrice = decimal.Parse(DoubleRoomCostPriceTextBox.Text.Trim());
                    }
                }
                else
                {
                    DoubleRoomCostPrice = null;
                }
                if (DoubleRoomSellPriceTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Mention The Double Room Selling Price", "Required", DialogTypes.Information);
                    SingleRoomSellPriceTextBox.Focus();
                    return;
                }
                else if (DoubleRoomSellPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(DoubleRoomSellPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Double Room Selling Price Field", "Invalid", DialogTypes.Information);
                        return;
                    }
                }
                if (ExtraBedCostPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(ExtraBedCostPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Extra Bed Cost Price", "Invalid", DialogTypes.Information);
                        return;
                    }
                    else
                    {
                        ExtraBedCostPrice = decimal.Parse(ExtraBedCostPriceTextBox.Text.Trim());
                    }
                }
                else
                {
                    ExtraBedCostPrice = null;
                }
                if (ExtraBedSellPriceTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Mention The Extra Bed Selling Price", "Required", DialogTypes.Information);
                    ExtraBedSellPriceTextBox.Focus();
                    return;
                }
                else if (ExtraBedSellPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(ExtraBedSellPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Extra Bed Charge For Selling", "Invalid", DialogTypes.Information);
                        return;
                    }
                }
                if (ExtraChildSellPriceTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Mention The Selling Price For Extra Child Room Charge", "Required", DialogTypes.Information);
                    ExtraChildSellPriceTextBox.Focus();
                    return;
                }
                else if (ExtraChildSellPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(ExtraChildSellPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Extra Child Room Charge For Selling", "Invalid", DialogTypes.Information);
                        return;
                    }
                }
                if (ExtraChildCostPriceTextBox.Text.Trim() != "")
                {
                    if (!ValidatorClass.IsNumeric(ExtraChildCostPriceTextBox.Text.Trim()))
                    {
                        WebMessenger.Show(this, "Invalid Input In Extra Child Room Charge", "Invalid", DialogTypes.Information);
                        return;
                    }
                    else
                    {
                        ExtraChildRoomCharge = decimal.Parse(ExtraChildCostPriceTextBox.Text.Trim());
                    }
                }
                else
                {
                    ExtraChildRoomCharge = null;
                }
                
                #endregion
                if (Session["OperationType"] != null)
                {
                    if (Session["OperationType"].ToString() == "ADD")
                    {
                        objAccommodationTypeMstBLL.AddAccTypeWiseRoomRate(Session["BusDtl"].ToString().Split('|')[0].ToString(), long.Parse(AccTypeDropDown.SelectedValue.ToString()), long.Parse(DestinationDropDown.SelectedValue.ToString()), long.Parse(RoomPlanDropDown.SelectedValue.ToString()), SingleRoomCostPrice, DoubleRoomCostPrice, decimal.Parse(SingleRoomSellPriceTextBox.Text.Trim()), decimal.Parse(DoubleRoomSellPriceTextBox.Text.Trim()), ExtraBedCostPrice, decimal.Parse(ExtraBedSellPriceTextBox.Text.Trim()), ExtraChildRoomCharge, decimal.Parse(ExtraChildSellPriceTextBox.Text.Trim()), null, null, Page.User.Identity.Name);
                        WebMessenger.Show(this.Page, "Record Saved Successfully", "Success", DialogTypes.Success);
                        ETravelCustomGridView.BindData();
                    }
                    else if (Session["OperationType"].ToString() == "UPDATE")
                    {
                        objAccommodationTypeMstBLL.EditAccTypeWiseRoomRate(ViewState["AccRateID"].ToString(), Session["BusDtl"].ToString().Split('|')[0].ToString(), long.Parse(AccTypeDropDown.SelectedValue.ToString()), long.Parse(DestinationDropDown.SelectedValue.ToString()), long.Parse(RoomPlanDropDown.SelectedValue.ToString()), SingleRoomCostPrice, DoubleRoomCostPrice, decimal.Parse(SingleRoomSellPriceTextBox.Text.Trim()), decimal.Parse(DoubleRoomSellPriceTextBox.Text.Trim()), ExtraBedCostPrice, decimal.Parse(ExtraBedSellPriceTextBox.Text.Trim()), ExtraChildRoomCharge, decimal.Parse(ExtraChildSellPriceTextBox.Text.Trim()), null, null, Page.User.Identity.Name);
                        WebMessenger.Show(this.Page, "Record Updated Successfully", "Success", DialogTypes.Success);
                        ETravelCustomGridView.BindData();
                    }
                    Clearall();
                    InputEnableDisable(false);
                    Session["OperationType"] = null;
                }

                else
                {
                    WebMessenger.Show(this, "Select A Operation First", "Information", DialogTypes.Information);
                    return;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    WebMessenger.Show(this, "The Following Rate Has Already Been Configured", "Inforrmation", DialogTypes.Information);
                    return;
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objAccommodationTypeMstBLL = null;
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Clearall();
            InputEnableDisable(false);
            
            Session["OperationType"] = null;
        }
       
        #endregion
    }
}