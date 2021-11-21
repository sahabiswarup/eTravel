using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using e_Travel.Class;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using Sibin.Utilities.Imaging.TwoD;
using Sibin.Utilities.Web.Messengers;
using SIBINUtility.ValidatorClass;
using e_TravelBLL.TourPackage;
using System.IO;
using Sibin.Utilities.Web.ExceptionHandling;
using Sibin.FrameworkExtensions.DotNet.Web;
using System.Collections.ObjectModel;

namespace e_Travel.BusinessAdmin
{
    public partial class BusinessDetails : AdminBasePage
    {
        #region Private Variable        
       
        BusinessMstBLL objBusinessMstBLL;
        DestinationMasterBLL objDestinationMasterBLL;
        Decimal? latitude;
        Decimal? longitude;
        byte[] businessLogo;
        
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
                LoadCountry();
                LoadDestination(null);
                LoadTimeZone();
                LoadBusinessType();
                LoadBusinessDetails(Session["BusDtl"].ToString().Split('|')[0].ToString());  
            }
        }
        #endregion

        #region Load Country, TimeZone,Business Details and Destination
        private void LoadCountry()
        {
            objBusinessMstBLL = new BusinessMstBLL();
            try
            {
                CountryDropDownList.Items.Clear();
                CountryDropDownList.Items.Add(new ListItem("-------------------------Select Country-------------------------", "0"));
                CountryDropDownList.DataSource = objBusinessMstBLL.GetCountry();
                CountryDropDownList.DataTextField = "CountryName";
                CountryDropDownList.DataValueField = "CountryID";
                CountryDropDownList.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Error While Loading Country", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusinessMstBLL = null;
            }
        }        

        private void LoadDestination(Int64? countryID)
        {
            objDestinationMasterBLL = new DestinationMasterBLL();
            try
            {
                DestinationDropDownList.Items.Clear();
                DestinationDropDownList.Items.Add(new ListItem("--- Select Destination ---", "0"));
                DestinationDropDownList.DataSource = objDestinationMasterBLL.GetDestination(countryID);
                DestinationDropDownList.DataTextField = "DestinationName";
                DestinationDropDownList.DataValueField = "DestinationID";
                DestinationDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error:" + ex, "Error Occured", DialogTypes.Error);
                return;
            }
            finally
            {
                objDestinationMasterBLL = null;
            }
        }

        private void LoadTimeZone()
        {
            ReadOnlyCollection<TimeZoneInfo> tzCollection;
            tzCollection = TimeZoneInfo.GetSystemTimeZones();
            TimeZoneDropDownList.Items.Clear();
            TimeZoneDropDownList.Items.Add(new ListItem("----------Select Time Zone----------", "0"));
            foreach (TimeZoneInfo timeZone in tzCollection)
            {
                TimeZoneDropDownList.Items.Add(new ListItem(timeZone.DisplayName, timeZone.Id));
            }
        }

        private void LoadBusinessDetails(string businessID)
        {
            objBusinessMstBLL = new BusinessMstBLL();
            try
            {
                DataTable dt = new DataTable();
                dt = objBusinessMstBLL.GetBusinessListDetails(1, 10, businessID);
                if (dt.Rows.Count > 0)
                {
                    BusinessNameLabel.Text = "Welcome " + dt.Rows[0]["BusName"].ToString();
                    BusNameTextBox.Text = dt.Rows[0]["BusName"].ToString();
                    BusAddTextBox.Text = dt.Rows[0]["BusAddress"].ToString();
                    CountryDropDownList.SelectedValue = dt.Rows[0]["CountryID"].ToString();
                    DestinationDropDownList.SelectedValue = dt.Rows[0]["DestinationID"].ToString();
                    BusCityTextBox.Text = dt.Rows[0]["BusCity"].ToString();
                    ZipCodeTextBox.Text = dt.Rows[0]["BusZipCode"].ToString();
                    WebsiteTextBox.Text = dt.Rows[0]["BusWebsite"].ToString();
                    BusTypeDropDownList.SelectedValue = dt.Rows[0]["BusType"].ToString();
                    TimeZoneDropDownList.SelectedValue = dt.Rows[0]["BusTimeZone"].ToString();
                     LatitudeTextBox.Text = dt.Rows[0]["Latitude"].ToString(); 
                    LongitudeTextBox.Text =dt.Rows[0]["Longitude"].ToString(); 
                    ContactNameTextBox.Text = dt.Rows[0]["ContactPersonName"].ToString();
                    PhoneNoTextBox.Text = dt.Rows[0]["ContactPhoneNo"].ToString();
                    MobileNoTextBox.Text = dt.Rows[0]["ContactMobileNo"].ToString();
                    AltMobileNoTextBox.Text = dt.Rows[0]["ContactAlternateMobileNo"].ToString();
                    EmailIDTextBox.Text = dt.Rows[0]["ContactEmailID"].ToString();
                    AltEmailIDTextBox.Text = dt.Rows[0]["ContactAlternateEmailID"].ToString();
                    FaxNoTextBox.Text = dt.Rows[0]["ContactFaxNo"].ToString();
                    if (dt.Rows[0]["BusLogo"].ToString() != "")
                    {
                        Session["ucPhotoUploaderUploadedImage"] = (byte[])(dt.Rows[0]["BusLogo"]);
                        this.RegisterJS("loadImage();");
                    }
                }
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                BRMessengers.BRInformation(this, ex.Message);
            }
            finally
            {
                objBusinessMstBLL = null;
            }
        }
        #endregion

        #region Load Business Type in Dropdown
        private void LoadBusinessType()
        {
            objBusinessMstBLL = new BusinessMstBLL();
            try
            {

                BusTypeDropDownList.Items.Clear();
                BusTypeDropDownList.Items.Add(new ListItem("--- Select Business Type ---", "0"));
                BusTypeDropDownList.DataSource = objBusinessMstBLL.GetBusinessType();
                BusTypeDropDownList.DataTextField = "BusinessTypeName";
                BusTypeDropDownList.DataValueField = "BusinessTypeID";
                BusTypeDropDownList.DataBind();
            }
            catch (Exception)
            {
                WebMessenger.Show(this.Page, "Error Occured While Loading Business Type", "Error", DialogTypes.Error);
                return;
            }
            finally
            {
                objBusinessMstBLL = null;
            }
        }

        #endregion

        #region Load Destination By Country ID

        protected void CountryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CountryDropDownList.SelectedIndex > 0)
            {
                LoadDestination(Int64.Parse(CountryDropDownList.SelectedValue.ToString()));
            }
        }
        #endregion

        #region Button Click Events    

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            objBusinessMstBLL = new BusinessMstBLL();

            #region Validation
            if (BusNameTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.ValidateText(BusNameTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Please Type Valid Business Name", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            else if (BusNameTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Please Type Business Name", "Required", DialogTypes.Information);
                return;
            }
            if (BusAddTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.ValidateText(BusAddTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Please Type Valid Business Address", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            else if (BusAddTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Please Type Business Address", "Required", DialogTypes.Information);
                return;
            }
            if (CountryDropDownList.SelectedIndex <= 0)
            {
                WebMessenger.Show(this, "Please Select Business Country", "Required", DialogTypes.Information);
                return;
            }
            if (DestinationDropDownList.SelectedIndex <= 0)
            {
                WebMessenger.Show(this, "Please Select Business Destination", "Required", DialogTypes.Information);
                return;
            }
            if (BusCityTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.ValidateText(BusCityTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Please Type Valid City Name", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            else if (BusCityTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Please Type Business City Name", "Required", DialogTypes.Information);
                return;
            }
            if (ZipCodeTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Please Type Business Zip Code", "Required", DialogTypes.Information);
                return;
            }
            if (BusTypeDropDownList.SelectedIndex <= 0)
            {
                WebMessenger.Show(this, "Please Select Business Type", "Required", DialogTypes.Information);
                return;
            }
            if (TimeZoneDropDownList.SelectedIndex <= 0)
            {
                WebMessenger.Show(this, "Please Select Time Zone", "Required", DialogTypes.Information);
                return;
            }
            if (LatitudeTextBox.Text.Trim() == "")
            {
                latitude = null;
            }
            else if (LatitudeTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.IsNumeric(LatitudeTextBox.Text.Trim()))
                {
                    latitude = Decimal.Parse(LatitudeTextBox.Text.Trim().ToString());
                }
                else
                {
                    WebMessenger.Show(this, "Invalid Latitude", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            if (LongitudeTextBox.Text.Trim() == "")
            {
                longitude = null;
            }
            else if (LongitudeTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.IsNumeric(LongitudeTextBox.Text.Trim()))
                {
                    longitude = Decimal.Parse(LongitudeTextBox.Text.Trim().ToString());
                }
                else
                {
                    WebMessenger.Show(this, "Invalid Longitude", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            if (Session["ucPhotoUploaderUploadedImage"] == null)
            {
                businessLogo = null;
            }
            else
            {
                businessLogo = ((byte[])Session["ucPhotoUploaderUploadedImage"]);
            }
            if (EmailIDTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.IsValidEmail(EmailIDTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Your Email ID is invalid", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            if (AltEmailIDTextBox.Text.Trim() != "")
            {
                if (ValidatorClass.IsValidEmail(AltEmailIDTextBox.Text.Trim()) == false)
                {
                    WebMessenger.Show(this, "Your Alternate Email ID is invalid", "Invalid", DialogTypes.Information);
                    return;
                }
            }
            #endregion
            if (Page.IsValid)
            {
                try
                {
                    objBusinessMstBLL.UpdateBusiness(BusNameTextBox.Text.Trim(), BusAddTextBox.Text.Trim(), long.Parse(CountryDropDownList.SelectedValue.ToString()), long.Parse(DestinationDropDownList.SelectedValue.ToString()),
                                BusCityTextBox.Text.Trim(), ZipCodeTextBox.Text.Trim(), BusTypeDropDownList.SelectedValue, TimeZoneDropDownList.SelectedValue, WebsiteTextBox.Text.Trim() == string.Empty ? null : WebsiteTextBox.Text.Trim(),
                                latitude, longitude, ContactNameTextBox.Text.Trim() == string.Empty ? null : ContactNameTextBox.Text.Trim(),
                                PhoneNoTextBox.Text.Trim() == string.Empty ? null : PhoneNoTextBox.Text.Trim(), MobileNoTextBox.Text.Trim() == string.Empty ? null : MobileNoTextBox.Text.Trim(),
                                AltMobileNoTextBox.Text.Trim() == string.Empty ? null : AltMobileNoTextBox.Text.Trim(), EmailIDTextBox.Text.Trim() == string.Empty ? null : EmailIDTextBox.Text.Trim(),
                                AltEmailIDTextBox.Text.Trim() == string.Empty ? null : AltEmailIDTextBox.Text.Trim(), FaxNoTextBox.Text.Trim() == string.Empty ? null : FaxNoTextBox.Text.Trim(), Page.User.Identity.Name, Session["BusDtl"].ToString().Split('|')[0].ToString(), businessLogo);
                            LoadBusinessDetails(Session["BusDtl"].ToString().Split('|').ToString());
                            WebMessenger.Show(this.Page, "Record Updated Successfully", "Success", DialogTypes.Success);                       
                            Session["ucPhotoUploaderUploadedImage"] = null;                            
                        }
                catch (Exception)
                {
                    WebMessenger.Show(this, "Server Error", "Error", DialogTypes.Error);
                    return;
                }
                finally
                {
                    objBusinessMstBLL = null;
                }
            }
            else
            {
                WebMessenger.Show(this, "Invalid Operation", "Invalid", DialogTypes.Error);
                return;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {          
        }
        #endregion

        
    }
}