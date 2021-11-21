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
using Sibin.FrameworkExtensions.DotNet.Web;
using System.Data;
using Sibin.Utilities.Imaging.TwoD;

namespace e_Travel.BusinessAdmin
{
    public partial class AccommodationMaster : AdminBasePage
    {
        #region Private Variable
        AccommodationMstBLL objBusAdmAccommodationMstBLL;
        CountryMasterBLL objPtlAdmCountryMasterBLL;
        DestinationMasterBLL objPtlAdmDestinationMasterBLL;

        byte[] AccImage;


        #endregion

        #region Page Load
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
                AccommodationMasterCustomGridView.AddGridViewColumns("Accomodation Name|Description|City", "AccName|AccDesc|AccCity", "200|300|100", "AccID|AccTypeID|CountryID|DestinationID|AccAddress|ZipCode|PhoneNo|MobileNo|EmailID|FaxNo|Website|Latitude|Longitude|AccImgThumb|AccImgNormal", true, true);
                    AccommodationMasterCustomGridView.BindData();
                    LoadAccomodationType();
                    LoadCountry();
                    LoadDestination(null);
                    InputEnableDisable(false);
                    Clearall();
                    CheckImage();
            }
        }
        #endregion

        #region Load Accomodation Type in Dropdown

        private void LoadAccomodationType()
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                AccTypeDropDown.Items.Clear();
                AccTypeDropDown.Items.Add(new ListItem("--------------------Select Accomodation--------------------", "0"));
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

        #region Load All Country in Dropdown
        private void LoadCountry()
        {
            objPtlAdmCountryMasterBLL = new CountryMasterBLL();
            try
            {
                AccCountryDropDown.Items.Clear();
                AccCountryDropDown.Items.Add(new ListItem("-------------------------Select Country-------------------------", "0"));
                AccCountryDropDown.DataSource = objPtlAdmCountryMasterBLL.GetCountry();
                AccCountryDropDown.DataTextField = "CountryName";
                AccCountryDropDown.DataValueField = "CountryID";
                AccCountryDropDown.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error While Loading Country", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objPtlAdmCountryMasterBLL = null;
            }
        }
        #endregion

        #region Load Destination
        private void LoadDestination(Int64? countryID)
        {
            objPtlAdmDestinationMasterBLL = new DestinationMasterBLL();
            try
            {
                AccDestinationDropDown.Items.Clear();
                AccDestinationDropDown.Items.Add(new ListItem("-----------------------Select Destination-----------------------", "0"));
                AccDestinationDropDown.DataSource = objPtlAdmDestinationMasterBLL.GetDestination(countryID);
                AccDestinationDropDown.DataTextField = "DestinationName";
                AccDestinationDropDown.DataValueField = "DestinationID";
                AccDestinationDropDown.DataBind();
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

        #region Load Destination By Country ID
        protected void AccCountryDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            objPtlAdmDestinationMasterBLL = new DestinationMasterBLL();
            if (AccCountryDropDown.SelectedIndex > 0)
            {
                try
                {
                    AccDestinationDropDown.Items.Clear();
                    AccDestinationDropDown.Items.Add(new ListItem("-----------------------Select Destination-----------------------", "0"));
                    AccDestinationDropDown.DataSource = objPtlAdmDestinationMasterBLL.GetDestination(long.Parse(AccCountryDropDown.SelectedValue.ToString()));
                    AccDestinationDropDown.DataTextField = "DestinationName";
                    AccDestinationDropDown.DataValueField = "DestinationID";
                    AccDestinationDropDown.DataBind();
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
        }
        #endregion

        #region Page Function

        private void Clearall()
        {
            AccTypeDropDown.SelectedValue = "0";
            AccCountryDropDown.SelectedValue = "0";
            AccDestinationDropDown.SelectedValue = "0";
            AccCityTextBox.Text = "";
            AccNameTextBox.Text = "";
            AccDescTextBox.Text = "";
            AccAddressTextBox.Text = "";
            AccZipCodeTextBox.Text = "";
            AccPhoneNumberTextBox.Text = "";
            AccMobileNumberTextBox.Text = "";
            AccEmailTextBox.Text = "";
            AccFaxNumberTextBox.Text = "";
            AccWebsiteTextBox.Text = "";
            AccLatitudeTextBox.Text = "";
            AccLongitudeTextBox.Text = "";
            Session["ucPhotoUploaderUploadedImage"] = null;
            CheckImage();
        }

        private void InputEnableDisable(bool isEnabled)
        {
            AccTypeDropDown.Enabled = isEnabled;
            AccCountryDropDown.Enabled = isEnabled;
            AccDestinationDropDown.Enabled = isEnabled;
            AccCityTextBox.Enabled = isEnabled;
            AccNameTextBox.Enabled = isEnabled;
            AccDescTextBox.Enabled = isEnabled;
            AccAddressTextBox.Enabled = isEnabled;
            AccZipCodeTextBox.Enabled = isEnabled;
            AccPhoneNumberTextBox.Enabled = isEnabled;
            AccMobileNumberTextBox.Enabled = isEnabled;
            AccEmailTextBox.Enabled = isEnabled;
            AccFaxNumberTextBox.Enabled = isEnabled;
            AccWebsiteTextBox.Enabled = isEnabled;
            AccLatitudeTextBox.Enabled = isEnabled;
            AccLongitudeTextBox.Enabled = isEnabled;
            ClearImageButton.Enabled = isEnabled;
            if (isEnabled == true)
            {
                this.RegisterJS("$('#fupActivityImage').css('pointer-events','');");
            }
            else if (isEnabled == false)
            {
                this.RegisterJS("$('#fupActivityImage').css('pointer-events','none');");
            }
        }
        #endregion

        #region Button Click Events

        protected void AddButton_Click(object sender, EventArgs e)
        {
            Session["OperationType"] = "ADD";
            Clearall();
            InputEnableDisable(true);

            AccNameTextBox.Focus();
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (AccommodationMasterCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                
                Session["OperationType"] = "UPDATE";
                InputEnableDisable(true);
                AccNameTextBox.Focus();
                CheckImage();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                #region Validation
                decimal? latitude = null, longitude = null;
                string phoneNumber = null, faxNo = null, website = null;
                if (AccNameTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Please Provide Accomodation Name", "Required", DialogTypes.Information);
                    return;
                }
                if (AccNameTextBox.Text.Trim() != "")
                {
                    if (ValidatorClass.ValidateText(AccNameTextBox.Text.Trim()) == false)
                    {
                        WebMessenger.Show(this, "Please Type Valid Accomodation Name", "Invalid", DialogTypes.Information);
                        return;
                    }
                }
                if (AccTypeDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Accomodation Type", "Required", DialogTypes.Information);
                    return;
                }
                if (AccWebsiteTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "You Cannot Left Website Field Blank", "Required", DialogTypes.Information);
                    return;
                }
                else if (AccWebsiteTextBox.Text.Trim() != "")
                {
                    if (ValidatorClass.isValidURL(AccWebsiteTextBox.Text.Trim()) == false)
                    {
                        WebMessenger.Show(this, "Provide A Valid Url", "Invalid Url", DialogTypes.Information);
                        return;
                    }

                }
                if (AccCountryDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Country", "Required", DialogTypes.Information);
                    return;
                }
                if (AccDestinationDropDown.SelectedIndex <= 0)
                {
                    WebMessenger.Show(this, "Please Select Destination", "Required", DialogTypes.Information);
                    return;
                }
                if (AccCityTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Please Provide City Name", "Required", DialogTypes.Information);
                    return;
                }
                else if (ValidatorClass.ValidateText(AccCityTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Provide A Valid City Name", "Invalid", DialogTypes.Information);
                    return;
                }
                if (AccZipCodeTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Please Provide ZipCode", "Required", DialogTypes.Information);
                    return;
                }
                if (AccAddressTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "You Cannot Left Address Field Blank", "Required", DialogTypes.Information);
                    return;
                }
                else if (AccAddressTextBox.Text.Trim() != "")
                {
                    if (ValidatorClass.ValidateText(AccAddressTextBox.Text.Trim()) == false)
                    {
                        WebMessenger.Show(this, "Invalid Address", "Invalid", DialogTypes.Information);
                        return;
                    }
                }

                if (AccDescTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Please Type Accomodation Description", "Required", DialogTypes.Information);
                    return;
                }
                else if (ValidatorClass.ValidateText(AccDescTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Provide A Valid Description ", "Invalid", DialogTypes.Information);
                    return;
                }
                if (AccPhoneNumberTextBox.Text.Trim() == "")
                {
                    phoneNumber = null;
                }
                else
                {
                    phoneNumber = AccPhoneNumberTextBox.Text.Trim();
                }
                if (AccMobileNumberTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Mobile Number Required", "Required", DialogTypes.Information);
                    return;
                }
                if (AccFaxNumberTextBox.Text.Trim() == "")
                {
                    faxNo = null;
                }
                else
                {
                    faxNo = AccFaxNumberTextBox.Text.Trim();
                }
                if (AccEmailTextBox.Text.Trim() == "")
                {
                    WebMessenger.Show(this, "Email ID Required", "Required", DialogTypes.Information);
                    return;
                }
                else if (AccEmailTextBox.Text.Trim() != "")
                {
                    if (ValidatorClass.IsValidEmail(AccEmailTextBox.Text.Trim()) == false)
                    {
                        WebMessenger.Show(this, "Invalid Email ID", "Invalid", DialogTypes.Information);
                        return;
                    }
                }


                if (AccLatitudeTextBox.Text.Trim() == "")
                {
                    latitude = null;
                }
                else if (AccLatitudeTextBox.Text.Trim() != "")
                {
                    if (ValidatorClass.IsNumeric(AccLatitudeTextBox.Text.Trim()))
                    {
                        latitude = Decimal.Parse(AccLatitudeTextBox.Text.Trim().ToString());
                    }
                    else
                    {
                        WebMessenger.Show(this, "Invalid Latitude", "Invalid", DialogTypes.Information);
                        return;
                    }
                }
                if (AccLongitudeTextBox.Text.Trim() == "")
                {
                    longitude = null;
                }
                else if (AccLongitudeTextBox.Text.Trim() != "")
                {
                    if (ValidatorClass.IsNumeric(AccLongitudeTextBox.Text.Trim()))
                    {
                        longitude = Decimal.Parse(AccLongitudeTextBox.Text.Trim().ToString());
                    }
                    else
                    {
                        WebMessenger.Show(this, "Invalid Longitude", "Invalid", DialogTypes.Information);
                        return;
                    }
                }

                if (Session["ucPhotoUploaderUploadedImage"] == null)
                {
                    WebMessenger.Show(this, "You Have Not Upload Image, Please Upload Image!", "Required", DialogTypes.Information);
                    return;
                }
                if (Session["ucPhotoUploaderUploadedImage"] != null)
                {
                    AccImage = ((byte[])Session["ucPhotoUploaderUploadedImage"]);
                }
                else
                {
                    AccImage = null;
                }
                // if (AccImage == null)
                //{
                //  WebMessenger.Show(this, "You Have Not Upload Image, Please Upload Image!", "Required", DialogTypes.Information);
                // return;
                //}

                #endregion

                byte[] PhotoThumb = null;
                if (AccImage != null)
                {
                    AccImage = ImageShaper.ResizeImage(AccImage, 600, 480);
                    PhotoThumb = ImageShaper.ResizeImage(AccImage, 100, 80);
                }
                if (Session["OperationType"] != null)
                {
                    if (Session["OperationType"].ToString() == "ADD")
                    {
                        objBusAdmAccommodationMstBLL.AddAccommodation(Session["BusDtl"].ToString().Split('|')[0].ToString(), AccNameTextBox.Text.Trim(), AccDescTextBox.Text.Trim(), Int64.Parse(AccTypeDropDown.SelectedValue.ToString()), Int64.Parse(AccDestinationDropDown.SelectedValue.ToString()), AccAddressTextBox.Text.Trim(), AccCityTextBox.Text.Trim(), long.Parse(AccCountryDropDown.SelectedValue.ToString()),
                            AccZipCodeTextBox.Text.Trim(), AccPhoneNumberTextBox.Text.Trim(), AccMobileNumberTextBox.Text.Trim(), AccEmailTextBox.Text.Trim(), AccFaxNumberTextBox.Text.Trim(), AccWebsiteTextBox.Text.Trim(), latitude, longitude, AccImage, PhotoThumb, false, Page.User.Identity.Name);
                        AccommodationMasterCustomGridView.BindData();
                        WebMessenger.Show(this, "Record Saved Successfully", "Success", DialogTypes.Success);

                    }
                    else if (Session["OperationType"].ToString() == "UPDATE")
                    {
                        objBusAdmAccommodationMstBLL.EditAccommodation(ViewState["AccID"].ToString(), Session["BusDtl"].ToString().Split('|')[0].ToString(), AccNameTextBox.Text.Trim(), AccDescTextBox.Text.Trim(), Int64.Parse(AccTypeDropDown.SelectedValue.ToString()), Int64.Parse(AccDestinationDropDown.SelectedValue.ToString()), AccAddressTextBox.Text.Trim(), AccCityTextBox.Text.Trim(), long.Parse(AccCountryDropDown.SelectedValue.ToString()),
                            AccZipCodeTextBox.Text.Trim(), AccPhoneNumberTextBox.Text.Trim(), AccMobileNumberTextBox.Text.Trim(), AccEmailTextBox.Text.Trim(), AccFaxNumberTextBox.Text.Trim(), AccWebsiteTextBox.Text.Trim(), latitude, longitude, AccImage, PhotoThumb, Page.User.Identity.Name);

                        AccommodationMasterCustomGridView.BindData();
                        WebMessenger.Show(this, "Record Update Successfully", "Success", DialogTypes.Success);
                    }

                    InputEnableDisable(false);

                    Clearall();
                }
                else
                {
                    WebMessenger.Show(this, "Select A Operation First ", "Information", DialogTypes.Information);
                    return;
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Record Could Not Save", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusAdmAccommodationMstBLL = null;
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            InputEnableDisable(false);
            Clearall();
        }

        #endregion

        #region Gridview Events, BindEvent and Load Gridview
        private DataTable LoadGridView()
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            DataTable dt = new DataTable();
            try
            {
                int startRowIndex = (AccommodationMasterCustomGridView.PageIndex * AccommodationMasterCustomGridView.PageSize) + 1;
                dt = objBusAdmAccommodationMstBLL.GetAccommodationPaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, AccommodationMasterCustomGridView.PageSize);
                if (dt.Rows.Count > 0)
                {
                    AccommodationMasterCustomGridView.TotalRecordCount = int.Parse(dt.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error Occured While Loading Accomodation", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmAccommodationMstBLL = null;
            }
            return dt;
        }

        private void BindEvents()
        {
            AccommodationMasterCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(AccommodationMasterCustomGridView_SelectedIndexChanging);
            AccommodationMasterCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(AccommodationMasterCustomGridView_RowDeleting);
            AccommodationMasterCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void AccommodationMasterCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                Session["ucPhotoUploaderUploadedImage"] = null;
                Session["OperationType"] = null;
                DataTable dt = new DataTable();
                int startRowIndex = (AccommodationMasterCustomGridView.PageIndex * AccommodationMasterCustomGridView.PageSize) + 1;
                InputEnableDisable(false);
                ViewState["AccID"] = AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccID"].ToString();
                AccTypeDropDown.SelectedValue = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccTypeID"].ToString());
                AccCountryDropDown.SelectedValue = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["CountryID"].ToString());
                AccDestinationDropDown.SelectedValue = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["DestinationID"].ToString());
                AccNameTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
                AccDescTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
                AccCityTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text);
                AccAddressTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccAddress"].ToString());
                AccZipCodeTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["ZipCode"].ToString());
                AccPhoneNumberTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["PhoneNo"].ToString());
                AccMobileNumberTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["MobileNo"].ToString());
                AccEmailTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["EmailID"].ToString());
                AccFaxNumberTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["FaxNo"].ToString());
                AccWebsiteTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["Website"].ToString());
                AccLatitudeTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["Latitude"].ToString());
                AccLongitudeTextBox.Text = HttpUtility.HtmlDecode(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["Longitude"].ToString());
                if (AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccImgThumb"].ToString() != "")
                {
                    Session["ucPhotoUploaderUploadedImage"] = (byte[])(AccommodationMasterCustomGridView.DataKeys[e.NewSelectedIndex].Values["AccImgNormal"]);
                    
                }
                CheckImage();
                InputEnableDisable(false);

            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                WebMessenger.Show(this, "Error in Record Navigating", "Error", DialogTypes.Error);
            }
            finally
            {
                objBusAdmAccommodationMstBLL = null;
            }
        }

        protected void AccommodationMasterCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
            try
            {
                objBusAdmAccommodationMstBLL = new AccommodationMstBLL();
                int status = 0;
                objBusAdmAccommodationMstBLL.DeleteAccommodation(AccommodationMasterCustomGridView.DataKeys[e.RowIndex].Values["AccID"].ToString(), Page.User.Identity.Name, ref status);
                if (status == 0)
                {
                    WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
                    AccommodationMasterCustomGridView.BindData();
                }
                else
                {
                    WebMessenger.Show(this.Page, "Record Cannot Be Deleted As It Is Refered By Other Record", "Relation Exist", DialogTypes.Information);
                    AccommodationMasterCustomGridView.BindData();
                }

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
    }
}