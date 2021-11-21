using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using e_Travel.Class;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sibin.Utilities.Web.ExceptionHandling;
using System.Data;
using SIBINUtility.ValidatorClass;
using e_TravelBLL.TourPackage;
using Sibin.Utilities.Imaging.TwoD;
using Sibin.FrameworkExtensions.DotNet.Web;
namespace e_Travel.BusinessAdmin
{
    public partial class DestinationDtlMaster : AdminBasePage
    {
        #region Private Variables
        DestinationDtlBLL objDestinationDtlBLL;
        byte[] AccImage;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                BindEvents();
                Sibin.Utilities.Imaging.TwoD.ImageEditingUtility sec = new ImageEditingUtility();
                if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ImageCroppingScript"))
                {
                    string str = sec.GetImageCroppingScriptClientCode();
                    this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "ImageCroppingScript", str);
                }
                if (Session["BusDtl"] == null)
                {
                    Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
                }
                if (!IsPostBack)
                {
                    DestinationDtlCustomGridView.AddGridViewColumns("Tagline|Description", "Tagline|Description", "200|300", "DestDtlID|BussinessID|Hierarchy|DefaultPhotoNormal|DefaultPhotoThumb", true, true);
                    DestinationDtlCustomGridView.BindData();

                    EnableDisableControls(false);
                    ClearControls();
                    CheckImage();
                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }


        }

        
     

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            DestinationDtlCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(DestinationDtlCustomGridView_SelectedIndexChanging);
            DestinationDtlCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(DestinationDtlCustomGridView_RowDeleting);
            DestinationDtlCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void DestinationDtlCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                Session["OpernType"] = null;
                EnableDisableControls(false);
                TaglineTextBox.Text = DestinationDtlCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text;
                DescTextBox.Text = DestinationDtlCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text;
                HirarchyTextBox.Text = DestinationDtlCustomGridView.DataKeys[e.NewSelectedIndex].Values["Hierarchy"].ToString();
                if (DestinationDtlCustomGridView.DataKeys[e.NewSelectedIndex].Values["DefaultPhotoThumb"].ToString() != "")
                {
                    Session["ucPhotoUploaderUploadedImage"] = (byte[])(DestinationDtlCustomGridView.DataKeys[e.NewSelectedIndex].Values["DefaultPhotoNormal"]);
                    
                }
                CheckImage();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }

        }

        protected void DestinationDtlCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objDestinationDtlBLL = new DestinationDtlBLL();
            try
            {
                objDestinationDtlBLL.DeleteDestinationDtl(DestinationDtlCustomGridView.DataKeys[e.RowIndex].Values["DestDtlID"].ToString(), DestinationDtlCustomGridView.DataKeys[e.RowIndex].Values["BussinessID"].ToString());
                DestinationDtlCustomGridView.BindData();
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, "Unable To Delete Country At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                EnableDisableControls(false);
                objDestinationDtlBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objDestinationDtlBLL = new DestinationDtlBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (DestinationDtlCustomGridView.PageIndex * DestinationDtlCustomGridView.PageSize) + 1;
                dtRec = objDestinationDtlBLL.GetAllDestinationDtlByPaged(startRowIndex, DestinationDtlCustomGridView.PageSize,Session["BusDtl"].ToString().Split('|')[0].ToString());
                if (dtRec.Rows.Count > 0)
                {
                    DestinationDtlCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Destination List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objDestinationDtlBLL = null;
            }
            return dtRec;
        }
        #endregion

        #region Enable/Disable/Clear Form Elements
        private void EnableDisableControls(bool isEnabled)
        {
            TaglineTextBox.Enabled = isEnabled;
            HirarchyTextBox.Enabled = isEnabled;
            DescTextBox.Enabled = isEnabled;
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


        private void ClearControls()
        {
            TaglineTextBox.Text = string.Empty;
            HirarchyTextBox.Text = string.Empty;
            DescTextBox.Text = string.Empty;
            Session["ucPhotoUploaderUploadedImage"] = null;
            CheckImage();

        }
        #endregion

        #region All button events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableControls(true);
                ClearControls();
                this.Session["OpernType"] = "Add";
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (DestinationDtlCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    this.RegisterJS("loadImage();");
                    Session["OpernType"] = "Update";
                    EnableDisableControls(true);
                    TaglineTextBox.Focus();
                    CheckImage();

                }
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, ex.Message, "Error", DialogTypes.Error);
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {

            //Validation
            if (string.IsNullOrEmpty(TaglineTextBox.Text.Trim()))
            {
                WebMessenger.Show(this, "Tagline is mandatory..", "Information", DialogTypes.Information);
                TaglineTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(HirarchyTextBox.Text.Trim()))
            {
                WebMessenger.Show(this, "Hirarchy is mandatory..", "Information", DialogTypes.Information);
                HirarchyTextBox.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(HirarchyTextBox.Text.Trim()))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(HirarchyTextBox.Text.Trim(), @"^([0-9]|10|0[0-9])$"))
                {
                    WebMessenger.Show(this, "Invalid input,enter hirarchy between 0-10..", "Information", DialogTypes.Information);
                    HirarchyTextBox.Focus();
                    return;
                }

            }
            if (string.IsNullOrEmpty(DescTextBox.Text))
            {
                WebMessenger.Show(this, "Description is mandatory..", "Information", DialogTypes.Information);
                DescTextBox.Focus();
                return;
            }
            if (Session["ucPhotoUploaderUploadedImage"] == null)
            {
                WebMessenger.Show(this, "upload image is mandatory..", "Information", DialogTypes.Information);
                return;
            }

            objDestinationDtlBLL = new DestinationDtlBLL();

            try
            {
                if (Session["ucPhotoUploaderUploadedImage"] != null)
                {
                    AccImage = ((byte[])Session["ucPhotoUploaderUploadedImage"]);
                }
                else
                {
                    AccImage = null;
                }
                byte[] PhotoThumb = null;
                if (AccImage != null)
                {
                    AccImage = ImageShaper.ResizeImage(AccImage, 600, 300);
                    PhotoThumb = ImageShaper.ResizeImage(AccImage, 100, 50);
                }

                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            objDestinationDtlBLL.AddDestinationDtl(Session["BusDtl"].ToString().Split('|')[0].ToString(), AccImage, PhotoThumb, TaglineTextBox.Text.Trim(), byte.Parse(HirarchyTextBox.Text), DescTextBox.Text, Page.User.Identity.Name);
                            DestinationDtlCustomGridView.BindData();
                            ClearControls();
                            EnableDisableControls(false);
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Destination details added sucessfully..", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Update")
                        {
                            objDestinationDtlBLL.UpdateDestinationDtl(DestinationDtlCustomGridView.SelectedDataKey["DestDtlID"].ToString(), DestinationDtlCustomGridView.SelectedDataKey["BussinessID"].ToString(), AccImage, PhotoThumb, TaglineTextBox.Text.Trim(), byte.Parse(HirarchyTextBox.Text), DescTextBox.Text, Page.User.Identity.Name);
                            DestinationDtlCustomGridView.BindData();
                            ClearControls();
                            EnableDisableControls(false);
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Destination details Updated sucessfully..", "Success", DialogTypes.Success);
                        }
                        else
                        {
                            WebMessenger.Show(this, "Invalid Operation..", "Operation", DialogTypes.Information);
                            ClearControls();
                            EnableDisableControls(false);
                            Session["OpernType"] = null;
                        }
                    }
                    else
                    {
                        WebMessenger.Show(this, "Invalid Operation..", "Operation", DialogTypes.Information);
                        ClearControls();
                        EnableDisableControls(false);
                        Session["OpernType"] = null;
                    }


                }
                else
                {
                    WebMessenger.Show(this, "Invalid Operation", "Operation", DialogTypes.Information);
                    ClearControls();
                    EnableDisableControls(false);
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
                objDestinationDtlBLL = null;
            }


        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            EnableDisableControls(false);
            ClearControls();
            CheckImage();
        }
        #endregion

        protected void ClearImageButton_Click(object sender, EventArgs e)
        {
            Session["ucPhotoUploaderUploadedImage"] = null;
            CheckImage();
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
    }
}