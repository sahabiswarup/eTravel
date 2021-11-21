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

namespace e_Travel.BusinessAdmin
{
    public partial class ReqTestimonials : AdminBasePage
    {
        CMSTestimonialsBLL objPtlAdmTestimonialsBLL;
        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            TestimonailsCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(TestimonailsCustomGridView_SelectedIndexChanging);
            TestimonailsCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(TestimonailsCustomGridView_RowDeleting);
            TestimonailsCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void TestimonailsCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OpernType"] = null;
            DisableInputControls();
            ClientNameTextBox.Text = HttpUtility.HtmlDecode(TestimonailsCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            ClientMessageCKEditor.Text = TestimonailsCustomGridView.DataKeys[e.NewSelectedIndex].Values["ClientMessage"].ToString();
        }

        protected void TestimonailsCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();
            try
            {
                objPtlAdmTestimonialsBLL.DeleteTestimonials(TestimonailsCustomGridView.DataKeys[e.RowIndex].Values["TestimonialsID"].ToString());
                TestimonailsCustomGridView.BindData();
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
                int startRowIndex = (TestimonailsCustomGridView.PageIndex * TestimonailsCustomGridView.PageSize) + 1;
                dtRec = objPtlAdmTestimonialsBLL.GetAllTestimonailsPaged(Session["BusinessID"].ToString(), startRowIndex, TestimonailsCustomGridView.PageSize,null);
                if (dtRec.Rows.Count > 0)
                {
                    TestimonailsCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
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
            checkBusinessID();
            BindEvents();
            if (!IsPostBack)
            {
                TestimonailsCustomGridView.AddGridViewColumns("ClientName|Approved Status", "ClientName|ApprovedStatusName", "200|100", "TestimonialsID|ClientMessage", true, true);
                TestimonailsCustomGridView.BindData();
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
                ClientMessageCKEditor.Focus();
                this.Session["OpernType"] = "Add";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (TestimonailsCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                EnableInputControls();
                ClientMessageCKEditor.Focus();
                this.Session["OpernType"] = "Edit";
            }
        }

        
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objPtlAdmTestimonialsBLL = new CMSTestimonialsBLL();

            //Validation
            if (ClientNameTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Provide Client Name", "Information", DialogTypes.Information);
                return;
            }
            if (ClientMessageCKEditor.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Provide Client Message", "Information", DialogTypes.Information);
                return;
            }
            try
            {
                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            objPtlAdmTestimonialsBLL.AddTestimonials(Session["BusinessID"].ToString(), ClientNameTextBox.Text.Trim(), ClientMessageCKEditor.Text.Trim(), 1, Page.User.Identity.Name);
                            TestimonailsCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Testimonails Added Sucessfully", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objPtlAdmTestimonialsBLL.EditTestimonial(TestimonailsCustomGridView.SelectedDataKey.Values["TestimonialsID"].ToString(), ClientNameTextBox.Text.Trim(), ClientMessageCKEditor.Text.Trim(), 1, Page.User.Identity.Name);
                            TestimonailsCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Testimonails Updated Sucessfully", "Success", DialogTypes.Success);
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
                        WebMessenger.Show(this, "Select A Operation First", "Information", DialogTypes.Information);
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
                return;
            }
            finally
            {
                objPtlAdmTestimonialsBLL = null;
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
            ClientMessageCKEditor.Text = "";
            ClientNameTextBox.Text = "";
        }
        #endregion
        protected void checkBusinessID()
        {
            if (Session["BusinessID"] == null)
            {
                if (Request.Cookies["BusinessID"] != null)
                {
                    Session["BusinessID"] = Request.Cookies["BusinessID"].Value.ToString();
                }
                else
                {
                    if (Page.User.Identity.Name != "")
                    {
                        if (User.IsInRole("BusinessAdmin"))
                        {
                            Session["BusinessID"] = HttpContext.Current.Profile.GetPropertyValue("BusinessID").ToString();
                        }
                    }
                }
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
        }
    }
}