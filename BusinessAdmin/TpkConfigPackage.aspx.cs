using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using Sibin.Utilities.Web.ExceptionHandling;
using e_TravelBLL.TourPackage;
using SIBINUtility.ValidatorClass;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Sibin.FrameworkExtensions.DotNet.Web;
using Sibin.Imaging.TwoD;
using Sibin.Utilities.Imaging.TwoD;



namespace e_Travel.BusinessAdmin
{
   
    public partial class TpkConfigPackage : AdminBasePage
    {
        #region Private Variables
        TpkPackageMst objPackage;
        TourPackageTypeMstBLL objPtlAdmTourPackageMstBLL;
        TravelSegmentMstBLL objPtlAdmTravelSegmentMstBLL;
        DestinationMasterBLL objPtlAdmDestinationMasterBLL;
        
        ItineraryMstBLL objBusAdmItineraryMstBLL;
        bool overNightDesReq = false;
        #endregion

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            TpkGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(TpkGridView_SelectedIndexChanging);
            TpkGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(TpkGridView_RowDeleting);
            TpkGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);

            TpkPackagaeItineraryCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(TpkPackagaeItineraryCustomGridView_SelectedIndexChanging);
            TpkPackagaeItineraryCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(TpkPackagaeItineraryCustomGridView_RowDeleting);
            TpkPackagaeItineraryCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadItineraryDetailGridView);
        }


       
        protected void TpkItnDayNoDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableStepTwoControls();
            if (TpkItnDayNoDropDownList.SelectedValue != "0")
            {
                if (TpkItnDayNoDropDownList.Items.Count.ToString() == TpkItnDayNoDropDownList.SelectedValue)
                {
                    DestDropDownList.Enabled = false;
                    DestDropDownList.SelectedValue = "0";
                    TpkItnOverNightTextBox.Enabled = false;
                    TpkItnOverNightTextBox.Text = "Not Required";
                    NoAccomadationRequiredRadioButton.Enabled = false;
                    YesAccomadationRequiredRadioButton.Enabled = false;
                    NoAccomadationRequiredRadioButton.Checked = true;
                    YesAccomadationRequiredRadioButton.Checked = false;
                }

            }
        }
        protected void TpkGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            ViewState["TpkID"] = TpkGridView.DataKeys[e.NewSelectedIndex].Values["TpkID"].ToString();
            TpkNameTextBox.Text = HttpUtility.HtmlDecode(TpkGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
            TpkTypeDropDownList.SelectedValue = TpkGridView.DataKeys[e.NewSelectedIndex].Values["PackageTypeID"].ToString();
            TravelSegmentDropDownList.SelectedValue = TpkGridView.DataKeys[e.NewSelectedIndex].Values["TravelSegmentID"].ToString();
            TpkDescTextBox.Text = TpkGridView.DataKeys[e.NewSelectedIndex].Values["TpkDesc"].ToString();
            TpkDestCoveredTextBox.Text = TpkGridView.DataKeys[e.NewSelectedIndex].Values["TpkDestCovered"].ToString();
            TpkPickUpTextBox.Text = TpkGridView.DataKeys[e.NewSelectedIndex].Values["TpkPickUpPoint"].ToString();
            TpkDropTextBox.Text = TpkGridView.DataKeys[e.NewSelectedIndex].Values["TpkDropPoint"].ToString();
            TpkDaysTextBox.Text = HttpUtility.HtmlDecode(TpkGridView.Rows[e.NewSelectedIndex].Cells[4].Text);
            TpkNightsTextBox.Text = HttpUtility.HtmlDecode(TpkGridView.Rows[e.NewSelectedIndex].Cells[5].Text);
            if (TpkGridView.DataKeys[e.NewSelectedIndex].Values["AdditionalRemarks"].ToString() != string.Empty)
            {
                AdditionalCostCheckBox.Checked = true;
                AdditionalCostPanel.Visible = true;
                RemarksTextBox.Text = TpkGridView.DataKeys[e.NewSelectedIndex].Values["AdditionalRemarks"].ToString();
                AdditionalCostTextBox.Text = TpkGridView.DataKeys[e.NewSelectedIndex].Values["AdditionalCost"].ToString();
            }
            else
            {
                AdditionalCostCheckBox.Checked = false;
                AdditionalCostPanel.Visible = false;
                RemarksTextBox.Text = "";
                AdditionalCostTextBox.Text = "";
            }
        }
        protected void TpkPackagaeItineraryCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            TpkItnDayNoDropDownList.SelectedValue = HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            TpkActivityForDayTextBox.Text = HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
            SelectedItineraryHiddenField.Value = HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItineraryID"].ToString());
            ItineraryDetailTextBox.Text = HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItineraryDetail"].ToString());
            if (HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["DestinationID"].ToString()) == "")
            {
                DestDropDownList.SelectedValue = "0";
            }
            else
            {
                DestDropDownList.SelectedValue = HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.DataKeys[e.NewSelectedIndex].Values["DestinationID"].ToString());
            }
            TpkItnOverNightTextBox.Text = HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[4].Text);
            if (HttpUtility.HtmlDecode(TpkPackagaeItineraryCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text) == "True")
            {
                YesAccomadationRequiredRadioButton.Checked = true;
            }
            else
            {
                NoAccomadationRequiredRadioButton.Checked = true;
            }
        }


        protected void TpkGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                objPackage = new TpkPackageMst();
                try
                {
                    int Status = 0;
                    objPackage.DeleteTourPackage(TpkGridView.DataKeys[e.RowIndex].Values["TpkID"].ToString(), Page.User.Identity.Name,ref Status);
                    if (Status == 0)
                    {
                        TpkGridView.BindData();
                        WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
                    }
                    else
                    {
                        WebMessenger.Show(this, "Record Cannot Be Deleted As It Has Reference In Other Records", "Relation Exist", DialogTypes.Information);
                    }

                }
                catch (Exception ex)
                {
                    UserInterfaceExceptionHandler.HandleException(ref ex);
                    Session["OperationType"] = null;
                    WebMessenger.Show(this, "Record Could Not Delete", "Error", DialogTypes.Error);
                    return;
                }
                finally
                {
                    objPackage = null;
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        protected void TpkPackagaeItineraryCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objPackage = new TpkPackageMst();
            try
            {
                objPackage.DeleteTpkPackagaeItineraryDtl(TpkPackagaeItineraryCustomGridView.DataKeys[e.RowIndex].Values["TpkItineraryID"].ToString(), Page.User.Identity.Name);
                TpkPackagaeItineraryCustomGridView.BindData();
                WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OperationType"] = null;
                WebMessenger.Show(this, "Record Could Not Delete", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }

        private DataTable LoadGridView()
        {
            objPackage = new TpkPackageMst();
            DataTable dtRec = new DataTable();
            string TpkID = null, TpkName = null, BusinessID = Session["BusDtl"].ToString().Split('|')[0].ToString();
            Int64? PackageTypeID=null;
            Int32 IncompletePackageCount =0;
            Int32? InCompletePercentage = null;
            try
            {
                if (ViewState["InComplete"] != null)
                {
                    InCompletePercentage = Int32.Parse(ViewState["InComplete"].ToString());
                }
                int startRowIndex = (TpkGridView.PageIndex * TpkGridView.PageSize) + 1;
                if (TpkIDSearchTextBox.Text.Trim() != "")
                {
                    TpkID = TpkIDSearchTextBox.Text.Trim();
                }
                if (TpkSearchNameTextBox.Text.Trim() != "")
                {
                    TpkName = TpkSearchNameTextBox.Text.Trim();
                }
                if (TpkTypeSearchDropDownList.SelectedValue != "0" && TpkTypeSearchDropDownList.SelectedValue != "" && TpkTypeSearchDropDownList.SelectedValue!=null)
                {
                    PackageTypeID = Int64.Parse(TpkTypeSearchDropDownList.SelectedValue);
                }
                dtRec = objPackage.GetAllTourPackagePaged(startRowIndex, TpkGridView.PageSize, TpkID, TpkName, PackageTypeID, BusinessID, InCompletePercentage, ref IncompletePackageCount);
                if (dtRec.Rows.Count > 0)
                {
                    TpkGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                    InCompletePackageLinkButton.Text = IncompletePackageCount.ToString();
                   if(!IsPostBack)
                    {
                        CompletePackageLinkButton.Text = (int.Parse(dtRec.Rows[0]["TotalRows"].ToString()) - IncompletePackageCount).ToString();
                    }
                }
                if (ViewState["InComplete"] != null)
                {
                    ViewAllLinkButton.Visible = true;
                }
                else
                {
                    ViewAllLinkButton.Visible = false;
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Tour Package List At This Moment...", "Error", DialogTypes.Error);
               
            }
            finally
            {
                objPackage = null;
            }
            return dtRec;
        }

        private DataTable LoadItineraryDetailGridView()
        {
            objPackage = new TpkPackageMst();
            DataTable dtRec = new DataTable(); 
            try
            {
                int startRowIndex = (TpkGridView.PageIndex * TpkGridView.PageSize) + 1;

                dtRec = objPackage.GetAllTpkPackagaeItineraryDtlPaged(startRowIndex, TpkPackagaeItineraryCustomGridView.PageSize, Session["BusDtl"].ToString().Split('|')[0].ToString(), ViewState["TpkID"].ToString());
                if (dtRec.Rows.Count > 0)
                {
                    TpkPackagaeItineraryCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());                  
                    Session["DestinationID"] = dtRec.Rows[dtRec.Rows.Count-1]["DestinationID"].ToString();

                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Itinerary Detail List At This Moment...", "Error", DialogTypes.Error);
                return dtRec;
            }
            finally
            {
                objPackage = null;
            }
            return dtRec;
        }
        #endregion

        #region page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            BindEvents();
            if (Session["BusDtl"] == null)
            {
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
            Sibin.Utilities.Imaging.TwoD.ImageEditingUtility sec = new ImageEditingUtility();
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ImageCroppingScript"))
            {
                string str = sec.GetImageCroppingScriptClientCode();
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "ImageCroppingScript", str);
            }
            if (!IsPostBack)
            {                
                
                    ViewState["TpkID"] = 1;
                    Session["ucPhotoUploaderUploadedImage"] = null;
                    SearchPanel.Visible = true;
                    TpkWizardPanel.Visible = false;
                    TpkGridView.AddGridViewColumns("Package Code|Package Name|Package Type|No. of Days|No. of Nights|Complete", "TpkID|TpkName|PackageTypeName|TotalDays|TotalNights|CompletePercentage", "100|280|180|80|80|80", "TpkID|PackageTypeID|TravelSegmentID|TpkDesc|TpkDestCovered|TpkPickUpPoint|TpkDropPoint|AdditionalRemarks|AdditionalCost", true, true);
                    TpkGridView.BindData();
                    TpkPackagaeItineraryCustomGridView.AddGridViewColumns("DayNo|Itinerary Heading|Over Night AccRequired|OverNightDestNote", "DayNo|ItineraryHeading|OverNightAccRequired|OverNightDestNote", "100|380|150|100", "TpkItineraryID|TpkID|ItineraryID|DestinationID|ItineraryDetail", true, true);
                    TpkPackagaeItineraryCustomGridView.BindData();
                    LoadAllPackageType();
                    LoadTravelSegment();
                    LoadDestination();
                    AutosuggestItinerary();
                   // LoadItnType();
                    DisableStepTwoControls();
                    DisableStepThreeControls();
            }
            CheckImage();
        }
        #endregion

        #region Wizard Previous Button Click Event
        protected void Wizard1_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == 0)
            {
                
            }
            else if (e.NextStepIndex == 1)
            {
                
            }
            else if (e.NextStepIndex == 2)
            {
                
            }
        }
        #endregion

        #region Wizard Next Button Click Event
        protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            string AdditionalRemarks = null;
            decimal? AdditionalCost = null;
            if (e.NextStepIndex == 1)
            {
                if (TpkNameTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, " Tour Package Name Is Required", "Information", DialogTypes.Information);
                    TpkNameTextBox.Focus();
                    e.Cancel= true;
                    return;
                }
                if (TpkDescTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Tour Package Description Is Required", "Information", DialogTypes.Information);
                    TpkDescTextBox.Focus();
                    e.Cancel = true;
                    return;
                }
                if (TpkDestCoveredTextBox.Text.Trim()== "")
                {
                    WebMessenger.Show(this, " Destination Covered List Is Required", "Information", DialogTypes.Information);
                    TpkDestCoveredTextBox.Focus();
                    e.Cancel = true;
                    return;
                }
                if (TpkPickUpTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, " Pick Up Point Is Required", "Information", DialogTypes.Information);
                    TpkPickUpTextBox.Focus();
                    e.Cancel = true;
                    return;
                }
                if (TpkDropTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, " Drop Point Is Required", "Information", DialogTypes.Information);
                    TpkDropTextBox.Focus();
                    e.Cancel = true;
                    return;
                }
                if (TravelSegmentDropDownList.SelectedValue=="0")
                {
                    WebMessenger.Show(this, "Select A Travel Segment", "Information", DialogTypes.Information);
                    TravelSegmentDropDownList.Focus();
                    e.Cancel = true;
                    return;
                }
                if (TpkTypeDropDownList.SelectedValue == "0")
                {
                    WebMessenger.Show(this, "Select A Package Type", "Information", DialogTypes.Information);
                    TpkTypeDropDownList.Focus();
                    e.Cancel = true;
                    return;
                }
                if (TpkDaysTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "No Of Days Required ", "Information", DialogTypes.Information);
                    TpkDaysTextBox.Focus();
                    e.Cancel = true;
                    return;
                }
                if (AdditionalCostCheckBox.Checked == true)
                {
                    if (RemarksTextBox.Text.Trim() == "")
                    {
                        WebMessenger.Show(this, "Remarks For Additional Cost Is Required .", "Information", DialogTypes.Information);
                        RemarksTextBox.Focus();
                        e.Cancel = true;
                        return;
                    }
                    else if (AdditionalCostTextBox.Text.Trim() == "")
                    {
                        WebMessenger.Show(this, "Additional Cost Is Required ", "Information", DialogTypes.Information);
                        AdditionalCostTextBox.Focus();
                        e.Cancel = true;
                        return;
                    }
                    //else if (ValidatorClass.isValidCurrency(AdditionalCostTextBox.Text.Trim()))
                    //{
                    //    WebMessenger.Show(this, "Invalid Character In Additional Cost", "Information", DialogTypes.Information);
                    //    AdditionalCostTextBox.Focus();
                    //    e.Cancel = true;
                    //    return;
                    //}
                    else
                    {
                        AdditionalRemarks = RemarksTextBox.Text.Trim();
                        AdditionalCost = decimal.Parse(AdditionalCostTextBox.Text.Trim());
                    }
                }
                TpkNightsTextBox.Text = (int.Parse(TpkDaysTextBox.Text.Trim()) - 1).ToString();
                objPackage = new TpkPackageMst();
                try
                {
                    if (ViewState["TpkID"] != null)
                    {
                        if (ViewState["TpkID"].ToString() == "1")
                        {
                            string tpkID = null;
                            objPackage.AddTourPackage(Session["BusDtl"].ToString().Split('|')[0].ToString(), TpkNameTextBox.Text.Trim(), TpkDescTextBox.Text.Trim(), TpkDestCoveredTextBox.Text.Trim(), TpkPickUpTextBox.Text.Trim(), TpkDropTextBox.Text.Trim(), Int16.Parse(TpkDaysTextBox.Text.Trim()), Int16.Parse(TpkNightsTextBox.Text.Trim()), Int64.Parse(TpkTypeDropDownList.SelectedValue.ToString()), Int64.Parse(TravelSegmentDropDownList.SelectedValue.ToString()), DateTime.Now, DateTime.Now,AdditionalRemarks,AdditionalCost, false, Page.User.Identity.Name, ref tpkID);
                            TpkItnCodeTextBox.Text = tpkID;
                            ViewState["TpkID"] = tpkID;
                            LoadItnDay();
                            TpkItnNameTextBox.Text = TpkNameTextBox.Text.Trim();
                            WebMessenger.Show(this, "Tour Package Added Successfully", "Package Added", DialogTypes.Success);
                        }
                        else
                        {
                            objPackage.EditTourPackage(TpkNameTextBox.Text.Trim(), TpkDescTextBox.Text.Trim(), TpkDestCoveredTextBox.Text.Trim(), TpkPickUpTextBox.Text.Trim(), TpkDropTextBox.Text.Trim(), Int16.Parse(TpkDaysTextBox.Text.Trim()), Int16.Parse(TpkNightsTextBox.Text.Trim()), Int64.Parse(TpkTypeDropDownList.SelectedValue.ToString()), Int64.Parse(TravelSegmentDropDownList.SelectedValue.ToString()), DateTime.Now, DateTime.Now,AdditionalRemarks,AdditionalCost, false, Page.User.Identity.Name, ViewState["TpkID"].ToString());
                            LoadItnDay();
                            TpkItnNameTextBox.Text = TpkNameTextBox.Text.Trim();
                            TpkItnCodeTextBox.Text = ViewState["TpkID"].ToString();
                        }
                        TpkPackagaeItineraryCustomGridView.BindData();
                    }
                }
                catch (Exception ex)
                {
                    WebMessenger.Show(this, "Unable to Add  Tour Package At this Moment", "Error", DialogTypes.Error);
                    return;
                }
                finally
                {
                    objPackage = null;
                }
                
            }
            else if (e.NextStepIndex == 2)
            {
                loadgrid();
                TpkPhotoCodeTextBox.Text = ViewState["TpkID"].ToString();
                TpkPhotoNameTextBox.Text = TpkNameTextBox.Text.Trim();
                                
            }
            else if (e.NextStepIndex == 3)
            {
                
                
            }
        }


        #endregion
        #region Finish Button Click
        protected void Wizard1_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            TpkWizardPanel.Visible = false;
            SearchPanel.Visible = true;
        }

        #endregion

        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            SearchPanel.Visible = false;
            TpkWizardPanel.Visible = true;
            ClearStepOne();
        }
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            TpkGridView.BindData();
        }
        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (TpkGridView.SelectedDataKey!=null)
            {
                SearchPanel.Visible = false;
                TpkWizardPanel.Visible = true;
                
            }
            else
            {
                WebMessenger.Show(this,"Select A Package First","Select",DialogTypes.Information);
            }
        }
        protected void ItnAddButton_Click(object sender, EventArgs e)
        {
            try
            {
               EnableStepTwoControls();
               ClearStepTwoControls();
                ViewState["Operation"] = "ADD";
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Add Itinerary Details At this Moment", "Itinerary Detail", DialogTypes.Error);
                return;
            }
        }
        protected void ItnEditButton_Click(object sender, EventArgs e)
        {
            try
            {
                EnableStepTwoControls();

                if (TpkItnDayNoDropDownList.SelectedValue == TpkItnDayNoDropDownList.Items.Count.ToString())
                {
                    DestDropDownList.Enabled = false;
                    DestDropDownList.SelectedValue = "0";
                    TpkItnOverNightTextBox.Enabled = false;
                    TpkItnOverNightTextBox.Text = "Not Required";
                    NoAccomadationRequiredRadioButton.Enabled = false;
                    YesAccomadationRequiredRadioButton.Enabled = false;
                    NoAccomadationRequiredRadioButton.Checked = true;
                    YesAccomadationRequiredRadioButton.Checked = false;
                }
                ViewState["Operation"] = "EDIT";
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Add Itinerary Details At this Moment", "Itinerary Detail", DialogTypes.Error);
                return;
            }
        }
        protected void ItnSaveButton_Click(object sender, EventArgs e)
        {
            
            objPackage = new TpkPackageMst();
            Int64? DestinationID=null;
            bool accomadationReq = true;
            try
            {

                if (SelectedItineraryHiddenField.Value == "")
                {
                    WebMessenger.Show(this, "Select A Itineary ", "Information", DialogTypes.Information);
                    return;
                }
                if (DestDropDownList.Enabled == true)
                {
                    if (DestDropDownList.SelectedValue == "0")
                    {
                        WebMessenger.Show(this, "Select A Destination for the Day", "Information", DialogTypes.Information);
                        DestDropDownList.Focus();
                        return;
                    }
                    else
                    {
                        DestinationID = Int64.Parse(DestDropDownList.SelectedValue.ToString());
                    }
                }
              
                else
                {
                    DestinationID = null;
                }
                if (YesAccomadationRequiredRadioButton.Checked == false && NoAccomadationRequiredRadioButton.Checked == false)
                {
                    WebMessenger.Show(this, "Provide Input Whether Accomadation Required Or Not", "Information", DialogTypes.Information);
                    return;
                }
                else
                {

                    if (YesAccomadationRequiredRadioButton.Checked == true)
                    {
                        accomadationReq = true;
                    }
                    else
                    {
                        accomadationReq = false;
                    }
                }
                if (SelectedItineraryHiddenField.Value == "")
                {
                    WebMessenger.Show(this, "Select A Itineray For The Day", "Information", DialogTypes.Information);
                    return;
                }
               

                if (ViewState["Operation"] != null)
                {
                    if (ViewState["Operation"].ToString() == "ADD")
                    {
                        //if (Session["DestinationID"] == "")
                       // {
                        //    WebMessenger.Show(this, "no destination found for the last existing day, please set destination then add new record! ", "Information", DialogTypes.Information);
                        //    DestDropDownList.Focus();
                        //    return;
                       // }
                        objPackage.AddTpkPackagaeItineraryDtl(Session["BusDtl"].ToString().Split('|')[0].ToString(), TpkItnCodeTextBox.Text.ToString(), SelectedItineraryHiddenField.Value, Int16.Parse(TpkItnDayNoDropDownList.SelectedValue), DestinationID, accomadationReq, TpkItnOverNightTextBox.Text.Trim(), Page.User.Identity.Name);
                        TpkPackagaeItineraryCustomGridView.BindData();
                        WebMessenger.Show(this, "Record Added Successfully", "Record Updated", DialogTypes.Success);
                        ViewState["Operation"] = null;
                        DisableStepTwoControls();
                        ClearStepTwoControls();
                    }
                    else if (ViewState["Operation"].ToString() == "EDIT")
                    {
                        objPackage.EditTpkPackagaeItineraryDtl(TpkPackagaeItineraryCustomGridView.SelectedDataKey.Values["TpkItineraryID"].ToString(), SelectedItineraryHiddenField.Value, Int16.Parse(TpkItnDayNoDropDownList.SelectedValue), DestinationID, accomadationReq, TpkItnOverNightTextBox.Text.Trim(), Page.User.Identity.Name);
                        TpkPackagaeItineraryCustomGridView.BindData();
                        WebMessenger.Show(this, "Record Updated Successfully", "Record Updated", DialogTypes.Success);
                        ViewState["Operation"] = null;
                        DisableStepTwoControls();
                        ClearStepTwoControls();
                    }
                    else
                    {
                        WebMessenger.Show(this, "Select A Valid Operation", "Operation", DialogTypes.Information);
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Select A Opeartion First", "Opeartion", DialogTypes.Information);
                }

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Mulitple Itinerary Cannot Be Added For Single Day", "Itinerary Detail", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }
        protected void ItnCancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                DisableStepTwoControls();
                ClearStepTwoControls();
                ViewState["Operation"] = null;

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Add Itinerary Details At this Moment", "Itinerary Detail", DialogTypes.Error);
                return;
            }
        }

        protected void PhotoAddButton_Click(object sender, EventArgs e)
        {
            try
            {
                EnableStepThreeControls();
                ClearStepThreeControls();
                ViewState["PhotoOperation"] = "ADD";
                
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Add Photo At this Moment", "Photo", DialogTypes.Error);
                return;
            }
        }
        protected void PhotoEditButton_Click(object sender, EventArgs e)
        {
            try
            {
                EnableStepThreeControls();
                ViewState["PhotoOperation"] = "EDIT";
                
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Update Photo Details At this Moment", "Photo", DialogTypes.Error);
                return;
            }
        }
        protected void PhotoSaveButton_Click(object sender, EventArgs e)
        {
            objPackage = new TpkPackageMst();
            try
            {
                byte[] Photograph;
                byte[] PhotoThumb;
                Photograph = (byte[])Session["ucPhotoUploaderUploadedImage"];
                Photograph = Sibin.Imaging.TwoD.ImageShaper.ResizeImage(Photograph, 600, 480);
                PhotoThumb = Sibin.Imaging.TwoD.ImageShaper.ResizeImage(Photograph, 100, 80);
                if (ViewState["PhotoOperation"] != null)
                {
                    if (ViewState["PhotoOperation"].ToString() == "ADD")
                    {
                        objPackage.AddTpkPackagePhotoDtl(Session["BusDtl"].ToString().Split('|')[0].ToString(), ViewState["TpkID"].ToString(), TpkPhotoDescTextBox.Text.Trim(), IsDefaultCheckBox.Checked, Photograph, PhotoThumb, Page.User.Identity.Name);
                        ClearStepThreeControls();
                        DisableStepThreeControls();
                        ViewState["PhotoOperation"] = null;
                        WebMessenger.Show(this, "Photo  Added Sucessfully", "Success", DialogTypes.Success);
                    }
                    else if (ViewState["PhotoOperation"].ToString() == "EDIT")
                    {
                        objPackage.EditTpkPackagePhotoDtl(ViewState["TpkPhotoID"].ToString(), TpkPhotoDescTextBox.Text.Trim(), IsDefaultCheckBox.Checked, Photograph, PhotoThumb, Page.User.Identity.Name);
                        
                        ClearStepThreeControls();
                        DisableStepThreeControls();
                        ViewState["PhotoOperation"] = null;
                        WebMessenger.Show(this, "Photo  Updated Sucessfully", "Success", DialogTypes.Success);
                    }
                    else
                    {
                        WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                        ClearStepThreeControls();
                        DisableStepThreeControls();
                        ViewState["PhotoOperation"] = null;
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                    ClearStepThreeControls();
                    DisableStepThreeControls();
                    ViewState["PhotoOperation"] = null;
                }
                loadgrid();
                CheckImage();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Add Photo Details At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        
        }
        protected void PhotoCancelButton_Click(object sender, EventArgs e)
        {
                DisableStepThreeControls();
                ClearStepThreeControls();
                ViewState["PhotoOperation"] = "";
                CheckImage();
        }
        #endregion

        private void LoadAllPackageType()
        {
            objPtlAdmTourPackageMstBLL = new TourPackageTypeMstBLL();
            try
            {
                DataTable dt = objPtlAdmTourPackageMstBLL.GetPackageType();
                TpkTypeSearchDropDownList.Items.Clear();
                TpkTypeSearchDropDownList.Items.Add(new ListItem("--- Select ---", "0"));
                TpkTypeSearchDropDownList.DataTextField = "PackageTypeName";
                TpkTypeSearchDropDownList.DataValueField = "PackageTypeID";
                TpkTypeSearchDropDownList.DataSource = dt;
                TpkTypeSearchDropDownList.DataBind();
                TpkTypeSearchDropDownList.SelectedIndex = 0;


                TpkTypeDropDownList.Items.Clear();
                TpkTypeDropDownList.Items.Add(new ListItem("--- Select ---", "0"));
                TpkTypeDropDownList.DataTextField = "PackageTypeName";
                TpkTypeDropDownList.DataValueField = "PackageTypeID";
                TpkTypeDropDownList.DataSource = dt;
                TpkTypeDropDownList.DataBind();
                TpkTypeDropDownList.SelectedIndex = 0;
               
                
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Package Type At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmTourPackageMstBLL = null;
            }

        }
        private void LoadTravelSegment()
        {

            objPtlAdmTravelSegmentMstBLL = new TravelSegmentMstBLL();
            try
            {
                TravelSegmentDropDownList.Items.Clear();
                TravelSegmentDropDownList.Items.Add(new ListItem("--- Select ---", "0"));
                TravelSegmentDropDownList.DataTextField = "TravelSegmentName";
                TravelSegmentDropDownList.DataValueField = "TravelSegmentID";
                TravelSegmentDropDownList.DataSource = objPtlAdmTravelSegmentMstBLL.GetTravelSegment();
                TravelSegmentDropDownList.DataBind();
                TravelSegmentDropDownList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                 WebMessenger.Show(this, "Unable To Load All Travel Segment At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmTravelSegmentMstBLL = null;
            }

        }
        private void LoadDestination()
        {
            objPtlAdmDestinationMasterBLL = new DestinationMasterBLL();
            try
            {
                DestDropDownList.Items.Clear();
                DestDropDownList.Items.Add(new ListItem("--- Select ---", "0"));
                DestDropDownList.DataTextField = "DestinationName";
                DestDropDownList.DataValueField = "DestinationID";
                DestDropDownList.DataSource = objPtlAdmDestinationMasterBLL.GetDestination(null);
                DestDropDownList.DataBind();
                DestDropDownList.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load All Destination At This Moment", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmDestinationMasterBLL = null;
            }

        }

        private void LoadItnDay()
        {
            try
            {
                TpkItnDayNoDropDownList.Items.Clear();

                if (TpkDaysTextBox.Text.Trim() != "")
                {
                    int noOfDays = int.Parse(TpkDaysTextBox.Text.Trim());
                    for (int i = 1; i <= noOfDays; i++)
                    {
                        TpkItnDayNoDropDownList.Items.Add(new ListItem("Day -" + i.ToString(), i.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Itinerary Days At this Moment", "Error", DialogTypes.Error);
                return;
            }
        }
        public void AutosuggestItinerary()
        {
            objBusAdmItineraryMstBLL = new ItineraryMstBLL();
            try
            {
                DataTable dt = objBusAdmItineraryMstBLL.GetItinerary(Session["BusDtl"].ToString().Split('|')[0].ToString());
                string json = Server.HtmlDecode(JsonConvert.SerializeObject(dt));
                ItineraryListHiddenField.Value = json;
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Itineary List at this Moment", "Error", DialogTypes.Error);
                return;
            }
           
        }
        
        public void EnableStepTwoControls()
        {
            TpkItnDayNoDropDownList.Enabled = true;
            TpkActivityForDayTextBox.Enabled = true;
            DestDropDownList.Enabled = true;
            TpkItnOverNightTextBox.Enabled = true;
            YesAccomadationRequiredRadioButton.Enabled = true;
            NoAccomadationRequiredRadioButton.Enabled = true;
        }
        public void DisableStepTwoControls()
        {
            TpkItnCodeTextBox.Enabled = false;
            TpkItnNameTextBox.Enabled = false;
            TpkItnDayNoDropDownList.Enabled = false;
            TpkActivityForDayTextBox.Enabled = false;
            DestDropDownList.Enabled = false;
            TpkItnOverNightTextBox.Enabled = false;
            YesAccomadationRequiredRadioButton.Enabled = false;
            NoAccomadationRequiredRadioButton.Enabled = false;
        }
        public void ClearStepTwoControls()
        {
            ItineraryDetailTextBox.Text = "";
            TpkItnDayNoDropDownList.SelectedValue = "1";
            TpkActivityForDayTextBox.Text = "";
            DestDropDownList.SelectedValue = "0";
            TpkItnOverNightTextBox.Text = "";
            YesAccomadationRequiredRadioButton.Checked = false;
            NoAccomadationRequiredRadioButton.Checked = false;
        }
        public void DisableStepThreeControls()
        {
            TpkPhotoCodeTextBox.Enabled = false;
            TpkPhotoNameTextBox.Enabled = false;
            TpkPhotoDescTextBox.Enabled = false;
            IsDefaultCheckBox.Enabled = false;
            ClearImageButton.Enabled = false;
            this.RegisterJS("$('#fupActivityImage').css('pointer-events','none');");
           
        }
        public void EnableStepThreeControls()
        {
            IsDefaultCheckBox.Enabled = true;
            TpkPhotoDescTextBox.Enabled = true;
            ClearImageButton.Enabled = true;
            this.RegisterJS("$('#fupActivityImage').css('pointer-events','');");
        }
        public void ClearStepThreeControls()
        {
            Session["ucPhotoUploaderUploadedImage"] = null;
            TpkPhotoDescTextBox.Text = "";
            IsDefaultCheckBox.Checked = false;
            CheckImage();
        }

        public void loadgrid()
        {
            objPackage = new TpkPackageMst();
            try
            {
                DataTable dt = new DataTable();
                dt = objPackage.GetAllTpkPackagePhotoDtlPaged(1, 100, Session["BusDtl"].ToString().Split('|')[0].ToString(), ViewState["TpkID"].ToString());
                PhotoDtlsGridView.DataSource = dt;
                PhotoDtlsGridView.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Load Photo Details At this Moment .", "Photo Details", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }
        protected void PhotoDtlsGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            TpkPhotoDescTextBox.Text = PhotoDtlsGridView.Rows[e.NewSelectedIndex].Cells[1].Text;
            if (PhotoDtlsGridView.Rows[e.NewSelectedIndex].Cells[1].Text == "True")
            {
                IsDefaultCheckBox.Checked = true;
            }
            else
            {
                IsDefaultCheckBox.Checked = false;
            }
            Session["ucPhotoUploaderUploadedImage"] = (byte[])PhotoDtlsGridView.DataKeys[e.NewSelectedIndex].Values["PhotoNormal"];
            ViewState["TpkPhotoID"] = PhotoDtlsGridView.DataKeys[e.NewSelectedIndex].Values["TpkPhotoID"].ToString();
            CheckImage();
        }
        protected void PhotoDtlsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objPackage = new TpkPackageMst();
            try
            {
                objPackage.DeleteTpkPackagePhotoDtl(PhotoDtlsGridView.DataKeys[e.RowIndex].Values["TpkPhotoID"].ToString(), Page.User.Identity.Name);
                loadgrid();
                WebMessenger.Show(this, "Photo Details Deleted Successfully", "Deleted", DialogTypes.Success);
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Unable To Delete Photo Details At this Moment", "Deleted", DialogTypes.Error);
                return;
            }
            finally
            {
                objPackage = null;
            }
        }

        protected void CompletePackageLinkButton_Click(object sender, EventArgs e)
        {
            ViewState["InComplete"] = 100;
            TpkGridView.SetPageIndex = 0;
            TpkIDSearchTextBox.Text = "";
            TpkSearchNameTextBox.Text = "";
            TpkTypeSearchDropDownList.SelectedValue = "0";
            TpkGridView.BindData();
        }

        protected void InCompletePackageLinkButton_Click(object sender, EventArgs e)
        {
            TpkIDSearchTextBox.Text = "";
            TpkSearchNameTextBox.Text = "";
            TpkTypeSearchDropDownList.SelectedValue = "0";
            ViewState["InComplete"] = 33;
            TpkGridView.SetPageIndex = 0;
            TpkGridView.BindData();
        }

        protected void ViewAllLinkButton_Click(object sender, EventArgs e)
        {
            ViewState["InComplete"] = null;
            TpkGridView.BindData();
        }
        protected void BackLinkButton_Click(object sender, EventArgs e)
        {
            SearchPanel.Visible = true;
            TpkWizardPanel.Visible = false;
            TpkGridView.BindData();
        }
        protected void ClearStepOne()
        {
            ViewState["TpkID"] = "1";
            TpkNameTextBox.Text = "";
            TpkTypeDropDownList.SelectedValue = "0";
            TravelSegmentDropDownList.SelectedValue = "0";
            TpkDescTextBox.Text = "";
            TpkDestCoveredTextBox.Text = "";
            TpkPickUpTextBox.Text = "";
            TpkDropTextBox.Text = "";
            TpkDaysTextBox.Text = "";
            TpkNightsTextBox.Text = "";
            AdditionalCostPanel.Visible = false;
            AdditionalCostCheckBox.Checked = false;
        }
       
        protected void CheckImage()
        {
            if (Session["ucPhotoUploaderUploadedImage"] == null)
            {
                this.RegisterJS("HideThumbImage();");
            }
            else
            {
                this.RegisterJS("ShowThumbImage();");
            }
        }
        protected void ClearImageButton_Click(object sender, EventArgs e)
        {
            Session["ucPhotoUploaderUploadedImage"] = null;
            CheckImage();
        }

        protected void AdditionalCostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AdditionalCostCheckBox.Checked == true)
            {
                AdditionalCostPanel.Visible = true;
            }
            else
            {
                AdditionalCostPanel.Visible = false;
            }
        }
    }
}

