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
    public partial class ConfigAccWiseRoomRate : AdminBasePage
    {
        #region Private Variables
        AccommodationMstBLL objBusAdmAccommodationMstBLL;//For Accomodation Type
        DestinationMasterBLL objPtlAdmDestinationMasterBLL;
        RoomPlanMstBLL objPtlAdmRoomPlanMstBLL;//For Room Plan Name
       
       
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
               
                    ETravelCustomGridView.AddGridViewColumns("Accomodation Name|Accomadation Type|Room Plan Name|Destination Name", "AccName|AccTypeName|RoomPlanName|DestinationName", "200|200|100|100", "AccRateID|AccTypeID|AccID|DestinationID|RoomPlanID|SingleRoomSellRate|DoubleRoomSellRate|ExtraBedSellRate|ExtraChildCost|ExtraChildSellRate|SingleRoomCost|DoubleRoomCost|ExtraBedCost", true, true);
                    ETravelCustomGridView.BindData();
                    LoadAccomodation();
                    LoadRoomPlanName();
                    Clearall();
                    InputEnableDisable(false);
                    Session["OperationType"] = null;
            }
        }
        #endregion

        #region Load Accomodation Type in Dropdown

        //private void LoadAccomodationType()
        //{
        //    objBusAdmAccommodationMstBLL = new BusAdmAccommodationMstBLL();
        //    try
        //    {
        //        AccTypeDropDown.Items.Clear();
        //        AccTypeDropDown.Items.Add(new ListItem("------------------------------------------------------------Select Accomodation Type----------------------------------------------------------", "0"));
        //        AccTypeDropDown.DataSource = objBusAdmAccommodationMstBLL.GetAllAccTypeList();
        //        AccTypeDropDown.DataTextField = "AccTypeName";
        //        AccTypeDropDown.DataValueField = "AccTypeID";
        //        AccTypeDropDown.DataBind();
        //    }
        //    catch (Exception)
        //    {
        //        WebMessenger.Show(this, "Error While Loading Accomodation Type", "Error", DialogTypes.Error);
        //        return;
        //    }
        //    finally
        //    {
        //        objBusAdmAccommodationMstBLL = null;
        //    }

        //}

        #endregion


        #region Load Room Plan Name in DropDown
        private void LoadRoomPlanName()
        {
            objPtlAdmRoomPlanMstBLL = new RoomPlanMstBLL();
            try
            {
                RoomPlanDropDown.Items.Clear();
                RoomPlanDropDown.Items.Add(new ListItem("------------------------------------------------------------Select Room Plan--------------------------------------------------------------------", "0"));
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

        #region Load Accomadtion in DropDown
        private void LoadAccomodation()
        {

            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                DataTable dt = objBusAdmAccommodationMstBLL.GetAccommodation(Session["BusDtl"].ToString().Split('|')[0].ToString());
                AccDropDown.Items.Clear();
                AccDropDown.Items.Add(new ListItem("------------------------------------------------------------Select Accomodation--------------------------------------------------------------------", "0"));
                AccDropDown.DataSource = dt;
                AccDropDown.DataValueField = "AccID";
                AccDropDown.DataTextField = "AccName";
                AccDropDown.DataBind();
                ViewState["AccomadationDetails"] = dt;
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading Accomodation", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusAdmAccommodationMstBLL = null;
            }
        }
        #endregion

        #region Page Function
        private void Clearall()
        {
           // AccTypeDropDown.SelectedValue="0";
            AccDropDown.SelectedValue="0";
           // DestinationDropDown.SelectedValue="0";
            RoomPlanDropDown.SelectedValue = "0";
            SingleRoomCostPriceTextBox.Text = "";
            SingleRoomSellPriceTextBox.Text = "";
            DoubleRoomCostPriceTextBox.Text = "";
            DoubleRoomSellPriceTextBox.Text = "";
            ExtraBedCostPriceTextBox.Text = "";
            ExtraBedSellPriceTextBox.Text = "";
            ExtraChildCostPriceTextBox.Text = "";
            ExtraChildSellPriceTextBox.Text = "";
        }
        private void InputEnableDisable(bool isEnabled)
        {
           // AccTypeDropDown.Enabled = isEnabled;
            AccDropDown.Enabled = isEnabled;
            //DestinationDropDown.Enabled = isEnabled;
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
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                Decimal? SingleRoomCostPrice = null;
                Decimal? DoubleRoomCostPrice = null;
                Decimal? ExtraChildRoomCharge = null;
                Decimal? ExtraBedCostPrice = null;
                #region Validation
                //if (AccTypeDropDown.SelectedIndex <= 0)
                // {
                //     WebMessenger.Show(this, "Please Select Accomodation Type", "Required", DialogTypes.Information);
                //     return;
                // }

                //  if (DestinationDropDown.SelectedIndex <= 0)
                //  {
                //     WebMessenger.Show(this, "Please Select Destination", "Required", DialogTypes.Information);
                //     return;
                //  }
                if (RoomPlanDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Room Plan", "Required", DialogTypes.Information);
                    return;
                }
                if (AccDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Accomodation", "Required", DialogTypes.Information);
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

                DataTable dt = (((DataTable)ViewState["AccomadationDetails"]).Select("AccID='" + AccDropDown.SelectedValue + "'")).CopyToDataTable();
                Int64 accType = 0, destinationID = 0;
                if (dt.Rows.Count > 0)
                {
                    accType = Int64.Parse(dt.Rows[0]["AccTypeID"].ToString());
                    destinationID = Int64.Parse(dt.Rows[0]["DestinationID"].ToString());
                }

                #endregion

                if (Session["OperationType"] != null)
                {
                    if (Session["OperationType"].ToString() == "ADD")
                    {
                        objBusAdmAccommodationMstBLL.AddAccWiseRoomRate(AccDropDown.SelectedValue, Session["BusDtl"].ToString().Split('|')[0].ToString(), accType, destinationID, long.Parse(RoomPlanDropDown.SelectedValue), SingleRoomCostPrice, DoubleRoomCostPrice, decimal.Parse(SingleRoomSellPriceTextBox.Text.Trim()), decimal.Parse(DoubleRoomSellPriceTextBox.Text.Trim()), ExtraBedCostPrice, decimal.Parse(ExtraBedSellPriceTextBox.Text.Trim()), ExtraChildRoomCharge, decimal.Parse(ExtraChildSellPriceTextBox.Text.Trim()), null, null, Page.User.Identity.Name);
                        WebMessenger.Show(this.Page, "Record Saved Successfully", "Success", DialogTypes.Success);
                        ETravelCustomGridView.BindData();
                    }
                    else if (Session["OperationType"].ToString() == "UPDATE")
                    {
                        objBusAdmAccommodationMstBLL.EditAccWiseRoomRate(ViewState["AccRateID"].ToString(), AccDropDown.SelectedValue, Session["BusDtl"].ToString().Split('|')[0].ToString(), accType, destinationID, long.Parse(RoomPlanDropDown.SelectedValue), SingleRoomCostPrice, DoubleRoomCostPrice, decimal.Parse(SingleRoomSellPriceTextBox.Text.Trim()), decimal.Parse(DoubleRoomSellPriceTextBox.Text.Trim()), ExtraBedCostPrice, decimal.Parse(ExtraBedSellPriceTextBox.Text.Trim()), ExtraChildRoomCharge, decimal.Parse(ExtraChildSellPriceTextBox.Text.Trim()), null, null, Page.User.Identity.Name);
                        WebMessenger.Show(this.Page, "Record Updated Successfully", "Success", DialogTypes.Success);
                        ETravelCustomGridView.BindData();
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Select A Operation First", "Information", DialogTypes.Information);
                    return;
                }
                Clearall();
                InputEnableDisable(false);
                Session["OperationType"] = null;
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
                WebMessenger.Show(this.Page, "Error Occured", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusAdmAccommodationMstBLL = null;
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            Clearall();
            InputEnableDisable(false);
            
        }
        #endregion

        #region Gridview Events and Load Gridview
        private DataTable LoadGridView()
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (ETravelCustomGridView.PageIndex * ETravelCustomGridView.PageSize) + 1;
                dt = objBusAdmAccommodationMstBLL.GetAllAccWiseRoomRatePaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, ETravelCustomGridView.PageSize);
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
                objBusAdmAccommodationMstBLL = null;
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
                ViewState["AccRateID"] = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccRateID"].ToString();
                //AccTypeDropDown.SelectedValue = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccTypeID"].ToString();
               AccDropDown.SelectedValue = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccID"].ToString();
                   // DestinationDropDown.SelectedValue = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["DestinationID"].ToString();
                    RoomPlanDropDown.SelectedValue = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["RoomPlanID"].ToString();
                    SingleRoomCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["SingleRoomCost"].ToString());
                    SingleRoomSellPriceTextBox.Text = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["SingleRoomSellRate"].ToString();
                    DoubleRoomCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["DoubleRoomCost"].ToString());
                    DoubleRoomSellPriceTextBox.Text = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["DoubleRoomSellRate"].ToString();
                    ExtraBedCostPriceTextBox.Text = HttpUtility.HtmlDecode(ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraBedCost"].ToString());
                    ExtraBedSellPriceTextBox.Text = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraBedSellRate"].ToString();
                    ExtraChildCostPriceTextBox.Text = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraChildCost"].ToString();
                    ExtraChildSellPriceTextBox.Text = ETravelCustomGridView.DataKeys[e.NewSelectedIndex].Values["ExtraChildSellRate"].ToString();
                    
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
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {

                objBusAdmAccommodationMstBLL.DeleteAccWiseRoomRate(ETravelCustomGridView.DataKeys[e.RowIndex].Values["AccRateID"].ToString(), Page.User.Identity.Name);
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
                objBusAdmAccommodationMstBLL = null;
            }
        }
       
        #endregion
    }
}