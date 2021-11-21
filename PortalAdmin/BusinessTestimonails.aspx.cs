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
using e_TravelBLL.BussinessManagement;

namespace e_Travel.PortalAdmin
{
    public partial class BusinessTestimonails : AdminBasePage
    {
        CMSTestimonialsBLL objPtlAdmTestimonialsBLL;

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            TestimonialsDtlsCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(TestimonialsDtlsCustomGridView_SelectedIndexChanging);
            TestimonialsDtlsCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(TestimonialsDtlsCustomGridView_RowDeleting);
            TestimonialsDtlsCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);


        }

        protected void TestimonialsDtlsCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (Session["BusDtl"] != null)
            {
                Session["OpernType"] = null;
                DisableInputControls();
                ClientNameTextBox.Text = HttpUtility.HtmlDecode(TestimonialsDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
                ClientMessageCKEditor.Text = TestimonialsDtlsCustomGridView.DataKeys[e.NewSelectedIndex].Values["ClientMessage"].ToString();

                if (TestimonialsDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text == "Requested")
                {
                    AddButton.Visible = true;
                    ClearButton.Visible = true;
                    EditButton.Visible = true;
                    RejectButton.Visible = true;
                    ApproveButton.Visible = true;
                    SaveButton.Visible = true;
                    RejectButton.Text = "Reject";
                }
                if (TestimonialsDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text == "Approved")
                {
                    AddButton.Visible = true;
                    ClearButton.Visible = true;
                    EditButton.Visible = true;
                    RejectButton.Visible = true;
                    ApproveButton.Visible = false;
                    SaveButton.Visible = true;
                    RejectButton.Text = "Undo Approved";
                }
                if (TestimonialsDtlsCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text == "Rejected")
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
            else
            {
                WebMessenger.Show(this, "please select bussiness first..", "Error", DialogTypes.Error);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx"); 
            }


        }

        protected void TestimonialsDtlsCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();
            try
            {
                objPtlAdmTestimonialsBLL.DeleteTestimonials(TestimonialsDtlsCustomGridView.DataKeys[e.RowIndex].Values["TestimonialsID"].ToString());
                TestimonialsDtlsCustomGridView.BindData();
                WebMessenger.Show(this, "Testimonials Deleted Successfully", "Deleted", DialogTypes.Success);

            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, "Unable To Delete Testimonials At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                ClearInputControls();
                DisableInputControls();
                Session["ucPhotoUploaderUploadedImage"] = null;
                objPtlAdmTestimonialsBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (TestimonialsDtlsCustomGridView.PageIndex * TestimonialsDtlsCustomGridView.PageSize) + 1;
                dtRec = objPtlAdmTestimonialsBLL.GetAllTestimonailsPaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, TestimonialsDtlsCustomGridView.PageSize, LinkStatusHiddenField.Value == "" ? null : LinkStatusHiddenField.Value);
                if (dtRec.Rows.Count > 0)
                {
                    TestimonialsDtlsCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Service List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objPtlAdmTestimonialsBLL = null;
            }
            return dtRec;
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AddButton.Visible = true;
            ClearButton.Visible = true;
            EditButton.Visible = false;
            RejectButton.Visible = false;
            SaveButton.Visible = true;
            ApproveButton.Visible = false;
            BindEvents();
            LoadAllBussinessDetail();
            if (!IsPostBack)
            {

                if (Session["BusDtl"] == null)
                {
                    WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                    Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
                }
                else
                {

                    TestimonialsDtlsCustomGridView.AddGridViewColumns("ClientName|Approved Status", "ClientName|ApprovedStatusName", "200|100", "TestimonialsID|ClientMessage", true, true);
                    TestimonialsDtlsCustomGridView.BindData();
                    DisableInputControls();
                    ClearInputControls();
                }
            }
        }
        #endregion

        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {

            if (Session["BusDtl"]==null)
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");

            }

            else
            {
                EnableInputControls();
                ClearInputControls();
                ClientMessageCKEditor.Focus();
                this.Session["OpernType"] = "Add";
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (Session["BusDtl"] != null)
            {

                if (TestimonialsDtlsCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select the record from list first...", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    EnableInputControls();
                    ClientMessageCKEditor.Focus();
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

                if (TestimonialsDtlsCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select the record from list first...", "Information", DialogTypes.Information);
                    return;
                }
                else
                {

                    short status = 0;
                    string message = string.Empty;

                    if (TestimonialsDtlsCustomGridView.SelectedRow.Cells[2].Text.Trim() == "Requested" && ApproveButton.Text.Trim() == "Approved" && RejectButton.Text.Trim() == "Reject")
                    {
                        status = 3;
                        message = "rejected successfully";
                    }
                    else if (TestimonialsDtlsCustomGridView.SelectedRow.Cells[2].Text.Trim() == "Rejected" && RejectButton.Text.Trim() == "Undo Reject")
                    {
                        status = 1;
                        message = "Undo reject successfully";
                    }
                    else if (TestimonialsDtlsCustomGridView.SelectedRow.Cells[2].Text.Trim() == "Approved" && RejectButton.Text.Trim() == "Undo Approved")
                    {
                        status = 1;
                        message = "Undo approved successfully";
                    }
                    else
                    {
                        WebMessenger.Show(this, "Invalid Operation..", "Information", DialogTypes.Information);                    
                        return;
                    }
                    objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();
                    try
                    {
                        objPtlAdmTestimonialsBLL.ChangeTestimonialsStatus(TestimonialsDtlsCustomGridView.SelectedDataKey.Values["TestimonialsID"].ToString(), status, Page.User.Identity.Name);
                        TestimonialsDtlsCustomGridView.BindData();
                        ClearInputControls();
                        WebMessenger.Show(this, "Testimonails Has Been " + message, "Success", DialogTypes.Success);
                    }
                    catch (Exception ex)
                    {
                        WebMessenger.Show(this, "Unable to " +RejectButton.Text+ " selected record at this moment", "Error", DialogTypes.Error);
                        return;
                    }
                    finally
                    {
                        objPtlAdmTestimonialsBLL = null;
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
            if (ClientNameTextBox.Text.Trim() == "" || string.IsNullOrEmpty(ClientNameTextBox.Text.Trim()))
            {
                WebMessenger.Show(this, "Provide Client Name", "Information", DialogTypes.Information);
                return;
            }
            if (ClientMessageCKEditor.Text.Trim() == "" || string.IsNullOrEmpty(ClientMessageCKEditor.Text.Trim()))
            {
                WebMessenger.Show(this, "Provide Client Message", "Information", DialogTypes.Information);
                return;
            }
            #endregion

            try
            {
                objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();

                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            objPtlAdmTestimonialsBLL.AddTestimonials(Session["BusDtl"].ToString().Split('|')[0].ToString(), ClientNameTextBox.Text.Trim(), ClientMessageCKEditor.Text.Trim(), 2, Page.User.Identity.Name);
                            TestimonialsDtlsCustomGridView.BindData();
                            //BusTestimonialsCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Testimonails added Sucessfully...", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objPtlAdmTestimonialsBLL.EditTestimonial(TestimonialsDtlsCustomGridView.SelectedDataKey.Values["TestimonialsID"].ToString(), ClientNameTextBox.Text.Trim(), ClientMessageCKEditor.Text.Trim(), 2, Page.User.Identity.Name);
                            TestimonialsDtlsCustomGridView.BindData();
                            //BusTestimonialsCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Testimonails updated sucessfully...", "Success", DialogTypes.Success);
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
                        WebMessenger.Show(this, "Select the operation first...", "Information", DialogTypes.Information);
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
                if (objPtlAdmTestimonialsBLL != null)
                {
                    objPtlAdmTestimonialsBLL = null;
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
          
            if (Session["BusDtl"] != null)
            {

                if (TestimonialsDtlsCustomGridView.SelectedDataKey == null)
                {
                    WebMessenger.Show(this, "Select the record from list first...", "Information", DialogTypes.Information);
                    return;
                }
                else
                {
                    try
                    {
                        objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();
                        objPtlAdmTestimonialsBLL.ChangeTestimonialsStatus(TestimonialsDtlsCustomGridView.SelectedDataKey.Values["TestimonialsID"].ToString(), 2, Page.User.Identity.Name);
                        TestimonialsDtlsCustomGridView.BindData();
                        ClearInputControls();
                        WebMessenger.Show(this, "Testimonails has been approved successfully" ,"Success", DialogTypes.Success);
                      
                    }
                    catch(Exception ex)
                    {
                        WebMessenger.Show(this, "Unable to aproved selected record at this moment", "Error", DialogTypes.Error);
                  
                    }
                    finally
                    {
                        objPtlAdmTestimonialsBLL = null;
                    }

                }
            }
            else
            {
                WebMessenger.Show(this.Page, "Session Expired... You will be redirected to login page...", "Session Expired...", DialogTypes.Information);
                Response.Redirect("~/PortalAdmin/BussinessSearchPage.aspx");
            }
        }
        #endregion

        #region Enable/Disable/Clear Form Elements
        private void EnableInputControls()
        {
            ClientMessageCKEditor.Enabled = true;
            ClientNameTextBox.Enabled = true;
        }

        private void DisableInputControls()
        {
            ClientMessageCKEditor.Enabled = false;
            ClientNameTextBox.Enabled = false;
        }

        private void ClearInputControls()
        {
            ClientMessageCKEditor.Text = string.Empty;
            ClientNameTextBox.Text = string.Empty;
            LinkStatusHiddenField.Value = string.Empty;
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
                    CMSHeaderLabel.Text = bussinessManagementDt.Rows[0]["BusName"].ToString() + " (Testimonials Details)";

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
            TestimonialsDtlsCustomGridView.BindData();
        }
        protected void ReqLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = "1";
            TestimonialsDtlsCustomGridView.BindData();
        }
        protected void AppLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = "2";
            TestimonialsDtlsCustomGridView.BindData();
        }
        protected void RejLinkButton_Click(object Seneder, EventArgs e)
        {
            ClearInputControls();
            LinkStatusHiddenField.Value = "3";
            TestimonialsDtlsCustomGridView.BindData();
        }
        
        
        
        #endregion

    }
}