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
    public partial class IncludingExcluding : AdminBasePage
    {
        IncludingExcludingMstBLL objIncludingExcludingMstBLL;

        #region Gridview Events and Load GridView
        private void BindEvents()
        {
            ItemCustomGridView.ucGridView_SelectedIndexChanging += new GridViewSelectEventHandler(ItemCustomGridView_SelectedIndexChanging);
            ItemCustomGridView.ucGridView_RowDeleting += new GridViewDeleteEventHandler(ItemCustomGridView_RowDeleting);
            ItemCustomGridView.DataSource += new SIBINUtility.UserControl.GridView.ucGridView.GridViewDataSource(LoadGridView);
        }

        protected void ItemCustomGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session["OpernType"] = null;
            DisableInputControls();
            ItemDescTextBox.Text = HttpUtility.HtmlDecode(ItemCustomGridView.DataKeys[e.NewSelectedIndex].Values["ItemDesc"].ToString());
            ItemNameTextBox.Text = HttpUtility.HtmlDecode(ItemCustomGridView.Rows[e.NewSelectedIndex].Cells[1].Text);
            if (HttpUtility.HtmlDecode(ItemCustomGridView.DataKeys[e.NewSelectedIndex].Values["IsIncluding"].ToString()) == "True")
            {
                IncludingRadioButton.Checked = true;
            }
            else
            {
                ExcludingRadioButton.Checked = false;
            }
        }

        protected void ItemCustomGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            objIncludingExcludingMstBLL = new IncludingExcludingMstBLL();
            try
            {
                objIncludingExcludingMstBLL.DeleteItem(ItemCustomGridView.DataKeys[e.RowIndex].Values["InclusionExclusionID"].ToString());
                WebMessenger.Show(this.Page, "Record Deleted Successfully", "Deleted", DialogTypes.Success);
                ItemCustomGridView.BindData();
            }
            catch (Exception ex)
            {
                UserInterfaceExceptionHandler.HandleException(ref ex);
                Session["OpernType"] = null;
                WebMessenger.Show(this, "Unable To Delete Item At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                ClearInputControls();
                DisableInputControls();
                objIncludingExcludingMstBLL = null;
            }
        }

        private DataTable LoadGridView()
        {
            objIncludingExcludingMstBLL = new IncludingExcludingMstBLL();
            DataTable dtRec = new DataTable();
            try
            {
                int startRowIndex = (ItemCustomGridView.PageIndex * ItemCustomGridView.PageSize) + 1;
                dtRec = objIncludingExcludingMstBLL.GetItemPaged(Session["BusDtl"].ToString().Split('|')[0].ToString(), startRowIndex, ItemCustomGridView.PageSize);
                if (dtRec.Rows.Count > 0)
                {
                    ItemCustomGridView.TotalRecordCount = int.Parse(dtRec.Rows[0]["TotalRows"].ToString());
                }
            }
            catch (Exception)
            {
                WebMessenger.Show(this, "Unable To Load Question List At This Moment", "Error", DialogTypes.Error);
            }
            finally
            {
                objIncludingExcludingMstBLL = null;
            }
            return dtRec;
        }
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BusDtl"] == null)
            {
                Response.Redirect("~/PortalAdmin/RegisterBusiness.aspx");
            }
            BindEvents();
            if (!IsPostBack)
            {
                ItemCustomGridView.AddGridViewColumns("Item Name |Including/Excluding", "ItemName|TypeName", "200|350", "InclusionExclusionID|ItemDesc|IsIncluding", true, true);
                ItemCustomGridView.BindData();
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
            ItemNameTextBox.Focus();
            this.Session["OpernType"] = "Add";
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            if (ItemCustomGridView.SelectedDataKey == null)
            {
                WebMessenger.Show(this, "Select A Record From List First", "Information", DialogTypes.Information);
                return;
            }
            else
            {
                EnableInputControls();
                ItemNameTextBox.Focus();
                this.Session["OpernType"] = "Edit";
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            objIncludingExcludingMstBLL = new IncludingExcludingMstBLL();
            bool IsIncluding = true;
            
            if (ItemNameTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Item Name Is Required", "Information", DialogTypes.Information);
                return;
            }

            if (ItemDescTextBox.Text.Trim() == "")
            {
                WebMessenger.Show(this, "Item Description Is Required", "Information", DialogTypes.Information);
                return;
            }
            if (ExcludingRadioButton.Checked == false && IncludingRadioButton.Checked == false)
            {
                WebMessenger.Show(this, "Select Whether Item Is Including Or Excluding", "Information", DialogTypes.Information);
                return;
            }
            if (IncludingRadioButton.Checked == true)
            {
                IsIncluding = true;
            }
            else
            {
                IsIncluding = false;
            }
           


            try
            {
                if (Page.IsValid)
                {
                    if (Session["OpernType"] != null)
                    {
                        if (Session["OpernType"].ToString() == "Add")
                        {
                            objIncludingExcludingMstBLL.AddItem(Session["BusDtl"].ToString().Split('|')[0].ToString(),ItemNameTextBox.Text.Trim(),ItemDescTextBox.Text.Trim(),IsIncluding, Page.User.Identity.Name);
                            ItemCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Item Added Sucessfully", "Success", DialogTypes.Success);
                        }
                        else if (Session["OpernType"].ToString() == "Edit")
                        {
                            objIncludingExcludingMstBLL.EditItem(ItemCustomGridView.SelectedDataKey.Values["InclusionExclusionID"].ToString(), ItemNameTextBox.Text.Trim(), ItemDescTextBox.Text.Trim(), IsIncluding, Page.User.Identity.Name);
                            ItemCustomGridView.BindData();
                            ClearInputControls();
                            DisableInputControls();
                            Session["OpernType"] = null;
                            WebMessenger.Show(this, "Item Updated Sucessfully", "Success", DialogTypes.Success);
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
                objIncludingExcludingMstBLL = null;
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
            ItemNameTextBox.Enabled = true;
            ItemDescTextBox.Enabled = true;
            IncludingRadioButton.Enabled = true;
            ExcludingRadioButton.Enabled = true;
        }

        private void DisableInputControls()
        {
            ItemNameTextBox.Enabled = false;
            ItemDescTextBox.Enabled = false;
            IncludingRadioButton.Enabled = false;
            ExcludingRadioButton.Enabled = false;
        }

        private void ClearInputControls()
        {
            ItemNameTextBox.Text = "";
            ItemDescTextBox.Text = "";
            IncludingRadioButton.Checked = false;
            ExcludingRadioButton.Checked = false;
        }
        #endregion

    }
}