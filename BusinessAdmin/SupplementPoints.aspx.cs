using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using Sibin.Utilities.Web.ExceptionHandling;
using SIBINUtility.ValidatorClass;

using e_Travel.Class;
using e_TravelBLL.TourPackage;

namespace e_Travel.BusinessAdmin
{
    public partial class SupplementPoints : AdminBasePage
    {
        SupplementPointsBLL objSupplementPointsBLL;
        
        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            SuppPointsCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(SuppPointsCustomGridView_SelectedIndexChanging);
            SuppPointsCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(SuppPointsCustomGridView_RowDeleting);
            SuppPointsCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void SuppPointsCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OpernType"] = null;
            DisableInputControls();
            SupplementDescTextBox.Text = HttpUtility.HtmlDecode(SuppPointsCustomGridView.Rows[e.NewSelectedIndex].Cells[2].Text);
            SupplementNameTextBox.Text = HttpUtility.HtmlDecode(SuppPointsCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            
            if (HttpUtility.HtmlDecode(SuppPointsCustomGridView.DataKeys[e.NewSelectedIndex].Values["IsPerPax"].ToString()) == "True")
            {
                PerPaxCheckBox.Checked = true;
                //MaximumCapacityTextBox.Text = SuppPointsCustomGridView.DataKeys[e.NewSelectedIndex].Values["MaximumCapacity"].ToString();
            }
            else
            {
                PerPaxCheckBox.Checked = false;
            }
           
        }

        protected void SuppPointsCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objSupplementPointsBLL = new SupplementPointsBLL();
            try
            {
                objSupplementPointsBLL.DeleteSupplementPoints(SuppPointsCustomGridView.DataKeys[e.RowIndex].Values["SupplementPointID"].ToString(),Page.User.Identity.Name);
                WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
                SuppPointsCustomGridView.BindData();
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, "Unable To Delete Supplement Points At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                ClearInputControls();
                DisableInputControls();
                objSupplementPointsBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objSupplementPointsBLL = new SupplementPointsBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (SuppPointsCustomGridView.PageIndex * SuppPointsCustomGridView.PageSize) + 1;
                dtRec = objSupplementPointsBLL.GetAllSupplementPointsPaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, SuppPointsCustomGridView.PageSize);
                if (dtRec.Rows.Count > 0)
                {
                    SuppPointsCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Supplement Points List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objSupplementPointsBLL = null;
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
                SuppPointsCustomGridView.AddGridViewColumns("Supplement Point Name |SupplementDesc", "SupplementName|SupplementDesc", "200|200", "SupplementPointID|IsPerPax|MaximumCapacity", true, true);
                SuppPointsCustomGridView.BindData();
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
            SupplementNameTextBox.Focus();
            this.Session["OpernType"] = "Add";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (SuppPointsCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                EnableInputControls();
                SupplementNameTextBox.Focus();
                this.Session["OpernType"] = "Edit";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objSupplementPointsBLL = new SupplementPointsBLL();
            //Validation
            Int16? MaximumCapacity = null;
            int status=0;
            if (SupplementNameTextBox.Text.Trim()=="")
            {
                WebMessenger.Show(this, "Supplement Point Name Is Required", "Information", DialogTypes.Information);
                SupplementNameTextBox.Focus();
                return;
            }
            if (SupplementDescTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Supplement Point Description Is Required", "Information", DialogTypes.Information);
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
                            objSupplementPointsBLL.AddSupplementPoint(Session["BusDtl"].ToString().Split('|')[0].ToString(), SupplementNameTextBox.Text.Trim(), SupplementDescTextBox.Text.Trim(), PerPaxCheckBox.Checked, MaximumCapacity, Page.User.Identity.Name, ref status);
                            if (status == 0)
                            {
                                SuppPointsCustomGridView.BindData();
                                ClearInputControls();
                                DisableInputControls();
                                Session["OpernType"] = null;
                                WebMessenger.Show(this, "Supplement Point Added Sucessfully", "Success", DialogTypes.Success);
                            }
                            else
                            {
                                WebMessenger.Show(this, "Same Supplement Point Cannot Be  Added Twice", "Success", DialogTypes.Success);
                            }
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objSupplementPointsBLL.EditSupplementPoint(Session["BusDtl"].ToString().Split('|')[0].ToString(), SuppPointsCustomGridView.SelectedDataKey.Values["SupplementPointID"].ToString(), SupplementNameTextBox.Text.Trim(), SupplementDescTextBox.Text.Trim(), PerPaxCheckBox.Checked, MaximumCapacity, Page.User.Identity.Name, ref status);
                             if (status == 0)
                            {
                            SuppPointsCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Supplement Point Updated Sucessfully", "Success", DialogTypes.Success);
                            }
                             else
                             {
                                 WebMessenger.Show(this, "Same Supplement Point Cannot Be  Added Twice", "Success", DialogTypes.Success);
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
                        WebMessenger.Show(this, "Select A Opeartion First", "Information", DialogTypes.Information);
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
                objSupplementPointsBLL = null;
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
            SupplementNameTextBox.Enabled = true;
            SupplementDescTextBox.Enabled = true;
            PerPaxCheckBox.Enabled = true;
        }

        private void DisableInputControls()
        {
            SupplementNameTextBox.Enabled = false;
            SupplementDescTextBox.Enabled = false;
            PerPaxCheckBox.Enabled = false;
        }

        private void ClearInputControls()
        {
            SupplementNameTextBox.Text = "";
            SupplementDescTextBox.Text = "";
            PerPaxCheckBox.Checked = false;
        }
        #endregion
 
    }
}