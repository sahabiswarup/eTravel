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


namespace e_Travel.BusinessAdmin
{
    public partial class ReqMembership : AdminBasePage
    {
        CMSMembershipDtlsBLL objCMSMembershipDtlsBLL;
        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            MemberShipCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(MemberShipCustomGridView_SelectedIndexChanging);
            MemberShipCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(MemberShipCustomGridView_RowDeleting);
            MemberShipCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void MemberShipCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OpernType"] = null;
            ClearInputControls();
            DisableInputControls();
            OrganisationDropDownList.SelectedValue = HttpUtility.HtmlDecode(MemberShipCustomGridView.DataKeys[e.NewSelectedIndex].Values["OrganisationID"].ToString());
            MembershipDescTextBox.Text = HttpUtility.HtmlDecode(MemberShipCustomGridView.DataKeys[e.NewSelectedIndex].Values["MembershipDesc"].ToString());
        }

        protected void MemberShipCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objCMSMembershipDtlsBLL = new CMSMembershipDtlsBLL();
            try
            {
                objCMSMembershipDtlsBLL.DeleteMembership(MemberShipCustomGridView.DataKeys[e.RowIndex].Values["MembershipID"].ToString());
               
                MemberShipCustomGridView.BindData();
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
                objCMSMembershipDtlsBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objCMSMembershipDtlsBLL = new CMSMembershipDtlsBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (MemberShipCustomGridView.PageIndex * MemberShipCustomGridView.PageSize) + 1;
                dtRec = objCMSMembershipDtlsBLL.GetAllMembershipPaged(Session["BusinessID"].ToString(), startRowIndex, MemberShipCustomGridView.PageSize,null);
                if (dtRec.Rows.Count > 0)
                {
                    MemberShipCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Membership List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objCMSMembershipDtlsBLL = null;
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
                MemberShipCustomGridView.AddGridViewColumns("OrganisationName|Approved Status", "OrganisationName|ApprovedStatusName", "200|100", "MembershipID|MembershipDesc|Attachment|OrganisationID", true, true);
                MemberShipCustomGridView.BindData();
                DisableInputControls();
                ClearInputControls();
                LoadOrganisation();
            }
        }
        #endregion

        #region Button Click Events
        protected void AddButton_Click(object sender, EventArgs e)
        {
            EnableInputControls();
            ClearInputControls();
            OrganisationDropDownList.Focus();
            this.Session["OpernType"] = "Add";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (MemberShipCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                EnableInputControls();
                OrganisationDropDownList.Focus();
                this.Session["OpernType"] = "Edit";
            }
        }


        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objCMSMembershipDtlsBLL = new CMSMembershipDtlsBLL();

            //Validation
            string file=null;
            if (OrganisationDropDownList.SelectedValue == "0")
            {
                WebMessenger.Show(this, "Provide Organisation Name", "Information", DialogTypes.Information);
                return;
            }
            if (MembershipDescTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Provide Membership Description", "Information", DialogTypes.Information);
                return;
            }
            if (Session["ucFileUploaderUploadedFile"] != null)
            {
                file = Convert.ToBase64String((byte[])Session["ucFileUploaderUploadedFile"]) ;
            }
            try
            {
                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            objCMSMembershipDtlsBLL.AddMembershipDtls(Session["BusinessID"].ToString(),Int64.Parse(OrganisationDropDownList.SelectedValue),MembershipDescTextBox.Text.Trim(),file,1,Page.User.Identity.Name);
                            MemberShipCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Membership Request Sended Sucessfully", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objCMSMembershipDtlsBLL.EditMembershipDtls(MemberShipCustomGridView.SelectedDataKey.Values["MembershipID"].ToString(), Int64.Parse(OrganisationDropDownList.SelectedValue), MembershipDescTextBox.Text.Trim(), file, 1, Page.User.Identity.Name);
                            MemberShipCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Membership Request Sended Sucessfully", "Success", DialogTypes.Success);
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
                objCMSMembershipDtlsBLL = null;
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
            OrganisationDropDownList.Enabled = true;
            MembershipDescTextBox.Enabled = true;
            this.RegisterJS("$('#photographToUpload').css('pointer-events','');");
        }

        private void DisableInputControls()
        {
            OrganisationDropDownList.Enabled = false;
            MembershipDescTextBox.Enabled = false;
            this.RegisterJS("$('#photographToUpload').css('pointer-events','none');");
        }

        private void ClearInputControls()
        {
            OrganisationDropDownList.SelectedValue = "0";
            MembershipDescTextBox.Text = "";
            this.RegisterJS("$('#photographToUpload').val('');");
        }
        #endregion

        private void LoadOrganisation()
        {
            objCMSMembershipDtlsBLL = new CMSMembershipDtlsBLL();
            try
            {
                OrganisationDropDownList.Items.Clear();
                OrganisationDropDownList.Items.Add(new ListItem("--- Select Organisation  ---", "0"));
                OrganisationDropDownList.DataSource = objCMSMembershipDtlsBLL.GetAllOrganisation();
                OrganisationDropDownList.DataTextField = "OrganisationName";
                OrganisationDropDownList.DataValueField = "OrganisationID";
                OrganisationDropDownList.DataBind();
            }
            catch (Exception ex)
            {
                WebMessenger.Show(this, "Error:" + ex, "Error Occured", DialogTypes.Error);
                return;
            }
            finally
            {
                objCMSMembershipDtlsBLL = null;
            }
        }
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