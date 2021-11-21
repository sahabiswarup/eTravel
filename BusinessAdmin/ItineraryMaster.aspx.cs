using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using e_TravelBLL.TourPackage;
using System.Data;
using Sibin.ExceptionHandling.ExceptionHandler;
using SIBINUtility.ValidatorClass;

namespace e_Travel.BusinessAdmin
{
    public partial class ItineraryMaster : AdminBasePage
    {
        #region Private Variables
        ItineraryMstBLL objItineraryMstBLL;
        VehicleTypeMasterBLL objVehicleTypeMasterBLL;
        #endregion

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            ItineraryCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(ItineraryCustomGridView_SelectedIndexChanging);
            ItineraryCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(ItineraryCustomGridView_RowDeleting);
            ItineraryCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void ItineraryCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                ClearInputControls();
                DisableInputControls();
                Session["OpernType"] = null;
                ItineraryHeadingTextBox.Text = HttpUtility.HtmlDecode(ItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
                ItineraryDetailTextBox.Text = ItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItineraryDetail"].ToString();
                if (HttpUtility.HtmlDecode(ItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text.ToString()) == "Yes")
                {
                    YesRAPApplicableRadioButton.Checked = true;
                    RAPPanel.Visible = true;
                    if (HttpUtility.HtmlDecode(ItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["IsPerPax"].ToString()) == "True")
                    {
                        RAPPerPaxCheckBox.Checked = true;
                    }
                    else
                    {
                        RAPPerPaxCheckBox.Checked = false;
                    }
                    RAPCostPriceTextBox.Text = HttpUtility.HtmlDecode(ItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["PermitCostPrice"].ToString());
                    RAPSellPriceTextBox.Text = HttpUtility.HtmlDecode(ItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["PermitSellPrice"].ToString());
                }
                else
                {
                    NoRAPApplicableRadioButton.Checked = true;
                }
                if (HttpUtility.HtmlDecode(ItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text.ToString()) == "Yes")
                {
                    YesForeignerAllowedRadioButton.Checked = true;
                }
                else
                {
                    NoForeignerAllowedRadioButton.Checked = true;
                }
                if (HttpUtility.HtmlDecode(ItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[4].Text.ToString()) == "Yes")
                {
                    YesBothRadioButton.Checked = true;
                    VehicleDtlsSightSeenPanel.Visible = true;
                    VehicleDtlsTransferPanel.Visible = true;
                    VehicleDtlsPanel.Visible = false;
                }
                else
                {
                    NoBothRadioButton.Checked = true;
                    VehicleDtlsSightSeenPanel.Visible = false;
                    VehicleDtlsTransferPanel.Visible = false;
                    VehicleDtlsPanel.Visible = true;
                }
                GetAllVehicleCostDetailsByItinearyID(HttpUtility.HtmlDecode(ItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItineraryID"].ToString()));
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Details", "Error", DialogTypes.Error);
            }
        }

        protected void ItineraryCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objItineraryMstBLL = new ItineraryMstBLL();
            try
            {
                int status = 0;
                objItineraryMstBLL.DeleteItinerary(ItineraryCustomGridView.DataKeys[e.RowIndex].Values["ItineraryID"].ToString(), Page.User.Identity.Name, ref status);
                if (status == 0)
                {
                    ItineraryCustomGridView.BindData();
                    WebMessenger.Show(this, "Itinerary  Deleted Successfully", "Deleted", DialogTypes.Success);
                }
                else
                {
                    WebMessenger.Show(this, "Itineary Cannot Be Deleted As It Is Reference By Other Records", "Information", DialogTypes.Information);
                }
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                WebMessenger.Show(this, "Unable To Delete Itinerary  At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                Session["OpernType"] = null;
                ClearInputControls();
                DisableInputControls();
                objItineraryMstBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objItineraryMstBLL = new ItineraryMstBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (ItineraryCustomGridView.PageIndex * ItineraryCustomGridView.PageSize) + 1;
                dtRec = objItineraryMstBLL.GetAllItineraryPaged(startRowIndex, ItineraryCustomGridView.PageSize, Session["BusDtl"].ToString().Split('|')[0].ToString(),"A",null);
                if (dtRec.Rows.Count > 0)
                {
                    ItineraryCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Itinerary List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objItineraryMstBLL = null;
            }
            return dtRec;
        }
        #endregion

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
                ItineraryCustomGridView.AddGridViewColumns("Itinerary Heading |RAP Applicable| Foreigner Allowed|Include Both", "ItineraryHeading|RapApplicable|ForeignerAllowed|IncludeBoth", "600|70|70|70", "ItineraryID|ItineraryDetail|IsPerPax|PermitCostPrice|PermitSellPrice", true, true);
                ItineraryCustomGridView.BindData();
                LoadAllVehicleType();
                DisableInputControls();
                ClearInputControls();
            }

        }
        #endregion

        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            EnableInputControls();
            ClearInputControls();
            ItineraryHeadingTextBox.Focus();
            this.Session["OpernType"] = "Add";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (ItineraryCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                EnableInputControls();
                ItineraryHeadingTextBox.Focus();
                this.Session["OpernType"] = "Edit";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objItineraryMstBLL = new ItineraryMstBLL();

            #region Validation
            bool RapApplicable = false, ForeignerAllowed = false, IncludeBoth = false;
            bool? IsRAPPerPax = null;
            decimal? RAPCostPrice = null, RAPSellPrice = null;
            string VehicleDetails = null;
            if (ItineraryHeadingTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Itinearay Heading Is Required ", "Information", DialogTypes.Information);
                ItineraryHeadingTextBox.Focus();
                return;
            }

            if (ItineraryDetailTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Itinerary Detail Is Required", "Information", DialogTypes.Information);
                ItineraryDetailTextBox.Focus();
                return;
            }
            if (YesRAPApplicableRadioButton.Checked == false && NoRAPApplicableRadioButton.Checked == false)
            {
                WebMessenger.Show(this, "Provide Whether RAP Is Applicable Or Not", "Information", DialogTypes.Information);
                return;
            }
            if (YesForeignerAllowedRadioButton.Checked == false && NoForeignerAllowedRadioButton.Checked == false)
            {
                WebMessenger.Show(this, "Provide Whether Foreigner Is Allowed Or Not", "Information", DialogTypes.Information);
                return;
            }
            if (YesBothRadioButton.Checked == false && NoBothRadioButton.Checked == false)
            {
                WebMessenger.Show(this, "Provide Whether The Itinerary Is Both Tranfer And Sight Seen Or Not", "Information", DialogTypes.Information);
                return;
            }
            if (YesRAPApplicableRadioButton.Checked == true)
            {
                RapApplicable = true;
            }
            else
            {
                RapApplicable = false;
            }
            if (YesForeignerAllowedRadioButton.Checked == true)
            {
                ForeignerAllowed = true;
            }
            else
            {
                ForeignerAllowed = false;
            }
            if (YesBothRadioButton.Checked == true)
            {
                IncludeBoth = true;
            }
            else
            {
                IncludeBoth = false;
            }
            if (RAPPanel.Visible == true)
            {
                if (RAPPerPaxCheckBox.Checked == true)
                {
                    IsRAPPerPax = true;
                }
                else
                {
                    IsRAPPerPax = false;
                }
                if (RAPCostPriceTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "RAP Cost Price Is Required", "Information", DialogTypes.Information);
                    return;
                }
                else if (ValidatorClass.IsNumeric(RAPCostPriceTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "RAP Cost Is InValid Format", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    RAPCostPrice = decimal.Parse(RAPCostPriceTextBox.Text.Trim());
                }
                if (RAPSellPriceTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "RAP Selling Price Is Required", "Information", DialogTypes.Information);
                    return;
                }
                else if (ValidatorClass.IsNumeric(RAPSellPriceTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "RAP Selling Price Is InValid Format", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    RAPSellPrice = decimal.Parse(RAPSellPriceTextBox.Text.Trim());
                }
            }
            if (VehicleDtlsPanel.Visible == true)
            {

                foreach (GridViewRow row in VehicleDtlsGridView.Rows)
                {
                    string vehicleid = null, vehicleName = null, note = null;
                    decimal costrate = 0, sellrate = 0;
                    CheckBox cbk = (CheckBox)row.FindControl("IsApplicableCheckBox");
                    vehicleid = ((Label)row.FindControl("VehicleTypeIDLabel")).Text.ToString();
                    vehicleName = ((Label)row.FindControl("VehicleNameLabel")).Text.ToString();
                    if (((TextBox)row.FindControl("NoteTextBox")).Text.Trim() != "")
                    {
                        note = ((TextBox)row.FindControl("NoteTextBox")).Text.Trim();
                    }
                    if (cbk.Checked == true)
                    {
                        if (((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim() == "")
                        {
                            WebMessenger.Show(this, "Provide Cost Rate for Vehicle :" + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                        else if (!ValidatorClass.IsNumeric(((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim()))
                        {
                            WebMessenger.Show(this, "Invalid Input In Cost Price  Field of Vehicle " + vehicleName, "Invalid", DialogTypes.Information);
                            return;
                        }
                        if (((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim() == "")
                        {
                            WebMessenger.Show(this, "Provide Selling  Rate for Vehicle :" + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                        else if (!ValidatorClass.IsNumeric(((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim()))
                        {
                            WebMessenger.Show(this, "Invalid Input In Selling Price  Field of Vehicle " + vehicleName, "Invalid", DialogTypes.Information);
                            return;
                        }
                        costrate = decimal.Parse(((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim());
                        sellrate = decimal.Parse(((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim()); 
                    }
                    else
                    {
                        if (note == null)
                        {
                            WebMessenger.Show(this, "Note Is Required For Selected for " + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }   
                    }
                    VehicleDetails = VehicleDetails + vehicleid + "," + cbk.Checked + ",NA," + costrate + "," + sellrate + "," + note + "|";
                }
            }
            if (VehicleDtlsTransferPanel.Visible == true)
            {
              
                foreach (GridViewRow row in VehicleTransferGridView.Rows)
                {
                    string vehicleid = null, vehicleName = null, note = null;
                    decimal costrate = 0, sellrate = 0;
                    vehicleid = ((Label)row.FindControl("VehicleTypeIDLabel")).Text.ToString();
                    vehicleName = ((Label)row.FindControl("VehicleNameLabel")).Text.ToString();
                    CheckBox cbk = (CheckBox)row.FindControl("IsTransferApplicableCheckBox");
                    if(((TextBox)row.FindControl("NoteTextBox")).Text.Trim()!="")
                    {
                        note= ((TextBox)row.FindControl("NoteTextBox")).Text.Trim();
                    }
                    if (cbk.Checked == true)
                    {
                        if (((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim() == "")
                        {
                            WebMessenger.Show(this, "Provide Cost Rate for Vehicle :" + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                        else if (!ValidatorClass.IsNumeric(((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim()))
                        {
                            WebMessenger.Show(this, "Invalid Input In Cost Price  Field of Vehicle " + vehicleName, "Invalid", DialogTypes.Information);
                            return;
                        }
                        if (((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim() == "")
                        {
                            WebMessenger.Show(this, "Provide Selling  Rate for Vehicle :" + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                        else if (!ValidatorClass.IsNumeric(((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim()))
                        {
                            WebMessenger.Show(this, "Invalid Input In Selling Price  Field of Vehicle " + vehicleName, "Invalid", DialogTypes.Information);
                            return;
                        }
                        costrate = decimal.Parse(((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim());
                        sellrate = decimal.Parse(((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim()); 
                    }

                    else
                    {
                        if (note == null)
                        {
                            WebMessenger.Show(this, "Note Is Required For Selected for " + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                    }
                    VehicleDetails = VehicleDetails + vehicleid + "," + cbk.Checked + ",TR," + costrate + "," + sellrate + "," + note + "|";
                }
            }
            if (VehicleDtlsSightSeenPanel.Visible == true)
            {
                foreach (GridViewRow row in VehicleSightSeenGridView.Rows)
                {
                    string vehicleid = null, vehicleName = null, note = null;
                    decimal costrate = 0, sellrate = 0;
                    vehicleid = ((Label)row.FindControl("VehicleTypeIDLabel")).Text.ToString();
                    vehicleName = ((Label)row.FindControl("VehicleNameLabel")).Text.ToString();
                    CheckBox cbk = (CheckBox)row.FindControl("IsSightSeenApplicableCheckBox");
                    if (((TextBox)row.FindControl("NoteTextBox")).Text.Trim() != "")
                    {
                        note = ((TextBox)row.FindControl("NoteTextBox")).Text.Trim();
                    }
                    if (cbk.Checked == true)
                    {
                        if (((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim() == "")
                        {
                            WebMessenger.Show(this, "Provide Cost Rate for Vehicle :" + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                        else if (!ValidatorClass.IsNumeric(((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim()))
                        {
                            WebMessenger.Show(this, "Invalid Input In Cost Price  Field of Vehicle " + vehicleName, "Invalid", DialogTypes.Information);
                            return;
                        }
                        if (((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim() == "")
                        {
                            WebMessenger.Show(this, "Provide Selling  Rate for Vehicle :" + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                        else if (!ValidatorClass.IsNumeric(((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim()))
                        {
                            WebMessenger.Show(this, "Invalid Input In Selling Price  Field of Vehicle " + vehicleName, "Invalid", DialogTypes.Information);
                            return;
                        }
                        costrate = decimal.Parse(((TextBox)row.FindControl("CostPriceTextBox")).Text.ToString().Trim());
                        sellrate = decimal.Parse(((TextBox)row.FindControl("SellPriceTextBox")).Text.ToString().Trim());

                    }
                    else
                    {
                        if (note == null)
                        {
                            WebMessenger.Show(this, "Note Is Required For Selected " + vehicleName, "Information", DialogTypes.Information);
                            return;
                        }
                    }
                    VehicleDetails = VehicleDetails + vehicleid + "," + cbk.Checked + ",SS," + costrate + "," + sellrate + "," + note + "|";
                }
            }

            #endregion
            try
            {
                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            int record = 0;
                            objItineraryMstBLL.AddItinerary(Session["BusDtl"].ToString().Split('|')[0].ToString(), ItineraryHeadingTextBox.Text.Trim(), ItineraryDetailTextBox.Text.Trim(), RapApplicable, ForeignerAllowed, IncludeBoth, false, IsRAPPerPax, RAPCostPrice, RAPSellPrice, VehicleDetails, Page.User.Identity.Name, ref record);
                            if (record == 0)
                            {
                                ItineraryCustomGridView.BindData();
                                ClearInputControls();
                                DisableInputControls();
                                Session["OpernType"] = null;
                                WebMessenger.Show(this, "Itinerary Added Sucessfully", "Success", DialogTypes.Success);
                            }
                            else
                            {
                                WebMessenger.Show(this, "Same Itinerary Cannot Be Added Twice", "Information", DialogTypes.Information);
                                return;
                            }
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            int record = 0;
                            objItineraryMstBLL.EditItinerary(ItineraryCustomGridView.SelectedDataKey.Values["ItineraryID"].ToString(), ItineraryHeadingTextBox.Text.Trim(), ItineraryDetailTextBox.Text.Trim(), RapApplicable, ForeignerAllowed, IncludeBoth, false, IsRAPPerPax, RAPCostPrice, RAPSellPrice, VehicleDetails, Page.User.Identity.Name, ref record);
                            if (record == 0)
                            {
                                ItineraryCustomGridView.BindData();
                                ClearInputControls();
                                DisableInputControls();
                                Session["OpernType"] = null;
                                WebMessenger.Show(this, "Itinerary Updated Sucessfully", "Success", DialogTypes.Success);
                            }
                            else
                            {
                                WebMessenger.Show(this, "Same Itinerary Cannot Be Added Twice", "Information", DialogTypes.Information);
                                return;
                            }
                        }
                        else
                        {
                            WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                        }
                    }
                    else
                    {
                        WebMessenger.Show(this, "Select An Operation First", "Information", DialogTypes.Information);
                        return;
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                    ClearInputControls();
                    DisableInputControls();
                    Session["OpernType"] = null;
                }
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }
            finally
            {
                objItineraryMstBLL = null;
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearInputControls();
            DisableInputControls();
            this.Session["OpernType"] = null;
        }
        #endregion

        #region Enable/Disable/Clear Form Elements
        private void EnableInputControls()
        {
            ItineraryHeadingTextBox.Enabled = true;
            ItineraryDetailTextBox.Enabled = true;
            YesRAPApplicableRadioButton.Enabled = true;
            NoRAPApplicableRadioButton.Enabled = true;
            YesForeignerAllowedRadioButton.Enabled = true;
            NoForeignerAllowedRadioButton.Enabled = true;
            YesBothRadioButton.Enabled = true;
            NoBothRadioButton.Enabled = true;
            RAPPerPaxCheckBox.Enabled = true;
            RAPSellPriceTextBox.Enabled = true;
            RAPCostPriceTextBox.Enabled = true;

            foreach (GridViewRow row in VehicleDtlsGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Enabled = true;
                ((TextBox)row.FindControl("SellPriceTextBox")).Enabled = true;
                ((CheckBox)row.FindControl("IsApplicableCheckBox")).Enabled =true;
                ((TextBox)row.FindControl("NoteTextBox")).Enabled = true;
            }
            foreach (GridViewRow row in VehicleTransferGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Enabled = true;
                ((TextBox)row.FindControl("SellPriceTextBox")).Enabled = true;
                ((CheckBox)row.FindControl("IsTransferApplicableCheckBox")).Enabled = true;
                ((TextBox)row.FindControl("NoteTextBox")).Enabled = true;
            }
            foreach (GridViewRow row in VehicleSightSeenGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Enabled = true;
                ((TextBox)row.FindControl("SellPriceTextBox")).Enabled = true;
                ((CheckBox)row.FindControl("IsSightSeenApplicableCheckBox")).Enabled = true;
                ((TextBox)row.FindControl("NoteTextBox")).Enabled = true;
            }

        }

        private void DisableInputControls()
        {
            ItineraryHeadingTextBox.Enabled = false;
            ItineraryDetailTextBox.Enabled = false;
            YesRAPApplicableRadioButton.Enabled = false;
            NoRAPApplicableRadioButton.Enabled = false;
            YesForeignerAllowedRadioButton.Enabled = false;
            NoForeignerAllowedRadioButton.Enabled = false;
            YesBothRadioButton.Enabled = false;
            NoBothRadioButton.Enabled = false;
            RAPCostPriceTextBox.Enabled = false;
            RAPSellPriceTextBox.Enabled = false;
            RAPPerPaxCheckBox.Enabled = false;

            foreach (GridViewRow row in VehicleDtlsGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Enabled = false;
                ((TextBox)row.FindControl("SellPriceTextBox")).Enabled = false;
                ((CheckBox)row.FindControl("IsApplicableCheckBox")).Enabled = false;
                ((TextBox)row.FindControl("NoteTextBox")).Enabled = false;
            }
            foreach (GridViewRow row in VehicleTransferGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Enabled = false;
                ((TextBox)row.FindControl("SellPriceTextBox")).Enabled = false;
                ((CheckBox)row.FindControl("IsTransferApplicableCheckBox")).Enabled = false;
                ((TextBox)row.FindControl("NoteTextBox")).Enabled = false;
            }
            foreach (GridViewRow row in VehicleSightSeenGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Enabled = false;
                ((TextBox)row.FindControl("SellPriceTextBox")).Enabled = false;
                ((CheckBox)row.FindControl("IsSightSeenApplicableCheckBox")).Enabled = false;
                ((TextBox)row.FindControl("NoteTextBox")).Enabled = false;
            }
        }

        private void ClearInputControls()
        {
            ItineraryHeadingTextBox.Text = "";
            ItineraryDetailTextBox.Text = "";
            YesRAPApplicableRadioButton.Checked = false;
            NoRAPApplicableRadioButton.Checked = false;
            YesForeignerAllowedRadioButton.Checked = false;
            NoForeignerAllowedRadioButton.Checked = false;
            YesBothRadioButton.Checked = false;
            NoBothRadioButton.Checked = false;
            RAPPanel.Visible = false;
            RAPPerPaxCheckBox.Checked = true;
            RAPSellPriceTextBox.Text = "";
            RAPCostPriceTextBox.Text = "";
            VehicleDtlsPanel.Visible = false;
            VehicleDtlsSightSeenPanel.Visible = false;
            VehicleDtlsTransferPanel.Visible = false;

            foreach (GridViewRow row in VehicleDtlsGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Text = "";
                ((TextBox)row.FindControl("SellPriceTextBox")).Text = "";
                ((TextBox)row.FindControl("NoteTextBox")).Text = "";

            }
            foreach (GridViewRow row in VehicleTransferGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Text = "";
                ((TextBox)row.FindControl("SellPriceTextBox")).Text = "";
                ((TextBox)row.FindControl("NoteTextBox")).Text = "";
            }
            foreach (GridViewRow row in VehicleSightSeenGridView.Rows)
            {
                ((TextBox)row.FindControl("CostPriceTextBox")).Text = "";
                ((TextBox)row.FindControl("SellPriceTextBox")).Text = "";
                ((TextBox)row.FindControl("NoteTextBox")).Text = "";
            }

        }
        #endregion

        #region Load All Vehicle Type
        private void LoadAllVehicleType()
        {
            objVehicleTypeMasterBLL = new VehicleTypeMasterBLL();
            try
            {
                DataTable dt = objVehicleTypeMasterBLL.GetVehicleType();
                dt.Columns.Add(new DataColumn("ItinerarySellPrice", typeof(string)));
                dt.Columns.Add(new DataColumn("ItineraryCostPrice", typeof(string)));
                dt.Columns.Add(new DataColumn("IsApplicable", typeof(string)));
                dt.Columns.Add(new DataColumn("VehicleNote", typeof(string)));
                
                VehicleTransferGridView.DataSource = dt;
                VehicleTransferGridView.DataBind();

                VehicleSightSeenGridView.DataSource = dt;
                VehicleSightSeenGridView.DataBind();

                VehicleDtlsGridView.DataSource = dt;
                VehicleDtlsGridView.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Vehicle Type At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objVehicleTypeMasterBLL = null;
            }

        }
        #endregion

        #region Check BusinessID

       
        #endregion

        #region Conditional Event For RAP And Vehicle Cost 
        protected void IsApplicableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = sender as CheckBox;
            if (btn.Checked == false)
            {
                int iRowIndex = (((CheckBox)sender).Parent.Parent as GridViewRow).RowIndex;
                ((TextBox)VehicleDtlsGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Enabled = false;
                ((TextBox)VehicleDtlsGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Enabled = false;
                ((TextBox)VehicleDtlsGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Text = "0.00";
                ((TextBox)VehicleDtlsGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Text = "0.00";

            }
            else
            {
                int iRowIndex = (((CheckBox)sender).Parent.Parent as GridViewRow).RowIndex;
                ((TextBox)VehicleDtlsGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Enabled = true;
                ((TextBox)VehicleDtlsGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Enabled = true;
            }
        }

        protected void IsTransferApplicableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = sender as CheckBox;
            if (btn.Checked == false)
            {
                int iRowIndex = (((CheckBox)sender).Parent.Parent as GridViewRow).RowIndex;
                ((TextBox)VehicleTransferGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Enabled = false;
                ((TextBox)VehicleTransferGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Enabled = false;
                ((TextBox)VehicleTransferGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Text = "0.00";
                ((TextBox)VehicleTransferGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Text = "0.00";
            }
            else
            {
                int iRowIndex = (((CheckBox)sender).Parent.Parent as GridViewRow).RowIndex;
                ((TextBox)VehicleTransferGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Enabled = true;
                ((TextBox)VehicleTransferGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Enabled = true;
            }
        }
        protected void IsSightSeenApplicableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = sender as CheckBox;
            if (btn.Checked == false)
            {
                int iRowIndex = (((CheckBox)sender).Parent.Parent as GridViewRow).RowIndex;
                ((TextBox)VehicleSightSeenGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Enabled = false;
                ((TextBox)VehicleSightSeenGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Enabled = false;
                ((TextBox)VehicleSightSeenGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Text = "0.00";
                ((TextBox)VehicleSightSeenGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Text = "0.00";
            }
            else
            {
                int iRowIndex = (((CheckBox)sender).Parent.Parent as GridViewRow).RowIndex;
                ((TextBox)VehicleSightSeenGridView.Rows[iRowIndex].FindControl("CostPriceTextBox")).Enabled = true;
                ((TextBox)VehicleSightSeenGridView.Rows[iRowIndex].FindControl("SellPriceTextBox")).Enabled = true;
            }
        }
        protected bool IsPerPax(string PerPax)
        {
            if (PerPax == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void YesRAPApplicableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RAPPanel.Visible = true;
        }

        protected void NoRAPApplicableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RAPPanel.Visible = false;
        }

        protected void YesBothRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            VehicleDtlsTransferPanel.Visible = true;
            VehicleDtlsSightSeenPanel.Visible = true;
            VehicleDtlsPanel.Visible = false;
        }

        protected void NoBothRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            VehicleDtlsTransferPanel.Visible = false;
            VehicleDtlsSightSeenPanel.Visible = false;
            VehicleDtlsPanel.Visible = true;
        }
        protected void GetAllVehicleCostDetailsByItinearyID(string ItinearyID)
        {
            objItineraryMstBLL = new ItineraryMstBLL();
            try
            {
                DataTable foundVehicle = new DataTable();
                DataTable dt = objItineraryMstBLL.GetAllVehicleCostDetaislByItineraryID(ItinearyID);
                if (dt.Rows.Count > 0)
                {
                    if (VehicleDtlsPanel.Visible == true)
                    {
                        foreach (GridViewRow row in VehicleDtlsGridView.Rows)
                        {
                            Int64 vehicleID = Int64.Parse(((Label)row.FindControl("VehicleTypeIDLabel")).Text);
                            foundVehicle = null;
                            if (dt.Select("VehicleTypeID=" + vehicleID).Count() > 0)
                            {
                                foundVehicle = dt.Select("VehicleTypeID=" + vehicleID).CopyToDataTable();
                            }
                            if (foundVehicle != null)
                            {
                                if (foundVehicle.Rows.Count > 0)
                                {

                                    CheckBox cbk = ((CheckBox)row.FindControl("IsApplicableCheckBox"));
                                    cbk.Checked = bool.Parse(foundVehicle.Rows[0]["IsApplicable"].ToString());
                                    ((TextBox)row.FindControl("NoteTextBox")).Text = foundVehicle.Rows[0]["VehicleNote"].ToString();
                                    if (cbk.Checked == true)
                                    {
                                        ((TextBox)row.FindControl("CostPriceTextBox")).Text = foundVehicle.Rows[0]["ItineraryCostPrice"].ToString();
                                        ((TextBox)row.FindControl("SellPriceTextBox")).Text = foundVehicle.Rows[0]["ItinerarySellPrice"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (GridViewRow row in VehicleTransferGridView.Rows)
                        {
                            DataTable dtTR = dt.Select("ItineraryType='TR'").CopyToDataTable();
                            Int64 vehicleID = Int64.Parse(((Label)row.FindControl("VehicleTypeIDLabel")).Text);
                            foundVehicle = null;
                            if (dtTR.Select("VehicleTypeID=" + vehicleID).Count() > 0)
                            {
                               
                                foundVehicle = dtTR.Select("VehicleTypeID=" + vehicleID).CopyToDataTable();
                            }
                            if (foundVehicle != null)
                            {
                                if (foundVehicle.Rows.Count > 0)
                                {

                                    CheckBox cbk = ((CheckBox)row.FindControl("IsTransferApplicableCheckBox"));
                                    cbk.Checked = bool.Parse(foundVehicle.Rows[0]["IsApplicable"].ToString());
                                    ((TextBox)row.FindControl("NoteTextBox")).Text = foundVehicle.Rows[0]["VehicleNote"].ToString();
                                    if (cbk.Checked == true)
                                    {
                                        ((TextBox)row.FindControl("CostPriceTextBox")).Text = foundVehicle.Rows[0]["ItineraryCostPrice"].ToString();
                                        ((TextBox)row.FindControl("SellPriceTextBox")).Text = foundVehicle.Rows[0]["ItinerarySellPrice"].ToString();
                                    }
                                }
                            }
                        }
                        foreach (GridViewRow row in VehicleSightSeenGridView.Rows)
                        {
                            Int64 vehicleID = Int64.Parse(((Label)row.FindControl("VehicleTypeIDLabel")).Text);
                            DataTable dtSS = dt.Select("ItineraryType='SS'").CopyToDataTable();
                            foundVehicle = null;
                            if (dtSS.Select("VehicleTypeID=" + vehicleID).Count() > 0)
                            {
                                foundVehicle = dtSS.Select("VehicleTypeID=" + vehicleID).CopyToDataTable();
                            }
                            if (foundVehicle != null)
                            {
                                if (foundVehicle.Rows.Count > 0)
                                {
                                    CheckBox cbk = ((CheckBox)row.FindControl("IsSightSeenApplicableCheckBox"));
                                    cbk.Checked = bool.Parse(foundVehicle.Rows[0]["IsApplicable"].ToString());
                                    ((TextBox)row.FindControl("NoteTextBox")).Text = foundVehicle.Rows[0]["VehicleNote"].ToString();
                                    if (cbk.Checked == true)
                                    {
                                        ((TextBox)row.FindControl("CostPriceTextBox")).Text = foundVehicle.Rows[0]["ItineraryCostPrice"].ToString();
                                        ((TextBox)row.FindControl("SellPriceTextBox")).Text = foundVehicle.Rows[0]["ItinerarySellPrice"].ToString();
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Vehicle Cost Details By Itineary", "Error", DialogTypes.Error);
            }
        }

        protected bool IsApplicable(string IsApplicable)
        {
            if (IsApplicable == "")
            {
                return true;
            }
            else if (IsApplicable == "True")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}