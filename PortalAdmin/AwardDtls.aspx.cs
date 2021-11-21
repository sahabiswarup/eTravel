using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using e_Travel.Class;
using e_TravelBLL.CMS;

using Sibin.ExceptionHandling.ExceptionHandler;
using Sibin.FrameworkExtensions.DotNet.Web;
using Sibin.Utilities.Imaging.TwoD;
using e_TravelBLL.BussinessManagement;

namespace e_Travel.PortalAdmin
{
    public partial class AwardDtls : AdminBasePage
    {

        #region Private Variables
        CMSAwardDtlsBLL objPtlAdmAwardDtlsBLL;
        #endregion

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            AwardDtlsCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(AwardDtlsCustomGridView_SelectedIndexChanging);
            AwardDtlsCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(AwardDtlsCustomGridView_RowDeleting);
            AwardDtlsCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);

        }


        protected void AwardDtlsCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OpernType"] = null;
            DisableInputControls();
            AwardNameTextBox.Text = HttpUtility.HtmlDecode(AwardDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            AwardDescTextBox.Text = HttpUtility.HtmlDecode(AwardDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
            if (AwardDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text == "Requested")
            {
                AddButton.Visible = true;
                ClearButton.Visible = true;
                EditButton.Visible = true;
                RejectButton.Visible = true;
                ApproveButton.Visible = true;
                SaveButton.Visible = true;
                RejectButton.Text = "Reject";
            }
            if (AwardDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text == "Approved")
            {
                AddButton.Visible = true;
                ClearButton.Visible = true;
                EditButton.Visible = true;
                RejectButton.Visible = true;
                ApproveButton.Visible = false;
                SaveButton.Visible = true;
                RejectButton.Text = "Undo Approved";
            }
            if (AwardDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[3].Text == "Rejected")
            {
                AddButton.Visible = true;
                ClearButton.Visible = true;
                EditButton.Visible = true;
                RejectButton.Visible = true;
                ApproveButton.Visible = false;
                SaveButton.Visible = true;
                RejectButton.Text = "Undo Reject";
            }
        }

        protected void AwardDtlsCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objPtlAdmAwardDtlsBLL = new CMSAwardDtlsBLL();
            try
            {
                objPtlAdmAwardDtlsBLL.DeleteAwardDtls(AwardDtlsCustomGridView.DataKeys[e.RowIndex].Values["AwardID"].ToString());
                AwardDtlsCustomGridView.BindData();
                WebMessenger.Show(this, "Award Deleted Successfully", "Deleted", DialogTypes.Success);

            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, "Unable To Delete Award At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                ClearInputControls();
                DisableInputControls();
                Session["ucPhotoUploaderUploadedImage"] = null;
                objPtlAdmAwardDtlsBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objPtlAdmAwardDtlsBLL = new CMSAwardDtlsBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (AwardDtlsCustomGridView.PageIndex * AwardDtlsCustomGridView.PageSize) + 1;
                dtRec = objPtlAdmAwardDtlsBLL.GetAllAwardDtlsPaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, AwardDtlsCustomGridView.PageSize, LinkStatusHiddenField.Value == "" ? null : LinkStatusHiddenField.Value);
                if (dtRec.Rows.Count > 0)
                {
                    AwardDtlsCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Service List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objPtlAdmAwardDtlsBLL = null;
            }
            return dtRec;
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            BindEvents();
            AddButton.Visible = true;
            ClearButton.Visible = true;
            EditButton.Visible = false;
            RejectButton.Visible = false;
            SaveButton.Visible = true;
            ApproveButton.Visible = false;
            LoadAllBussinessDetail();


            Sibin.Utilities.Imaging.TwoD.ImageEditingUtility sec = new ImageEditingUtility();
            if (Session["BusDtl"] == null)
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ImageCroppingScript"))
            {
                string str = sec.GetImageCroppingScriptClientCode();
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "ImageCroppingScript", str);
            }
            if (!IsPostBack)
            {
                AwardDtlsCustomGridView.AddGridViewColumns("Award Name |Award Description|Approved Status", "AwardName|AwardDesc|ApprovedStatusName", "200|350|100", "AwardID|AwardImage|AwardAttachment", true, true);
                AwardDtlsCustomGridView.BindData();
                DisableInputControls();
                ClearInputControls();
            }
        }
        #endregion

        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }
            else
            {
                EnableInputControls();
                ClearInputControls();
                AwardNameTextBox.Focus();
                this.Session["OpernType"] = "Add";
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (Session["BusDtl"]!= null)
            {
                if (AwardDtlsCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    EnableInputControls();
                    AwardNameTextBox.Focus();
                    this.Session["OpernType"] = "Edit";
                }
            }
            else
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }
        }

        protected void RejectButton_Click(object sender, EventArgs e)
        {
            if (Session["BusDtl"] != null)
            {

                if (AwardDtlsCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                    return;
                }
                else
                {

                    short status = 0;
                    string message = string.Empty;

                    if (AwardDtlsCustomGridView.SelectedRow.Cells[3].Text.Trim() == "Requested" && ApproveButton.Text.Trim() == "Approved" && RejectButton.Text.Trim() == "Reject")
                    {
                        status = 3;
                        message = "rejected successfully";
                    }
                    else if (AwardDtlsCustomGridView.SelectedRow.Cells[3].Text.Trim() == "Rejected" && RejectButton.Text.Trim() == "Undo Reject")
                    {
                        status = 1;
                        message = "Undo reject successfully";
                    }
                    else if (AwardDtlsCustomGridView.SelectedRow.Cells[3].Text.Trim() == "Approved" && RejectButton.Text.Trim() == "Undo Approved")
                    {
                        status = 1;
                        message = "Undo approved successfully";
                    }
                    else
                    {
                        WebMessenger.Show(this, "Invalid Operation..", "Information", DialogTypes.Information);
                        return;
                    }
                    objPtlAdmAwardDtlsBLL = new CMSAwardDtlsBLL();
                    try
                    {
                        objPtlAdmAwardDtlsBLL.ChangeAwardStatus(AwardDtlsCustomGridView.SelectedDataKey.Values["AwardID"].ToString(), status, Page.User.Identity.Name);
                        AwardDtlsCustomGridView.BindData();
                        ClearInputControls();
                        WebMessenger.Show(this, "Award has been " + message, "Success", DialogTypes.Success);

                    }
                    catch (Exception ex)
                    {
                        WebMessenger.Show(this, "Unable to " + RejectButton.Text + " selected record at this moment", "Error", DialogTypes.Error);
                        return;
                    }
                    finally
                    {
                        objPtlAdmAwardDtlsBLL = null;
                    }

                }
            }
            else
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            #region server side validation
            if (AwardNameTextBox.Text.Trim() == "" || string.IsNullOrEmpty(AwardNameTextBox.Text.Trim()))
            {
                WebMessenger.Show(this, "Provide Award Name", "Information", DialogTypes.Information);
                return;
            }


            if (AwardDescTextBox.Text.Trim() == "" || string.IsNullOrEmpty(AwardDescTextBox.Text.Trim()))
            {
                WebMessenger.Show(this, "Provide Award  Description", "Information", DialogTypes.Information);
                return;
            }
            #endregion

            try
            {
                objPtlAdmAwardDtlsBLL = new CMSAwardDtlsBLL();

                if (Page.IsValid)
                {
                    byte[] Photograph = null;
                    string file = null;
                    if (Session["ucPhotoUploaderUploadedImage"] != null)
                    {
                        Photograph = (byte[])Session["ucPhotoUploaderUploadedImage"];
                        Photograph = ImageShaper.ResizeImage(Photograph, 600, 480);
                    }
                    if (AwardAttachmentFileUpload.HasFile)
                    {
                        file = Convert.ToBase64String(AwardAttachmentFileUpload.FileBytes);

                    }

                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {

                            objPtlAdmAwardDtlsBLL.AddAwardDtls(Session["BusDtl"].ToString().Split('|')[0].ToString(), AwardNameTextBox.Text.Trim(), AwardDescTextBox.Text.Trim(), Photograph, file, 2, Page.User.Identity.Name);
                            AwardDtlsCustomGridView.BindData();

                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Award  added sucessfully", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objPtlAdmAwardDtlsBLL.EditAwardDtls(AwardDtlsCustomGridView.SelectedDataKey.Values["AwardID"].ToString(), AwardNameTextBox.Text.Trim(), AwardDescTextBox.Text.Trim(), Photograph, file, 2, Page.User.Identity.Name);
                            AwardDtlsCustomGridView.BindData();

                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Award  updated sucessfully...", "Success", DialogTypes.Success);
                        }
                        else
                        {
                            WebMessenger.Show(this, "Invalid Operation...", "Operation", DialogTypes.Information);
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                        }
                    }
                    else
                    {
                        WebMessenger.Show(this, "Select a operation first...", "Information", DialogTypes.Information);
                        return;
                    }
                }
                else
                {
                    WebMessenger.Show(this, "Invalid Operation...", "Operation", DialogTypes.Information);
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
                return;
            }
            finally
            {
                if (objPtlAdmAwardDtlsBLL != null)
                {
                    objPtlAdmAwardDtlsBLL = null;
                }
            }
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearInputControls();
            DisableInputControls();
            this.Session["OpernType"] = null;
        }

        protected void ApproveButton_Click(object sender, EventArgs e)
        {

            if (Session["BusDtl"]!= null)
            {

                if (AwardDtlsCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select a record from the list first...", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    try
                    {
                        objPtlAdmAwardDtlsBLL = new CMSAwardDtlsBLL();
                        objPtlAdmAwardDtlsBLL.ChangeAwardStatus(AwardDtlsCustomGridView.SelectedDataKey.Values["AwardID"].ToString(), 2, Page.User.Identity.Name);
                        AwardDtlsCustomGridView.BindData();
                        ClearInputControls();
                        WebMessenger.Show(this, "Award has been approved successfully..", "Success", DialogTypes.Success);

                    }
                    catch (Exception ex)
                    {
                        WebMessenger.Show(this, "Unable to aproved selected record at this moment", "Error", DialogTypes.Error);

                    }
                    finally
                    {
                        objPtlAdmAwardDtlsBLL = null;
                    }

                }
            }
            else
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }
        }

        protected void btnClearImage2_Click(object sender, EventArgs e)
        {
            Session["ucPhotoUploaderUploadedImagePhoto"] = null;


        }
        #endregion

        #region Enable/Disable/Clear Form Elements
        private void EnableInputControls()
        {
            AwardNameTextBox.Enabled = true;
            AwardDescTextBox.Enabled = true;
            AwardAttachmentFileUpload.Enabled = true;
            this.RegisterJS("$('#photographToUpload').css('pointer-events','');");
        }

        private void DisableInputControls()
        {
            AwardNameTextBox.Enabled = false;
            AwardDescTextBox.Enabled = false;
            AwardAttachmentFileUpload.Enabled = false;
            this.RegisterJS("$('#photographToUpload').css('pointer-events','none');");
        }

        private void ClearInputControls()
        {
            AwardNameTextBox.Text = "";
            AwardDescTextBox.Text = "";
            AwardAttachmentFileUpload.Attributes.Clear();
            this.RegisterJS("$('#photographToUpload').val('');");
            Session["ucPhotoUploaderUploadedImage"] = null;
            LinkStatusHiddenField.Value = null;
        }
        #endregion

        #region Load all Bussiness details by BussinessID
        private void LoadAllBussinessDetail()
        {
            BussinessManagementBLL bussinessManagementObj;
            DataTable bussinessManagementDt = null;
            try
            {
                bussinessManagementObj = new BussinessManagementBLL();
                bussinessManagementDt = bussinessManagementObj.GetAllBussinessDetailsByID(Session["BusDtl"].ToString().Split('|')[0].ToString());
                if (bussinessManagementDt.Rows.Count > 0)
                {
                    CMSHeaderLabel.Text = bussinessManagementDt.Rows[0]["BusName"].ToString() + " (Award Details)";

                }
                else
                {
                    WebMessenger.Show(this, "Loading failed...", "Information", DialogTypes.Information);
                    Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
                }

            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error occured..", "Error", DialogTypes.Error);
            }
            finally
            {
                bussinessManagementObj = null;
                bussinessManagementDt.Dispose();

            }

        }
        #endregion

        #region All link button events

        protected void AllLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = null;
            AwardDtlsCustomGridView.BindData();
        }
        protected void ReqLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = "1";
            AwardDtlsCustomGridView.BindData();
        }
        protected void AppLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = "2";
            AwardDtlsCustomGridView.BindData();
        }
        protected void RejLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = "3";
            AwardDtlsCustomGridView.BindData();
        }



        #endregion

    }
}